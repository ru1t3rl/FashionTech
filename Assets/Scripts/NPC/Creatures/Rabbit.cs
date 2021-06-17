using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRolijk.AI
{
    [RequireComponent(typeof(Animator))]
    public class Rabbit : NPC
    {
        [Header("Animation")]
        [SerializeField] Animator animator;
        [SerializeField] AudioSource moveSource;

        protected override void Update()
        {
            base.Update();

            float speed = agent.velocity.sqrMagnitude;

            animator.SetFloat("Speed", speed);

            if ((speed > 0 || speed < 0) && !moveSource.isPlaying)
            {
                moveSource.Play();
            }
            else if (speed == 0 && moveSource.isPlaying)
            {
                moveSource.Stop();
            }
        }
    }
}