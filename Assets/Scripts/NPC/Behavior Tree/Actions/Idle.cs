using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace VRolijk.AI.BTree.Actions
{
    [AddComponentMenu("NPC/Action/Idle")]
    public class Idle : Node
    {
        [SerializeField]
        RandomNumber duration;
        float stopTime;

        public override NPCState Evaluate()
        {
            if (State != NPCState.Running)
            {
                stopTime = Time.time + duration.random;

                parent.npc.agent.isStopped = true;

                State = NPCState.Running;
            }

            if (Time.time >= stopTime)
            {
                State = NPCState.Success;

                parent.npc.agent.isStopped = false;
            }

            return State;
        }

        [System.Serializable]
        struct RandomNumber
        {
            public float min;
            public float max;

            public float random => Random.Range(min, max);
        }
    }
}
