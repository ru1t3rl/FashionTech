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

        protected override void Update()
        {
            base.Update();

            animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        }
    }
}