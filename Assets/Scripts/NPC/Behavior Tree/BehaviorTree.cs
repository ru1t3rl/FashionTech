using System.Collections.Generic;
using UnityEngine;
using VRolijk.AI.BTree.FlowControl;

namespace VRolijk.AI.BTree
{
    [RequireComponent(typeof(NPC))]
    [CreateAssetMenu(fileName = "Behavior Tree", menuName = "NPC/Behavior Tree", order = 0)]
    [System.Serializable]
    public class BehaviorTree : ScriptableObject
    {
        [SerializeField] string title;
        
        public List<Node> children = new List<Node>();
        public NPC npc { get; private set; }
        

        public virtual void Init(NPC npc)
        {
            this.npc = npc;
            
            for (int iChild = 0; iChild < children.Count; iChild++)
            {
                children[iChild].Init(this);
            }
        }

        public virtual void Update()
        {
            for (int iChild = 0; iChild < children.Count; iChild++)
            {
                children[iChild].Evaluate();
            }
        }
    }
}
