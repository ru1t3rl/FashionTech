using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace VRolijk.AI.BTree.FlowControl
{
    [AddComponentMenu("NPC/Flow Control/Selector")]
    public class Selector : Node
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
            /// Loop through all the children
            ///     If Success or Running
            ///         return state
            /// return Failure

            for (int iChild = 0; iChild < childNodes.Count; iChild++)
            {
                childState = childNodes[iChild].Evaluate();

                if (childState == NPCState.Success || childState == NPCState.Running)
                {
                    return State = childState;
                }
            }

            ResetNodeState();

            return State = NPCState.Failure;
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