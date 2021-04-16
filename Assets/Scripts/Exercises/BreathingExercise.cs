using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class BreathingExercise : MonoBehaviour
{
    [SerializeField] float minYDelta;
    Transform[] handBaseTransforms;

    private void Awake()
    {
        handBaseTransforms = new Transform[Player.instance.handCount];
    }

    public void OnBreathIn()
    {
        for (int iHand = 0; iHand < handBaseTransforms.Length; iHand++)
        {
            try { handBaseTransforms[iHand] = Player.instance.hands[iHand].transform; } catch (IndexOutOfRangeException) { }
        }
    }

    public void OnBreathOut()
    {
        for (int iHand = 0; iHand < handBaseTransforms.Length; iHand++)
        {
            try
            {
                Vector3 deltaY = Vector3.Scale(Player.instance.hands[iHand].transform.up, Player.instance.hands[iHand].transform.position) -
                                 Vector3.Scale(handBaseTransforms[iHand].up, handBaseTransforms[iHand].position);

                Debug.Log("deltaY sqrMagnitude: " + deltaY.sqrMagnitude);

                if (deltaY.sqrMagnitude >= minYDelta * minYDelta)
                {
                    Debug.Log("Hey you're doing great");
                }
                else
                {
                    Debug.Log("Try to breath more through your belly");
                }
            }
            catch (IndexOutOfRangeException) { }
        }
    }

    public void DetectBreathingThroughBelly()
    {

    }
}
