using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveOptions : MonoBehaviour
{
    public bool play = true;

    [Header("Sine Wave")]
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float frequentie = 0.1f;
    [SerializeField] private Vector3 center = Vector3.zero;
    private float angle = 0.0f;

    [Header("Scale")]
    public bool fixedXScale = false;
    public bool fixedYScale = false;
    public bool fixedZScale = false;

    [Header("Position")]
    public bool fixedXPosition = true;
    public bool fixedYPosition = false;
    public bool fixedZPosition = true;

    private Vector3 localScale = Vector3.zero,
                    localPosition = Vector3.zero,
                    startPosition;


    private void Start()
    {
        startPosition = transform.localPosition;
        localPosition = transform.localPosition;

        localScale = transform.localScale;
    }

    void Update()
    {
        if (!play)
        {
            return;
        }

        angle += frequentie;

        #region Scale
        if (!fixedXScale)
        {
            localScale.x = (amplitude * Mathf.Sin(angle) + center.x);
        }
        if (!fixedYScale)
        {
            localScale.y = (amplitude * Mathf.Sin(angle) + center.y);
        }
        if (!fixedZScale)
        {
            localScale.z = (amplitude * Mathf.Sin(angle) + center.z);
        }

        if (!fixedXScale || !fixedYScale || !fixedZScale)
        {
            transform.localScale = localScale;
        }
        #endregion

        #region Position
        if (!fixedXPosition)
        {
            localPosition.x = (amplitude * Mathf.Sin(angle) + center.x + startPosition.x);
        }
        if (!fixedYPosition)
        {
            localPosition.y = (amplitude * Mathf.Sin(angle) + center.y + startPosition.y);
        }
        if (!fixedZPosition)
        {
            localPosition.z = (amplitude * Mathf.Sin(angle) + center.z + startPosition.z);
        }

        if (!fixedXPosition || !fixedYPosition || !fixedZPosition)
        {
            transform.localPosition = localPosition;
        }
        #endregion
    }

    public void Play()
    {
        play = true;
    }

    public void Pause()
    {
        play = false;
    }
}
