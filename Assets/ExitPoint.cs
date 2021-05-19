using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    public Animator realWorldAnimator;
    private GameObject realWorld;

    // Start is called before the first frame update
    void Start()
    {
        realWorld = GameObject.Find("Realworld menu");
        realWorldAnimator = realWorld.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        realWorldAnimator.SetTrigger("openExit");
    }

    void OnTriggerExit(Collider other)
    {
        realWorldAnimator.SetTrigger("closeExit");
    }
}
