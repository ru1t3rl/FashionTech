using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRolijk
{
    [RequireComponent(typeof(ReflectionProbe))]
    public class ReflectionProbeUpdater : MonoBehaviour
    {
        ReflectionProbe reflectionProbe;
        int renderId = -1;

        [Header("Update Settings")]
        [SerializeField] int framesBetweenNextProbeUpdate = 10;
        int frames = 0;

        [Header("Movement based")]
        [SerializeField] bool updateOnMove;
        [SerializeField] WalkInPlace walkInPlace;

        [Header("Time-Based")]
        [SerializeField] bool updateBasedOnTime;
        [SerializeField, Tooltip("Time in seconds")] float waitTime;
        Coroutine updateOnTime;

        RenderTexture targetTexture;

        private void OnEnable()
        {
            if (reflectionProbe == null)
                reflectionProbe = GetComponent<ReflectionProbe>();

            renderId = reflectionProbe.RenderProbe(targetTexture = null);

            if (updateOnTime != null)
                StopCoroutine(updateOnTime);

            if (updateBasedOnTime)
                updateOnTime = StartCoroutine(UpdateEverySeconds());
        }

        private void OnDisable()
        {
            if (updateOnTime != null)
                StopCoroutine(updateOnTime);
        }

        private void Update()
        {
            if (reflectionProbe.refreshMode != UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting || updateBasedOnTime)
                return;

            if (updateOnMove)
            {
                UpdateOnMove();
            }
            else
            {
                UpdateOnNFrames();
            }
        }

        // Every Number of frames
        private void UpdateOnNFrames()
        {
            frames++;

            if (frames % framesBetweenNextProbeUpdate == 0 &&
               (renderId == -1 || reflectionProbe.IsFinishedRendering(renderId)) &&
               walkInPlace.state == MoveState.Idle)
            {
                renderId = reflectionProbe.RenderProbe(targetTexture = null);
            }
        }

        // When the player is moving
        private void UpdateOnMove()
        {
            // When the players is moving and n frames has pased and we have finished rendering
            if (walkInPlace.state == MoveState.Walking &&
                (renderId == -1 || reflectionProbe.IsFinishedRendering(renderId)))
            {
                renderId = reflectionProbe.RenderProbe(targetTexture = null);
            }
        }

        IEnumerator UpdateEverySeconds()
        {
            while (true)
            {
                yield return new WaitForSeconds(waitTime);

                if (walkInPlace.state == MoveState.Idle)
                    renderId = reflectionProbe.RenderProbe(targetTexture = null);
            }
        }
    }
}
