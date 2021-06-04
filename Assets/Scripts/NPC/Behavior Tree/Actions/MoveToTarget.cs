using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRolijk.AI.BTree.Actions
{
    public class MoveToTarget : Node
    {
        [SerializeField] Transform[] targets;
        Transform currentTarget;

        public override NPCState Evaluate()
        {
            if (parent.npc.agent.isStopped && 
               (currentTarget == null || 
                currentTarget.position == parent.npc.agent.transform.position))
            {
                currentTarget = targets[Random.Range(0, targets.Length)];
                parent.npc.agent.SetDestination(currentTarget.position);

                State = NPCState.Running;
            }

            if (parent.npc.agent.velocity.sqrMagnitude == 0.0f && 
                parent.npc.transform.position == currentTarget.position)
            {
                State = NPCState.Success;
            }
            else if (parent.npc.agent.velocity.sqrMagnitude == 0.0f)
            {
                State = NPCState.Failure;
            }

            return State;
        }
    }
}
