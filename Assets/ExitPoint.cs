using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    public Animator realWorldAnimator;
    private GameObject realWorld;
    private GameManager gameManager;
    public Material exitMaterial;
    public float maxDistance = 5f;
    private GameObject player;
    private float distance;
    public float minHeight = 1f;
    public float maxHeight = 20f;


    // Start is called before the first frame update
    void Start()
    {
        realWorld = GameObject.Find("Realworld menu");
        realWorldAnimator = realWorld.GetComponent<Animator>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        player = GameObject.Find("VRCamera");

    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if(distance < maxDistance)
        {
            float lerpedDistance = Mathf.Lerp(minHeight, maxHeight, distance / maxDistance);
            var poolHeight = Mathf.Clamp(lerpedDistance, minHeight, maxHeight);
            //exitMaterial.SetFloat("height", poolHeight);
        }
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
