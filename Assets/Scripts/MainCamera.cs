using UnityEngine;
using UnityEngine.Rendering;
using VRolijk.Portals;
using UnityEngine.Rendering.Universal;

namespace VRolijk
{
    public class MainCamera : MonoBehaviour
    {

        [SerializeField] Portal[] portals;

        void Awake()
        {
            portals = FindObjectsOfType<Portal>();
            RenderPipelineManager.beginCameraRendering += PreCull;
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