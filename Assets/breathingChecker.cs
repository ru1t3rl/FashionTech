using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breathingChecker : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
         gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BreathingStartComplete()
    {
        Debug.Log("hi there");
        gameManager.SetGameState(GameManager.gameState.inWorld);
    }

    public void BreathingExitComplete()
    {
        Debug.Log("bye there");

        gameManager.SetGameState(GameManager.gameState.exited);
    }

    public void ExitComplete()
    {
        Debug.Log("bye there");

        gameManager.SetGameState(GameManager.gameState.startup);
    }
}
