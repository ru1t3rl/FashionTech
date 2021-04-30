using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    public Animator realWorldAnimator;

    // Start is called before the first frame update
    void Start()
    {
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
