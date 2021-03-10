using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.XR.Interaction;
using UnityEngine.Events;

[CustomEditor(typeof(ColliderEvent))]
public class CollisionEvent_Editor : Editor
{
    static bool onTrigger = false;
    ColliderEvent events;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        /*
        if (events == null)
            events = (ColliderEvent)target;

        if (onTrigger)
        {
            EditorGUILayout.ObjectField(events.onCollisionEnter, typeof(UnityEvent));
        }
        */
    }
}
