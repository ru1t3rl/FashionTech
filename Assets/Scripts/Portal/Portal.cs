﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace VRolijk.Portals
{
    public class Portal : MonoBehaviour
    {
        [Header("Main Settings")]
        public Portal linkedPortal;
        public MeshRenderer screen;
        public int recursionLimit = 5;

        [Header("Advanced Settings")]
        public float nearClipOffset = 0.05f;
        public float nearClipLimit = 0.2f;

        // Private variables
        RenderTexture viewTexture;
        Camera portalCam;
        Camera playerCam;
        Material firstRecursionMat;
        MeshFilter screenMeshFilter;

        void Awake()
        {
            playerCam = Camera.main;
            portalCam = GetComponentInChildren<Camera>();
            portalCam.enabled = false;
            screenMeshFilter = screen.GetComponent<MeshFilter>();
            screen.material.SetInt("displayMask", 1);
        }

        // Manually render the camera attached to this portal
        // Called after PrePortalRender, and before PostPortalRender
        public void Render(ScriptableRenderContext context)
        {

            // Skip rendering the view from this portal if player is not looking at the linked portal
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
                    // No need for recursive rendering if linked portal is not visible through this portal
                    if (!CameraUtility.BoundsOverlap(screenMeshFilter, linkedPortal.screenMeshFilter, portalCam))
                    {
                        break;
                    }
                }
                localToWorldMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * localToWorldMatrix;
                int renderOrderIndex = recursionLimit - i - 1;
                renderPositions[renderOrderIndex] = localToWorldMatrix.GetColumn(3);
                renderRotations[renderOrderIndex] = localToWorldMatrix.rotation;

                portalCam.transform.SetPositionAndRotation(renderPositions[renderOrderIndex], renderRotations[renderOrderIndex]);
                startIndex = renderOrderIndex;
            }

            // Hide screen so that camera can see through portal
            screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            linkedPortal.screen.material.SetInt("displayMask", 0);

            for (int i = startIndex; i < recursionLimit; i++)
            {
                portalCam.transform.SetPositionAndRotation(renderPositions[i], renderRotations[i]);
                SetNearClipPlane();
                UniversalRenderPipeline.RenderSingleCamera(context, portalCam);

                if (i == startIndex)
                {
                    linkedPortal.screen.material.SetInt("displayMask", 1);
                }
            }

            // Unhide objects hidden at start of render
            screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }



        // Called once all portals have been rendered, but before the player camera renders
        public void PostPortalRender()
        {
            ProtectScreenFromClipping(playerCam.transform.position);
        }
        void CreateViewTexture()
        {
            if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
            {
                if (viewTexture != null)
                {
                    viewTexture.Release();
                }
                viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
                // Render the view from the portal camera to the view texture
                portalCam.targetTexture = viewTexture;
                // Display the view texture on the screen of the linked portal
                linkedPortal.screen.material.SetTexture("_MainTex", viewTexture);
            }
        }

        // Sets the thickness of the portal screen so as not to clip with camera near plane when player goes through
        float ProtectScreenFromClipping(Vector3 viewPoint)
        {
            float halfHeight = playerCam.nearClipPlane * Mathf.Tan(playerCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float halfWidth = halfHeight * playerCam.aspect;
            float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, playerCam.nearClipPlane).magnitude;
            float screenThickness = dstToNearClipPlaneCorner;

            Transform screenT = screen.transform;
            bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - viewPoint) > 0;
            screenT.localScale = new Vector3(screenT.localScale.x, screenT.localScale.y, screenThickness);
            screenT.localPosition = Vector3.forward * screenThickness * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
            return screenThickness;
        }

        // Use custom projection matrix to align portal camera's near clip plane with the surface of the portal
        // Note that this affects precision of the depth buffer, which can cause issues with effects like screenspace AO
        void SetNearClipPlane()
        {
            // Learning resource:
            // http://www.terathon.com/lengyel/Lengyel-Oblique.pdf
            Transform clipPlane = transform;
            int dot = System.Math.Sign(Vector3.Dot(clipPlane.forward, transform.position - portalCam.transform.position));

            Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint(clipPlane.position);
            Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector(clipPlane.forward) * dot;
            float camSpaceDst = -Vector3.Dot(camSpacePos, camSpaceNormal) + nearClipOffset;

            // Don't use oblique clip plane if very close to portal as it seems this can cause some visual artifacts
            if (Mathf.Abs(camSpaceDst) > nearClipLimit)
            {
                Vector4 clipPlaneCameraSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

                // Update projection based on new clip plane
                // Calculate matrix with player cam so that player camera settings (fov, etc) are used
                portalCam.projectionMatrix = playerCam.CalculateObliqueMatrix(clipPlaneCameraSpace);
            }
            else
            {
                portalCam.projectionMatrix = playerCam.projectionMatrix;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            // Add the Traveller to a list
        }

        void OnTriggerExit(Collider other)
        {
            // Remove Traveller from a list
        }

        /*
         ** Some helper/convenience stuff:
         */

        int SideOfPortal(Vector3 pos)
        {
            return System.Math.Sign(Vector3.Dot(pos - transform.position, transform.forward));
        }

        bool SameSideOfPortal(Vector3 posA, Vector3 posB)
        {
            return SideOfPortal(posA) == SideOfPortal(posB);
        }

        Vector3 portalCamPos
        {
            get
            {
                return portalCam.transform.position;
            }
        }

        void OnValidate()
        {
            if (linkedPortal != null)
            {
                linkedPortal.linkedPortal = this;
            }
        }
    }
}