using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;



public class temperatureChanger : MonoBehaviour
{
    public float maxRadius = 2;
    public float minRadius = 2;
    public float temperature = 0;
    public float maxTemperature = 100;
    public float minTemperature = -100;

    private Volume temperatureManager;
    private WhiteBalance _whiteBalance;
    private GameObject head;
    private GameObject leftHand;
    private GameObject rightHand;

    // Start is called before the first frame update
    void Awake()
    {
        temperatureManager = GameObject.Find("TemperatureManager").GetComponent<Volume>();
        temperatureManager.profile.TryGet(out _whiteBalance);
        print(_whiteBalance);
        SetTemperature(temperature);
        head = GameObject.Find("VRCamera");
        leftHand = GameObject.Find("LeftHand");
        rightHand = GameObject.Find("RightHand");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (float)System.Math.Round(CheckNearestDistance(),2);
        if (distance < maxRadius)
        {
            SetTemperature(Remap(distance, minRadius, maxRadius, minTemperature, maxTemperature));
        }
        
    }

    public void SetTemperature(float value)
    {
        temperature = value;
        _whiteBalance.temperature.value = temperature;
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
