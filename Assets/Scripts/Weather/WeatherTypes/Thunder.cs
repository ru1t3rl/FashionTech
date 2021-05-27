using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRolijk.Weather.Type
{
    public class Thunder : BaseWeather
    {
        [Header("Time Settings")]
        [SerializeField, Tooltip("Time in seconds | X: Min, Y: Max")]
        Vector2 randomIntervalTime;
        float timeToPlayAgain;

        [Header("Location Settings")]
        [SerializeField]
        Vector3 minRandomPosition, maxRandomPosition;

        Coroutine effectRoutine;

        public override void Play()
        {
            base.Play();

            effectRoutine = StartCoroutine(PlayWithInterval(
                                               Random.Range(randomIntervalTime.x,
                                                            randomIntervalTime.y)));
        }

        public override void Stop()
        {
            base.Stop();

            if(effectRoutine != null)
            {
                StopCoroutine(effectRoutine);
            }
        }

        IEnumerator PlayWithInterval(float intervalTime)
        {
            yield return new WaitForSeconds(intervalTime);            

            // Set random location for the effect
            weatherEffect.transform.position = new Vector3(
                Random.Range(minRandomPosition.x, maxRandomPosition.x),
                Random.Range(minRandomPosition.y, maxRandomPosition.y),
                Random.Range(minRandomPosition.z, maxRandomPosition.z)
                );

            // Show the effect
            weatherEffect.Play();

            // Activate the effect again
            PlayWithInterval(Random.Range(randomIntervalTime.x, randomIntervalTime.y));
        }
    }
}