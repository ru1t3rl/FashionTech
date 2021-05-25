using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum gameState {startup, inWorld, inExersise, inExit, exited };
    public gameState currentGameState;
    public GameObject player;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject HMD;

    public float breathingStartHeight;

    public Animator realWorldAnimator;
    private GameObject realWorld;
    private WalkInPlace walkInPlace;
    private bool isTrigger; 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        HMD = GameObject.Find("VRCamera");
        leftHand = GameObject.Find("LeftHand");
        rightHand = GameObject.Find("RightHand");

        realWorld = GameObject.Find("Realworld menu");
        realWorldAnimator = realWorld.GetComponent<Animator>();
        walkInPlace = player.GetComponent<WalkInPlace>();
        SetGameState(gameState.startup);

    }

    public void SetGameState(gameState state)
    {
        currentGameState = state;
        isTrigger = false;
        switch (currentGameState)
        {
            case gameState.startup: 
                {
                    EnableWalking(false);
                    break;
                }
            case gameState.inWorld:
                {
                    EnableWalking(true);
                    break;
                }
            case gameState.inExersise:
                {
                    EnableWalking(false);
                    break;
                }
            case gameState.inExit:
                {
                    EnableWalking(true);
                    break;
                }
            case gameState.exited:
                {
                    EnableWalking(false);
                    break;
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGameState == gameState.startup)
        {
            if (DetectBreathingStart() && !isTrigger)
            {
                realWorldAnimator.SetTrigger("enterWorld");
                isTrigger = true;
            }
        }

        if(currentGameState== gameState.inExit )
        {
            if (DetectBreathingStart()&&!isTrigger){
                EnableWalking(false);
                isTrigger = true;

                realWorldAnimator.SetTrigger("leaveWorld");
            }
        }
    }

    // checks if the hands are above the head, then the player can start breathing;
    bool DetectBreathingStart()
    {
        bool leftIsUp = leftHand.transform.localPosition.y >= HMD.transform.localPosition.y;
        bool rightIsUp = rightHand.transform.localPosition.y >= HMD.transform.localPosition.y;

        if (leftIsUp && rightIsUp && HMD.transform.localPosition.y != 0)
        {
            return true;
        }
        return false;
    }

    void EnableWalking(bool enable)
    {
        walkInPlace.enabled = enable;
    }
}
