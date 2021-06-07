using UnityEditor;
using UnityEngine;

namespace VRolijk.AI.BTree
{
    [System.Serializable]
    public class Node : MonoBehaviour
    {
        public string name;

        public BehaviorTree parent { get; set; }
        public virtual void Init(BehaviorTree parent)
        {
            this.parent = parent;
        }

        public NPCState State { get; protected set; }

        public virtual NPCState Evaluate() { throw new System.NotImplementedException(); }


        [MenuItem("GameObject/NPC/Behavior Tree", false)]
        public virtual void Create()
        {
            Instantiate(new GameObject(), Vector3.zero, Quaternion.identity).AddComponent<Node>();
        }

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
