using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class debugUIVisuals : MonoBehaviour
{
    public Image leftLegUp;
    public Image leftLegDown;
    public Image rightLegUp;
    public Image rightLegDown;
    public Text fps;
    public WalkInPlace walkinplace;

    // Start is called before the first frame update
    void Start()
    {
        CheckData();
    }

    void CheckData()
    {
        leftLegUp.enabled = walkinplace.isLeftLegUp;
        leftLegDown.enabled = walkinplace.isLeftLegDown;
        rightLegUp.enabled = walkinplace.isRightLegUp;
        rightLegDown.enabled = walkinplace.isRightLegUp;
        fps.text = "fps: " + (int)(1.0f / Time.smoothDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        CheckData();
    }
}
