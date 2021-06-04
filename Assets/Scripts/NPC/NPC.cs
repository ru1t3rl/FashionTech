using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VRolijk.AI.BTree;

namespace VRolijk.AI
{
    public class NPC : MonoBehaviour
    {
        [SerializeField]
        protected BehaviorTree behaviorTree;
        public NavMeshAgent agent;

        protected virtual void Awake()
        {
            behaviorTree.Init(this);
        }

        protected virtual void Update()
        {
            behaviorTree.Update();
        }
    }
}
