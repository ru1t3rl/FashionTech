using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class BreathingExercise : MonoBehaviour
{
    [Header("Calculations")]
    [SerializeField] float minDeltaYLeft;
    [SerializeField] float maxDeltaYRight;
    Vector3[] previousHandPos;
    [SerializeField] float sensitivity = 100f;

    [Header("Audio")]
    [SerializeField] int playOnNTimes = 3;
    int nTime = 0;
    [SerializeField] AudioSource instructionSource;
    [SerializeField] AudioClip encourageAudio;
    [SerializeField] AudioClip hintAudio;

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
        bool left = false, right = false;
        for (int iHand = 0; iHand < previousHandPos.Length; iHand++)
        {
            try
            {
                Vector3 deltaY = Vector3.Scale(Player.instance.hands[iHand].transform.up, Player.instance.hands[iHand].transform.position) -
                                 Vector3.Scale(Player.instance.hands[iHand].transform.up, previousHandPos[iHand]);

                Debug.Log($"Magnitude {Player.instance.hands[iHand].handType} {deltaY.sqrMagnitude * sensitivity}");

                if (Player.instance.hands[iHand].handType == SteamVR_Input_Sources.LeftHand && deltaY.sqrMagnitude * sensitivity >= minDeltaYLeft)
                {
                    left = true;
                }
                else if (Player.instance.hands[iHand].handType == SteamVR_Input_Sources.RightHand && deltaY.sqrMagnitude * sensitivity <= maxDeltaYRight)
                {
                    right = true;
                }
            }
            catch (IndexOutOfRangeException) { }
        }

        if (nTime % playOnNTimes == 0)
        {
            if (right && left)
            {
                if (encourageAudio != null && !instructionSource.isPlaying)
                {
                    instructionSource.Stop();
                    instructionSource.clip = encourageAudio;
                    instructionSource.Play();
                }
            }
            else
            {
                if (hintAudio != null && !instructionSource.isPlaying)
                {
                    instructionSource.Stop();
                    instructionSource.clip = hintAudio;
                    instructionSource.Play();
                }
            }
        }

        nTime++;
    }

    public void DetectBreathingThroughBelly()
    {

    }
}
