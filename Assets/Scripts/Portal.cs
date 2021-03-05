using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    public MeshRenderer screen;
    Camera playerCam, portalCam;
    RenderTexture viewTexture;

    MaterialPropertyBlock screenMpb;
    public MaterialPropertyBlock ScreenMpb => screenMpb;

    private void Awake()
    {
        playerCam = Camera.main;
        portalCam = GetComponentInChildren<Camera>();
    }

    void CreateViewTexture()
    {
        if(viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if(viewTexture != null)
            {
                viewTexture.Release();
            }

            viewTexture = new RenderTexture(Screen.width, Screen.height, 0);

            //Render the view from the portal camera to the view texture
            portalCam.targetTexture = viewTexture;

            //Display the view texture on the screen of the linked portal;
            linkedPortal.ScreenMpb.SetTexture("_BaseTexture", viewTexture);
        }
    }
}
