using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bhaptics.Tact.Unity;


/***
 * This is example script on how an object could subscribe to the staremanger's stare event and execute and stop a method.
 * */
public class StareReactor_Massage : MonoBehaviour
{
    // Start is called before the first frame update
    public float activationRange = 20;
    public float transitionBackSpeed = 0.5f;
    private bool eventStarted = false;
    private bool eventStopped = false;

    private float transitionProgress = 0;
    private Vector3 baseScale;
    private float distanceToPlayer;
    GameObject player;
    private float distance = 100;
    private Animator animator;
    public HapticSource massageClip;
    void Start()
    {
        baseScale = transform.localScale;
        StareManager.current.OnStartStaring += StartEvent;
        StareManager.current.OnStopStaring += StopEvent;
        player = GameObject.Find("VRCamera");
        animator = GetComponent<Animator>();
        massageClip.GetComponent<HapticSource>();
    }

    private void StartEvent(Vector3 startPosition)
    {
        if (IsWithinRange(startPosition)) {
            if (massageClip.IsPlaying() == false)
            {
                Massage();
                Debug.Log("lets go massaging");
            }
            eventStarted = true; }
    }

    private void StopEvent(Vector3 endPosition)
    {

        eventStarted = false;
        eventStopped = true;
    }

    void Update()
    {
        if (IsWithinRange(player.transform.position))
        {
            animator.SetBool("withinRange", true);
        } else
        {
            animator.SetBool("withinRange", false);
        }
        
        if (eventStopped)
        {
            StopMassage();
            eventStopped = false;
        }
    }

    void Massage()
    {
        if (massageClip != null)
        {
            massageClip.PlayLoop();
        }
    }

    void StopMassage()
    {
        if (massageClip != null)
        {
            massageClip.Stop();
        }
    }


void TransformBack()
    {
        transitionProgress += Time.deltaTime * transitionBackSpeed;
        Vector3 newScale = Vector3.Lerp(transform.localScale, baseScale, transitionProgress);
        transform.localScale = newScale;
        if (transitionProgress > 1)
        {
            eventStopped = false;
            transitionProgress = 0;
        }
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
