using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRolijk.AI.BTree.Actions
{
    [AddComponentMenu("NPC/Action/Move To Target")]
    public class MoveToTarget : Node
    {
        [SerializeField] Transform[] targets;
        Transform currentTarget;

        public override void Init(BehaviorTree parent)
        {
            base.Init(parent);

            if (targets.Length <= 0)
            {
                targets = new Transform[transform.childCount];

                for (int iChild = 0; iChild < targets.Length; iChild++)
                {
                    targets[iChild] = transform.GetChild(iChild).transform;
                }
            }
        }

        public override NPCState Evaluate()
        {
            if (State != NPCState.Running && State != NPCState.Success &&
                !parent.npc.agent.pathPending)
            {

                parent.npc.agent.isStopped = false;

                currentTarget = targets[Random.Range(0, targets.Length)];
                parent.npc.agent.SetDestination(currentTarget.position);

                State = NPCState.Running;
            }

            if (parent.npc.agent.remainingDistance <= .1f &&
                State == NPCState.Running &&
                !parent.npc.agent.pathPending)
            {
                State = NPCState.Success;
            }

            return State;
        }
    }
}
