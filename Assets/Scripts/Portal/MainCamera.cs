using UnityEngine;
using UnityEngine.Rendering;
using VRolijk.Portals;

namespace VRolijk
{
    public class MainCamera : MonoBehaviour
    {

        Portal[] portals;

        void Awake()
        {
            portals = FindObjectsOfType<Portal>();
            RenderPipelineManager.beginCameraRendering += PreCull;
        }

        void PreCull(ScriptableRenderContext context, Camera cam)
        {
            if (portals != null)
            {
                for (int i = 0; i < portals.Length; i++)
                {
                    portals[i].Render(context);
                }

                for (int i = 0; i < portals.Length; i++)
                {
                    // portals[i].PostPortalRender();
                }
            }
        }

    }
}