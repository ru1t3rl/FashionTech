using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRolijk.AI.BTree.Actions;
using VRolijk.AI.BTree.FlowControl;

namespace VRolijk.AI.BTree
{
    [CustomEditor(typeof(BehaviorTree))]
    public class BehaviorTreeEditor : Editor
    {
        public BehaviorTree asset { get; private set; }
        ENodeType selectedType;
        bool visible = false;

        private int indentLevel = 0;
        private const int TAB_SIZE = 20;
        string childType = string.Empty;



        private void OnEnable()
        {
            asset = target as BehaviorTree;
        }

        public override void OnInspectorGUI()
        {
            if (asset == null)
            {
                asset = target as BehaviorTree;

                if (asset == null)
                    return;
            }

            if (asset.children.Count > 0)
            {
                EditorGUILayout.BeginHorizontal();

                // Draw Titles
                indentLevel = 0;
                DrawElement(asset.children);

                // Draw Remove Buttons
                EditorGUILayout.BeginVertical();
                DrawRemove(asset.children);
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();
            }
            else if (asset.children.Count <= 0)
            {
                ShowAdd();
            }
        }

        void DrawRemove(List<Node> nodes)
        {
            for (int iNode = 0; iNode < nodes.Count; iNode++)
            {
                // Remove Button it self
                if (GUILayout.Button("-"))
                {
                    nodes.Remove(nodes[iNode]);
                }

                // Draw children recursive
                if (nodes[iNode].GetType() == typeof(Sequence))
                {
                    Sequence s = nodes[iNode] as Sequence;
                    DrawRemove(s.children);
                }
                else if (nodes[iNode].GetType() == typeof(Selector))
                {
                    Selector s = nodes[iNode] as Selector;
                    DrawRemove(s.children);
                }
            }
        }

        void DrawElement(List<Node> nodes)
        {
            indentLevel++;
            EditorGUI.indentLevel = indentLevel;

            for (int iChild = 0; iChild < nodes.Count; iChild++)
            {
                EditorGUILayout.BeginVertical();
                if (nodes[iChild].GetType() == typeof(Selector))
                {
                    // Draw node information
                    EditorGUILayout.BeginHorizontal();

                    #region Indent
                    GUILayout.Space(TAB_SIZE * indentLevel);

                    /*
                    if (indentLevel > 0)
                    {
                        GUILayout.Label(GUILayoutIndentString(indentLevel));
                    }
                    */
                    #endregion

                    childType = nodes[iChild].GetType().ToString();
                    GUILayout.Label(childType.Split('.')[childType.Split('.').Length - 1]);

                    ShowAdd(false, nodes[iChild]);

                    GUILayout.Space(5);

                    EditorGUILayout.EndHorizontal();

                    // Draw possible recursive children 
                    Selector child = (Selector)nodes[iChild];
                    DrawElement(child.children);
                }
                else if (nodes[iChild].GetType() == typeof(Sequence))
                {
                    // Draw node information
                    EditorGUILayout.BeginHorizontal();

                    #region Indent
                    GUILayout.Space(TAB_SIZE * indentLevel);

                    /*
                    if (indentLevel > 0)
                    {
                        GUILayout.Label(GUILayoutIndentString(indentLevel));
                    }
                    */
                    #endregion

                    childType = nodes[iChild].GetType().ToString();
                    GUILayout.Label(childType.Split('.')[childType.Split('.').Length - 1]);

                    ShowAdd(false, nodes[iChild]);

                    GUILayout.Space(5);

                    EditorGUILayout.EndHorizontal();

                    // Draw possible recursive children 
                    Sequence child = (Sequence)nodes[iChild];
                    DrawElement(child.children);
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();

                    #region Indent
                    GUILayout.Space(TAB_SIZE * indentLevel);

                    /*
                    if (indentLevel > 0)
                    {
                        GUILayout.Label(GUILayoutIndentString(indentLevel));
                    }
                    */
                    #endregion

                    childType = nodes[iChild].GetType().ToString();
                    GUILayout.Label(childType.Split('.')[childType.Split('.').Length - 1]);

                    GUILayout.Space(5);

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
            }

            indentLevel--;
        }

        void ShowRemove(bool isChild = false, Node node = null)
        {
            if (GUILayout.Button("-"))
            {
                asset.children.Remove(node);
            }
        }

        void ShowAdd(bool isChild = false, Node node = null)
        {
            if ((!visible || (visible && node != null && !node.isSelected)) && GUILayout.Button("Add"))
            {
                if (node != null)
                {
                    node.isSelected = true;
                }

                visible = true;
            }

            if (visible && (node == null || (node != null && node.isSelected)))
            {
                if (!isChild)
                    GUILayout.BeginHorizontal();

                // Show drop down of enum NodeTypes
                EditorGUI.indentLevel = indentLevel + 1;
                selectedType = (ENodeType)EditorGUILayout.EnumPopup("", selectedType);

                if (GUILayout.Button("+"))
                {
                    if (node == null)
                    {
                        switch (selectedType)
                        {
                            case ENodeType.Idle:
                                asset.children.Add(new Idle());
                                break;
                            case ENodeType.MoveTarget:
                                asset.children.Add(new MoveToTarget());
                                break;
                            case ENodeType.Selector:
                                asset.children.Add(new Selector());
                                break;
                            case ENodeType.Sequence:
                                asset.children.Add(new Sequence());
                                break;
                        }
                    }
                    else if (node != null)
                    {
                        if (node.GetType() == typeof(Sequence))
                        {
                            Sequence s = node as Sequence;

                            switch (selectedType)
                            {
                                case ENodeType.Idle:
                                    s.children.Add(new Idle());
                                    break;
                                case ENodeType.MoveTarget:
                                    s.children.Add(new MoveToTarget());
                                    break;
                                case ENodeType.Selector:
                                    s.children.Add(new Selector());
                                    break;
                                case ENodeType.Sequence:
                                    s.children.Add(new Sequence());
                                    break;
                            }
                        }
                        else if (node.GetType() == typeof(Selector))
                        {
                            Selector s = node as Selector;
                            switch (selectedType)
                            {
                                case ENodeType.Idle:
                                    s.children.Add(new Idle());
                                    break;
                                case ENodeType.MoveTarget:
                                    s.children.Add(new MoveToTarget());
                                    break;
                                case ENodeType.Selector:
                                    s.children.Add(new Selector());
                                    break;
                                case ENodeType.Sequence:
                                    s.children.Add(new Sequence());
                                    break;
                            }
                        }
                    }

                    visible = false;

                    if (node != null)
                    {
                        node.isSelected = false;
                    }

                    EditorGUI.indentLevel = indentLevel;
                }

                if (!isChild)
                    GUILayout.EndHorizontal();
            }
        }

        string GUILayoutIndentString(int indentLevel)
        {
            string indent = "|";

            for (int i = 0; i < indentLevel; i++)
            {
                indent += ("_");

            }

            return indent;
        }

        void GUILayoutIndent(int indentLevel)
        {
            GUILayout.Space(TAB_SIZE * indentLevel);
        }
    }
}