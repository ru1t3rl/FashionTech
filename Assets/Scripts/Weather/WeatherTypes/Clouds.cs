using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRolijk.Weather.Type
{
    [ExecuteAlways]
    public class Clouds : BaseWeather
    {
        public int cloudsResolution = 20;
        public float cloudsHeight;

        public Mesh cloudMesh;
        public Material cloudMaterial;

        private float _offset;
        private Camera _sceneCamera;
        private Matrix4x4 _cloudsPosMatrix;

        public bool shadowCasting, shadowReceive, useLightProbes;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            _sceneCamera = Camera.main;
        }

        private void Update()
        {
            var currentTransform = transform;
            cloudMaterial.SetFloat("_CloudsWorldPos", currentTransform.position.y);
            cloudMaterial.SetFloat("_CloudHeight", cloudsHeight);
            _offset = cloudsHeight / cloudsResolution / 2f;

            var initPos = transform.position + (Vector3.up * (_offset * cloudsResolution / 2f));
            for (int i = 0; i < cloudsResolution; i++)
            {
                // Take into consideration translation, rotation, scale of clouds object
                _cloudsPosMatrix = Matrix4x4.TRS(initPos - (Vector3.up * _offset * i),
                                                currentTransform.rotation,
                                                currentTransform.localScale);

                Matrix4x4[] matrices = { _cloudsPosMatrix };
                // Push the mesh dat to render without editor overhead of managing multiple objects
                //Graphics.DrawMeshNow(cloudMesh, _cloudsPosMatrix, cloudMaterial, 0, _sceneCamera, 0,
                //                  null, shadowCasting, shadowReceive, useLightProbes);

                Graphics.DrawMeshInstanced(cloudMesh, 0, cloudMaterial,  matrices, matrices.Length);
            }
        }
    }
}
