using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using VRolijk.AI.BTree.FlowControl;

namespace VRolijk.AI.BTree
{
    public class BehaviorTree : MonoBehaviour
    {
        [SerializeField] string title;
        List<Node> childNodes = new List<Node>();

        public NPC npc { get; private set; }

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

        public virtual void Init(NPC npc)
        {
            this.npc = npc;

            for (int iChild = 0; iChild < childNodes.Count; iChild++)
            {
                childNodes[iChild].Init(this);
            }
        }

        public virtual void Update()
        {
            for (int iChild = 0; iChild < childNodes.Count; iChild++)
            {
                childNodes[iChild].Evaluate();
            }
        }
    }
}
