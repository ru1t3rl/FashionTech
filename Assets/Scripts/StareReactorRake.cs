using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * This is example script on how an object could subscribe to the staremanger's stare event and execute and stop a method.
 * */

[RequireComponent(typeof(Animation))]
public class StareReactorRake : MonoBehaviour
{
    // Start is called before the first frame update
    public float activationRange = 20;
    public float transitionBackSpeed = 0.5f;

    public bool hasRaked = false;

    private Animation rakeAnimation;

    private float distanceToPlayer;
    void Start()
    {
        rakeAnimation = GetComponent<Animation>();

        StareManager.current.OnStartStaring += StartEvent;
        StareManager.current.OnStopStaring += StopEvent;
    }

    private void StartEvent(Vector3 startPosition)
    {
        if (IsWithinRange(startPosition) && !hasRaked) { rakeAnimation.Play(); }
    }

    private void StopEvent(Vector3 endPosition)
    {
        hasRaked = true;
    }

    bool IsWithinRange(Vector3 playerPosition)
    {
        distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
        if (distanceToPlayer <= activationRange) { return true; }
        else { return false; }
    }

    // never forget to destroy your event listners to make sure you dont have memory leaks
    private void OnDestroy()
    {
        StareManager.current.OnStartStaring -= StartEvent;
        StareManager.current.OnStopStaring -= StopEvent;
    }
}
