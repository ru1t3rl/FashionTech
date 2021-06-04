using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace VRolijk.AI.BTree
{
    [System.Serializable]
    public abstract class Node
    {
        [SerializeField] protected string name;

        public BehaviorTree parent { get; set; }
        public virtual void Init(BehaviorTree parent)
        {
            this.parent = parent;
        }

        public NPCState State { get; protected set; }

        public abstract NPCState Evaluate();
    }

    public enum NPCState
    {
        Idle,
        Running,
        Success,
        Failure
    }
}
