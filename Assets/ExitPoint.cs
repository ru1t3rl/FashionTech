using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    public Animator realWorldAnimator;
    private GameObject realWorld;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        realWorld = GameObject.Find("Realworld menu");
        realWorldAnimator = realWorld.GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "HeadCollider")
        {
            realWorldAnimator.SetTrigger("openExit");
            gameManager.SetGameState(GameManager.gameState.inExit);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "HeadCollider") { 

            realWorldAnimator.SetTrigger("closeExit");
            gameManager.SetGameState(GameManager.gameState.inWorld);

        }
    }
}
