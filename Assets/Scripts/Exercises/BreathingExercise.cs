using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class BreathingExercise : MonoBehaviour
{
    [SerializeField] float minDeltaY;
    Vector3[] previousHandPos;
    [SerializeField] float sensitivity = 100f;

    private void Awake()
    {
        previousHandPos = new Vector3[Player.instance.handCount];
    }

    public void OnBreathIn()
    {
        for (int iHand = 0; iHand < previousHandPos.Length; iHand++)
        {
            try
            {                
                previousHandPos[iHand] = new Vector3(Player.instance.hands[iHand].transform.position.x, Player.instance.hands[iHand].transform.position.y, Player.instance.hands[iHand].transform.position.z);
            }
            catch (IndexOutOfRangeException) { }
        }
    }

    public void OnBreathOut()
    {
        for (int iHand = 0; iHand < previousHandPos.Length; iHand++)
        {
            try
            {
                Vector3 deltaY = Vector3.Scale(Player.instance.hands[iHand].transform.up, Player.instance.hands[iHand].transform.position) -
                                 Vector3.Scale(Player.instance.hands[iHand].transform.up, previousHandPos[iHand]);

                /*
                Debug.Log($"Top: Hand ({Player.instance.hands[iHand].gameObject.name}): y-position {Player.instance.hands[iHand].transform.position.y}");


                Debug.Log($"Bottom: Hand ({Player.instance.hands[iHand].gameObject.name}): y-position {previousHandPos[iHand].y}");
                Debug.Log("Difference: " + (Player.instance.hands[iHand].transform.position.y * sensitivity - previousHandPos[iHand].y * sensitivity));
                */

                if (deltaY.sqrMagnitude >= minDeltaY * minDeltaY)
                {
                    Debug.Log("<b>[Breathing Exercise]</b> Hey you're doing great");
                }
                else
                {
                    Debug.Log("<b>[Breathing Exercise]</b> Try to breath more through your belly");
                }
            }
            catch (IndexOutOfRangeException) { }
        }
    }

    public void DetectBreathingThroughBelly()
    {

    }
}
