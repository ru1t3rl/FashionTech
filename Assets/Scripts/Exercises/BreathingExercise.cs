using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;
using VRolijk.Excercises;

[RequireComponent(typeof(ExerciseBase))]
public class BreathingExercise : MonoBehaviour
{
    [SerializeField] ExerciseBase baseExercise;

    [Header("Calculations")]
    [SerializeField] float minDeltaYLeft;
    [SerializeField] float maxDeltaYRight;
    Vector3[] previousHandPos;
    [SerializeField] float sensitivity = 100f;
    [SerializeField] float overallMaxTotalControllerMovement;

    [Header("Audio")]
    [SerializeField] int attemptsBeforeHint = 3;
    int attempts = 0;
    [SerializeField] AudioSource instructionSource;
    [SerializeField] AudioClip encourageAudio;
    [SerializeField] AudioClip hintAudio;
    [SerializeField] AudioClip tryAgainAudio;

    [Header("Visuals")]
    [SerializeField] float minVisualSize;
    [SerializeField] float maxVisualSize;
    [SerializeField] Animation animation;
    [SerializeField] AnimationClip breathIn, breathOut;
    Vector3 visualizationStartScale;

    bool active = false;
    public bool IsActive => active;

    private void Awake()
    {
        previousHandPos = new Vector3[Player.instance.handCount];
        visualizationStartScale = animation.gameObject.transform.localScale;
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

        for (int iHand = 0; iHand < previousHandPos.Length; iHand++)
        {
            if (Vector3.Magnitude(Player.instance.hands[iHand].transform.position - previousHandPos[iHand]) * sensitivity > overallMaxTotalControllerMovement)
            {
                baseExercise.Reset(Vector3.zero);
                Reset();


                instructionSource.Stop();
                instructionSource.clip = tryAgainAudio;
                instructionSource.Play();

                return;
            }
        }


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

    public void Reset()
    {
        attempts = 0;
        animation.gameObject.transform.localScale = visualizationStartScale;
    }

    public void Play()
    {
        active = true;
    }

    public void Stop()
    {
        active = false;
    }
}
