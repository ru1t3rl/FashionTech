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
    // make the variables in walkinplace public if you want to debug the values for now.
    void CheckData()
    {
        leftLegUp.enabled = walkinplace.isLeftLegUp;
        leftLegDown.enabled = walkinplace.isLeftLegDown;
        rightLegUp.enabled = walkinplace.isRightLegUp;
        rightLegDown.enabled = walkinplace.isRightLegDown;
        fps.text = "fps: " + (int)(1.0f / Time.smoothDeltaTime);
        leftLegY.text = "angleX: " + Mathf.RoundToInt(walkinplace.leftController.transform.localEulerAngles.x);
        rightLegY.text = "y: " + Mathf.RoundToInt(walkinplace.rightController.transform.localEulerAngles.x);
        leftLegMax.text = "max: " + (walkinplace.getLeftLegUpTime());
        leftLegMin.text = "min: " + (walkinplace.minLeftUp + walkinplace.baseLeftOrientation.x);
        rightLegMax.text = "max: " + (walkinplace.getRightLegUpTime());
        rightLegMin.text = "min: " + (walkinplace.minRightUp + walkinplace.baseRightOrientation.x);


    }

    // Update is called once per frame
    void Update()
    {
        CheckData();
    }
}
