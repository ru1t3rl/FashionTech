using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheoPoseController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    public string currentAnimation; 
    
    void Start()
    {
        animator = this.GetComponent<Animator>();
        animator.SetTrigger(currentAnimation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
