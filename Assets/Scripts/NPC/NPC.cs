using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VRolijk.AI.BTree;

namespace VRolijk.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPC : MonoBehaviour
    {
        [Header("General")]
        public NavMeshAgent agent;

        [SerializeField]
        protected BehaviorTree behaviorTree;

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
