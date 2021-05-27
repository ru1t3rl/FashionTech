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
    public Text rightControllerY;
    public Text leftControllerY;
    public Text headY;
    public Text hipsY;
    public Image hips;






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
        rightControllerY.text = "Ry: " + walkinplace.rightController.transform.localPosition.y; 
        leftControllerY.text = "Ly: " +walkinplace.leftController.transform.localPosition.y; ;
        headY.text = "height: " + walkinplace.headHeight;
        hipsY.text = "hips: " + walkinplace.getHipHeight();
        rightControllerY.transform.localPosition = new Vector3(rightControllerY.transform.localPosition.x,walkinplace.rightController.transform.localPosition.y*100 - 100,0);
        leftControllerY.transform.localPosition = new Vector3(leftControllerY.transform.localPosition.x, walkinplace.leftController.transform.localPosition.y * 100 - 100, 0);


    }

    // Update is called once per frame
    void Update()
    {
        CheckData();
    }
}
