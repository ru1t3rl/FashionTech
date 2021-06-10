using UnityEditor;
using UnityEngine;

namespace VRolijk.AI.BTree
{
    [System.Serializable]
    public class Node : MonoBehaviour
    {
        public string name;
        public BehaviorTree parent { get; set; }
        protected bool init = false;

        public virtual void Init(BehaviorTree parent)
        {
            this.parent = parent;
            init = true;
        }

        public NPCState State { get; protected set; }

        public virtual NPCState Evaluate() { throw new System.NotImplementedException(); }

        public virtual void ResetNodeState()
        {
            State = NPCState.Idle;
        }
    }

    public enum NPCState
    {
        Idle,
        Running,
        Success,
        Failure
    }
}
