using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StareManager : MonoBehaviour
{
    public static StareManager current;
    public float timeTillEvent = 10f;
    public float stareBounds = 10f;

    private float stareTimer;
    private float currentAngle;
    private float minBounds;
    private float maxBounds;
    private bool isStaring = false;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        currentAngle = Vector3.Angle(transform.forward, Vector3.forward);
        SetBounds();
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.IsWalking || 
           GameManager.instance.currentGameState == GameManager.gameState.inExit || 
           GameManager.instance.currentGameState == GameManager.gameState.startup ||
           GameManager.instance.currentGameState == GameManager.gameState.exited)
        {            
            return;
        }

        if (IsWithinBounds())
        {
            CountDown(Time.deltaTime);
        } else
        {
            SetBounds();
            ResetTimer();
            StopStaring();
        }
    }

    public event Action<Vector3> OnStartStaring;
    public void StartStaring()
    {
        if(!isStaring)
        {
            isStaring = true;
            OnStartStaring?.Invoke(transform.position);
        }
    }

    public event Action<Vector3> OnStopStaring;
    public void StopStaring()
    {
        if (isStaring)
        {
            isStaring = false;
            OnStopStaring?.Invoke(transform.position);
        }
    }

    void ResetTimer()
    {
        stareTimer = timeTillEvent;
    }

    void CountDown(float time)
    {
        stareTimer -= time;
        if (stareTimer<= 0  && !isStaring) {
            StartStaring();
        }
    }

    void SetBounds()
    {
        minBounds = currentAngle - stareBounds;
        maxBounds = currentAngle + stareBounds;
    }

    private bool IsWithinBounds()
    {
        currentAngle = Vector3.Angle(transform.forward, Vector3.forward);
        if(currentAngle >= minBounds && currentAngle <= maxBounds)
        {
            return true;

        } else
        {
            return false;
        }

    }
}
