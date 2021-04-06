using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StareManager : MonoBehaviour
{
    public float stareTimer = 1000.0f;
    public Vector3 stareBounds = new Vector3(10,4,4);

    private Vector3 currentRotation;
    private float minBounds;
    private float maxBounds;


    // Start is called before the first frame update
    void Start()
    {
        currentRotation = transform.eulerAngles;
        SetBounds();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (IsWithinBounds())
        {

        }*/
        maxBounds = Vector3.Angle(transform.forward, Vector3.forward);
        print(maxBounds);
    }

    void SetBounds()
    {
        minBounds = Vector3.Angle(transform.forward, Vector3.forward);
    }

    bool IsWithinBounds() {
        currentRotation = transform.eulerAngles;

        return true;
    }
}
