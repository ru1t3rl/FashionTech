using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRolijk.AI.BTree.FlowControl
{
    [System.Serializable]
    public class Sequence : Node
    {
        [SerializeField] Node[] children;
        NPCState childState;

        public override void Init(BehaviorTree parent)
        {
            for (int iChild = 0; iChild < children.Length; iChild++)
            {
                children[iChild].Init(parent);
            }
        }

        public override NPCState Evaluate()
        {
            /// Loop through children
            ///     if running or failure
            ///         return state
            /// return success      

            for (int iChild = 0; iChild < children.Length; iChild++)
            {
                childState = children[iChild].Evaluate();

                if (childState == NPCState.Failure || childState == NPCState.Running)
                {
                    return State = childState;
                }
            }

            return State = NPCState.Success;
        }
    }
}
