using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace VRolijk.AI.BTree.FlowControl
{
    [AddComponentMenu("NPC/Flow Control/Sequence")]
    public class Sequence : Node
    {
        List<Node> childNodes = new List<Node>();
        NPCState childState;

        private void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                try
                {
                    childNodes.Add(transform.GetChild(i).GetComponent<Node>());
                }
                catch (System.Exception) { }
            }
        }

        public override void Init(BehaviorTree parent)
        {
            for (int iChild = 0; iChild < childNodes.Count; iChild++)
            {
                childNodes[iChild].Init(parent);
            }
        }

        public override NPCState Evaluate()
        {
            /// Loop through children
            ///     if running or failure
            ///         return state
            /// return success      

            for (int iChild = 0; iChild < childNodes.Count; iChild++)
            {
                childState = childNodes[iChild].Evaluate();

                if (childState == NPCState.Failure || childState == NPCState.Running)
                {
                    return State = childState;
                }
            }

            ResetNodeState();

            return State = NPCState.Success;
        }

        public override void ResetNodeState()
        {
            base.ResetNodeState();

            for (int iChild = 0; iChild < childNodes.Count; iChild++)
            {
                childNodes[iChild].ResetNodeState();
            }
        }
    }
}
