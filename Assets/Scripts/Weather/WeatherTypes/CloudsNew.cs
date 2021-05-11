using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRolijk.Weather.Type
{
    public class CloudsNew : BaseWeather
    {
        [SerializeField] Renderer[] cloudLayersRenderer;
        MaterialPropertyBlock[] mpbs;

        private float[] startCloudCutOffs, startCloudSoftnesss;

        private void Awake()
        {
            mpbs = new MaterialPropertyBlock[cloudLayersRenderer.Length];
            startCloudSoftnesss = new float[mpbs.Length];
            startCloudCutOffs = new float[mpbs.Length];

            for(int iLayer = 0; iLayer < mpbs.Length; iLayer++)
            {
                mpbs[iLayer] = new MaterialPropertyBlock();
                cloudLayersRenderer[iLayer].GetPropertyBlock(mpbs[iLayer]);
                
                startCloudCutOffs[iLayer] = mpbs[iLayer].GetFloat("_CloudCutoff");
                startCloudSoftnesss[iLayer] = mpbs[iLayer].GetFloat("_CloudSoftness");

                mpbs[iLayer].SetFloat("_CloudCutoff", 0);
                mpbs[iLayer].SetFloat("_CloudSoftness", 1);

                cloudLayersRenderer[iLayer].SetPropertyBlock(mpbs[iLayer]);
            }
        }

        private void FadeIn()
        {
            for (int iLayer = 0; iLayer < mpbs.Length; iLayer++)
            {
                cloudLayersRenderer[iLayer].GetPropertyBlock(mpbs[iLayer]);

                mpbs[iLayer].SetFloat("_CloudCutoff", startCloudCutOffs[iLayer]);
                mpbs[iLayer].SetFloat("_CloudSoftness", startCloudSoftnesss[iLayer]);

                cloudLayersRenderer[iLayer].SetPropertyBlock(mpbs[iLayer]);
            }
        }

        private void FadeOut()
        {
            for (int iLayer = 0; iLayer < mpbs.Length; iLayer++)
            {
                cloudLayersRenderer[iLayer].GetPropertyBlock(mpbs[iLayer]);

                mpbs[iLayer].SetFloat("_CloudCutoff", 0);
                mpbs[iLayer].SetFloat("_CloudSoftness", 1);

                cloudLayersRenderer[iLayer].SetPropertyBlock(mpbs[iLayer]);
            }
        }

        public override void Play()
        {
            base.Play();
        }

        public override void Stop()
        {
            base.Stop();

        }
    }
}
