using UnityEngine;
using UnityEngine.Rendering;
using VRolijk.Portals;
using UnityEngine.Rendering.HighDefinition;

namespace VRolijk
{
    public class MainCamera : MonoBehaviour
    {

        [SerializeField] Portal[] portals;
        [SerializeField] Material skybox;

        void Awake()
        {
            portals = FindObjectsOfType<Portal>();
            RenderPipelineManager.beginCameraRendering += PreCull;
        }

        void Update()
        {
            if(RenderSettings.skybox == null)
            {
                Debug.Log("Was null");
                RenderSettings.skybox = skybox;
            }

            if (RenderSettings.skybox == null)
                Debug.Log("Still null");
        }

        void PreCull(ScriptableRenderContext context, Camera cam)
        {
            for (int i = 0; i < portals.Length; i++)
            {
                portals[i].Render(context, cam);
            }
        }

        private void OnDestroy()
        {
            RenderPipelineManager.beginCameraRendering -= PreCull;
        }
    }
}