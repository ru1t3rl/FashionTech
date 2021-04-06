using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * This is example script on how an object could subscribe to the staremanger's stare event and execute and stop a method.
 * */
public class StareReactor : MonoBehaviour
{
    // Start is called before the first frame update
    public float activationRange = 20;
    public float transitionBackSpeed = 0.5f;
    private bool eventStarted = false;
    private bool eventStopped = false;

    private float transitionProgress = 0;
    private Vector3 baseScale;
    private float distanceToPlayer;
    void Start()
    {
        baseScale = transform.localScale;
        StareManager.current.OnStartStaring += StartEvent;
        StareManager.current.OnStopStaring += StopEvent;
    }

    private void StartEvent(Vector3 startPosition) {
        if (IsWithinRange(startPosition)) { eventStarted = true; }
    }

    private void StopEvent(Vector3 endPosition)
    {

        eventStarted = false;
        eventStopped = true;
    }

    void Update()
    {
        if (eventStarted)
        {
            ScaleUpAndDown();
        }
        if (eventStopped)
        {
            TransformBack();
        }
    }

    void ScaleUpAndDown()
    {
        Vector3 newScale = new Vector3(1, 2 * Mathf.Sin(Time.time * 0.5f), 1);
        transform.localScale = newScale;
    }

    void TransformBack()
    {
        transitionProgress += Time.deltaTime * transitionBackSpeed;
        Vector3 newScale = Vector3.Lerp(transform.localScale, baseScale, transitionProgress);
        transform.localScale = newScale;
        if(transitionProgress > 1)
        {
            eventStopped = false;
            transitionProgress = 0;
        }
    }

    bool IsWithinRange(Vector3 playerPosition)
    {
        distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
        if (distanceToPlayer <= activationRange) {return true;}
        else {return false;}
    }

    // never forget to destroy your event listners to make sure you dont have memory leaks
    private void OnDestroy()
    {
        StareManager.current.OnStartStaring -= StartEvent;
        StareManager.current.OnStopStaring -= StopEvent;
    }
}
