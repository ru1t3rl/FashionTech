using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObject : MonoBehaviour
{
    public float frequency = 1.0f;
    public float speed = 1.0f;
    public float distance = 0.5f;


    private float _elapsedTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Cos(speed * Mathf.PI * frequency * _elapsedTime)*distance, transform.position.z);
    }
}
