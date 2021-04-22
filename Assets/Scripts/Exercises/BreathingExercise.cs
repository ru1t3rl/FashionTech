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
    [SerializeField] float maxTotalControllerMovement;

    [Header("Audio")]
    [SerializeField] int attemptsBeforeHint = 3;
    int attempts = 0;
    [SerializeField] AudioSource instructionSource;
    [SerializeField] AudioClip encourageAudio;
    [SerializeField] AudioClip hintAudio;

    [Header("Visuals")]
    [SerializeField] float minVisualSize;
    [SerializeField] float maxVisualSize;
    [SerializeField] Animation animation;
    [SerializeField] AnimationClip breathIn, breathOut;

    bool active = false;
    public bool IsActive => active;

    private void Awake()
    {
        previousHandPos = new Vector3[Player.instance.handCount];


        //animation.AddClip(breathIn, breathIn.name);        
        //animation.AddClip(breathOut, breathOut.name);
    }

    public void OnBreathIn()
    {
        animation.Play("BreathIn");

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
        animation.Play("BreathOut");

        bool left = false, right = false;
        for (int iHand = 0; iHand < previousHandPos.Length; iHand++)
        {
            try
            {
                Vector3 deltaY = Vector3.Scale(Player.instance.hands[iHand].transform.up, Player.instance.hands[iHand].transform.position) -
                                 Vector3.Scale(Player.instance.hands[iHand].transform.up, previousHandPos[iHand]);

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

        if (attempts % attemptsBeforeHint == 0)
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

        attempts++;
    }

    /*
    bool breathIn = false;
    public void SetVisualTime(float duration)
    {
        breathIn = !breathIn;
       
        visual.transform.localScale = Vector3.SmoothDamp(transform.localScale, breathIn ? new Vector3(maxVisualSize, maxVisualSize, maxVisualSize) :
                                                                                          new Vector3(minVisualSize, minVisualSize, minVisualSize),
                                                                                          ref velocity, duration*60);
    }
    */

    public void Play()
    {
        active = true;
    }

    public void Stop()
    {
        active = false;
    }

    public void DetectControllerPosition()
    {

    }
}
