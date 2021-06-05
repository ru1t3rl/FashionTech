using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRolijk.AI.BTree.FlowControl
{
    public class Selector : Node
    {
        public List<Node> children = new List<Node>();

        NPCState childState;

        public override void Init(BehaviorTree parent)
        {
            for (int iChild = 0; iChild < children.Count; iChild++)
            {
                children[iChild].Init(parent);
            }
        }

        public override NPCState Evaluate()
        {
            /// Loop through all the children
            ///     If Success or Running
            ///         return state
            /// return Failure

            for(int iChild = 0; iChild < children.Count; iChild++)
            {
                childState = children[iChild].Evaluate();
                
                if (childState == NPCState.Success || childState == NPCState.Running)
                {
                    return State = childState;
                }
            }

            return State = NPCState.Failure;
        }
    }
        
}