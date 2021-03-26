using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace VRolijk.Portals
{
    public class Portal : MonoBehaviour
    {
        [Header("Main Settings")]
        [SerializeField] MeshRenderer screen;
        public Portal linkedPortal;
        [SerializeField] RenderTexture targetRenderTexture;
        [SerializeField] bool useCustomViewTextureSize = true;
        [SerializeField] Vector2Int viewTextureSize = new Vector2Int(512, 512);

        [Header("Traveller Settings")]
        [SerializeField] float _teleportOffset = 1f;
        public float TeleportOffset => _teleportOffset;
        [SerializeField] int recursionLimit = 1;

        [SerializeField] LayerMask travellerLayer;
        [SerializeField] float cameraOffset = 1.5f;


        Camera playerCam, portalCam;
        MaterialPropertyBlock _screenMpb;
        public MaterialPropertyBlock screenMpb => _screenMpb;
        MeshFilter screenMeshFilter;

        private void Awake()
        {
            playerCam = Camera.main;
            portalCam = GetComponentInChildren<Camera>();

            _screenMpb = new MaterialPropertyBlock();
            screenMeshFilter = screen.GetComponent<MeshFilter>();
        }

        // Called just before player camera is rendered
        public void Render(ScriptableRenderContext context, Camera cam)
        {
            if (!CameraUtility.VisibleFromCamera(linkedPortal.screen, playerCam))
            {
                return;
            }

            CreateViewTexture();

            var localToWorldMatrix = playerCam.transform.localToWorldMatrix;
            var renderPositions = new Vector3[recursionLimit];
            var renderRotations = new Quaternion[recursionLimit];

            int startIndex = 0;
            portalCam.projectionMatrix = playerCam.projectionMatrix;
            for (int i = 0; i < recursionLimit; i++)
            {
                if (i > 0)
                {
                    if (!CameraUtility.BoundsOverlap(screenMeshFilter, linkedPortal.screenMeshFilter, portalCam))
                    {
                        break;
                    }
                }

                // Calculate position and orientation
                localToWorldMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * localToWorldMatrix;
                int renderOrderIndex = recursionLimit - i - 1;
                renderPositions[renderOrderIndex] = localToWorldMatrix.GetPosition();
                //renderPositions[renderOrderIndex] = new Vector3(portalCam.transform.position.x, playerCam.transform.position.y / cameraOffset, portalCam.transform.position.z);
                renderRotations[renderOrderIndex] = localToWorldMatrix.rotation;

                portalCam.transform.SetPositionAndRotation(renderPositions[renderOrderIndex], renderRotations[renderOrderIndex]);
                //portalCam.transform.position = new Vector3(0, portalCam.transform.localPosition.y, 0);
                startIndex = renderOrderIndex;
            }


            // Hide screen so that camera can see through portal
            screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            linkedPortal.SetInt("displayMask", 0);

            for (int i = startIndex; i < recursionLimit; i++)
            {
                portalCam.transform.SetPositionAndRotation(renderPositions[i], renderRotations[i]);
                UniversalRenderPipeline.RenderSingleCamera(context, portalCam);

                if (i == startIndex)
                {
                    linkedPortal.SetInt("displayMask", 1);
                }
            }

            // Unhide objects hidden at start of render
            screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }

        void CreateViewTexture()
        {
            if (targetRenderTexture == null ||
                (!useCustomViewTextureSize && (targetRenderTexture.width != Screen.width || targetRenderTexture.height != Screen.height)) ||
                (useCustomViewTextureSize && (targetRenderTexture.width != viewTextureSize.x || targetRenderTexture.height != viewTextureSize.y)))
            {

                if (targetRenderTexture != null)
                {
                    targetRenderTexture.Release();
                }

                if (!useCustomViewTextureSize)
                {
                    targetRenderTexture = new RenderTexture(Screen.width, Screen.height, 32);
                }
                else
                {
                    targetRenderTexture = new RenderTexture(viewTextureSize.x, viewTextureSize.y, 32);
                }

                targetRenderTexture.name = gameObject.name;
                targetRenderTexture.Create();
            }

            //Render the view from the portal camera to the view texture
            portalCam.targetTexture = targetRenderTexture;

            //Display the view texture on the screen of the linked portal;
            linkedPortal.SetTexture("_BaseMap", targetRenderTexture);
        }

        #region Traveling
        void OnTravellerEnterPortal(GameObject traveller)
        {
            Vector3 direction = new Vector3(linkedPortal.portalCam.transform.forward.x, 0, linkedPortal.portalCam.transform.forward.z);

            if (traveller.name.ToLower() != "headcollider")
                traveller.transform.position = linkedPortal.transform.position + direction * linkedPortal.TeleportOffset;
            else
                traveller.transform.parent.parent.parent.parent.position = linkedPortal.transform.position + direction * linkedPortal.TeleportOffset;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == travellerLayer.ToInteger())
            {
                OnTravellerEnterPortal(other.gameObject);
            }
        }
        #endregion

        public void SetTexture(string name, Texture texture)
        {
            screen.GetPropertyBlock(screenMpb);
            screenMpb.SetTexture(name, texture);
            screen.SetPropertyBlock(screenMpb);
        }

        public void SetInt(string name, int value)
        {
            screen.GetPropertyBlock(screenMpb);
            screenMpb.SetInt(name, value);
            screen.SetPropertyBlock(screenMpb);
        }
    }
}