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
    public Text leftLegY;
    public Text leftLegMax;
    public Text leftLegMin;
    public Text rightLegY;
    public Text rightLegMax;
    public Text rightLegMin;


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
        rightLegDown.enabled = walkinplace.isRightLegDown;
        fps.text = "fps: " + (int)(1.0f / Time.smoothDeltaTime);
        leftLegY.text = "y: " + walkinplace.leftController.transform.localPosition.y;
        rightLegY.text = "y: " + walkinplace.rightController.transform.localPosition.y;
        leftLegMax.text = "max: " + (walkinplace.leftLegUpTime);
        leftLegMin.text = "min: " + (walkinplace.minLeftUp + walkinplace.baseLeftPosition.y);
        rightLegMax.text = "max: " + (walkinplace.rightLegUpTime);
        rightLegMin.text = "min: " + (walkinplace.minRightUp + walkinplace.baseRightPosition.y);


    }

    // Update is called once per frame
    void Update()
    {
        CheckData();
    }
}
