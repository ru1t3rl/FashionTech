using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.PostProcessing;


public class temperatureChanger : MonoBehaviour
{
    public float maxRadius = 2;
    public float minRadius = 2;
    public float temperature = 0;
    public float maxTemperature = 100;
    public float minTemperature = -100;

    private Volume temperatureManager;
    private GameObject head;
    private GameObject leftHand;
    private GameObject rightHand;

    // Start is called before the first frame update
    void Awake()
    {
        temperatureManager = GameObject.Find("TemperatureManager").GetComponent<Volume>();
        head = GameObject.Find("A");
        leftHand = GameObject.Find("B");
        rightHand = GameObject.Find("C");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (float)System.Math.Round(CheckNearestDistance(),2);
        print(Remap(distance,0.1f,1f,-100f,100f)); //5
        if (distance < maxRadius)
        {
            temperature =  Remap(distance, minRadius, maxRadius, minTemperature, maxTemperature);
        } else
        {
            temperature = maxTemperature;
        }
    }

    public void SetTemperature(float value)
    {
        temperatureManager.profile.
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    private float CheckNearestDistance()
    {
        float headDistance = Vector3.Distance(transform.position, head.transform.position);
        float leftDistance = Vector3.Distance(transform.position, leftHand.transform.position);
        float rightDistance = Vector3.Distance(transform.position, rightHand.transform.position);
        float[] distances = new[] { headDistance, leftDistance, rightDistance };
        float value = float.PositiveInfinity;


        for (int i = 0; i < distances.Length; i++)
        {
            if (distances[i] < value)
            {
                value = distances[i];
            }
        }


        return value;
    }
}
