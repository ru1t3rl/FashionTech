using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class WalkInPlace : MonoBehaviour
{
    public GameObject leftController;
    public GameObject rightController;
    public float walkSpeed = 3.0f;
    public float rotateSpeed = 3.0f;
    public float detectionPrecision = 1.1f;
    public float maxLeftUp = 0.01f;
    public float minLeftUp = -0.5f;
    public float maxRightUp = 0.01f;
    public float minRightUp = -0.5f;


    public bool isLeftLegUp = false;
    public bool isLeftLegDown = false;
    public bool isRightLegUp = false;
    public bool isRightLegDown = false;
    private Vector3 bodyDirection;
    private Vector3 baseLeftPosition;
    private Vector3 baserightPosition;
    private Vector3 baseLeftOrientation;
    private Vector3 baseRightOrientation;


    CharacterController controller;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        print("----");

    }

    // Update is called once per frame
    void Update()
    {
        OrientBody();
        if (IsWalking())
        {
            //float curSpeed = walkSpeed * Input.GetAxis("Vertical");
           float curSpeed = walkSpeed; //TODO should ease in the walk speed to a max walking speed;
            controller.SimpleMove(bodyDirection * curSpeed);

        }
    }

    // this method is called during a tracking event change. on the left and right hand.
    public void Calibrate()
    {
        print("----");
        print("CALIBRATING");
   

        if(leftController.transform.localEulerAngles == Vector3.zero && rightController.transform.localEulerAngles != Vector3.zero)
        {
            leftController.transform.position = rightController.transform.position + Vector3.left;

        } else if (rightController.transform.localEulerAngles == Vector3.zero && leftController.transform.localEulerAngles != Vector3.zero)
        {
            rightController.transform.position = leftController.transform.position + Vector3.right;
        }

        if (rightController.transform.localEulerAngles != Vector3.zero && leftController.transform.localEulerAngles != Vector3.zero)
        {
            baseLeftPosition = leftController.transform.position;
            baserightPosition = rightController.transform.position;
            baseLeftOrientation = leftController.transform.localEulerAngles;
            baseRightOrientation = rightController.transform.localEulerAngles;
        }

        print(baseLeftPosition);
        print(baserightPosition);
        print(baseLeftOrientation);
        print(baseRightOrientation);
        print("----");

    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }


    void MeasureLeftLeg()
    {
        
        float heightDifference =  - baseLeftPosition.y;
        if (heightDifference < minLeftUp)
        {
            minLeftUp = heightDifference;
        }
        if (heightDifference > maxLeftUp)
        {
            maxLeftUp = heightDifference;
        };
        print(minLeftUp + "<-" + baseLeftPosition.y + "->"+ maxLeftUp);
    }

    void CheckLeftLeg()
    {
        if (IsWithinRange(leftController.transform.position.y, maxLeftUp, detectionPrecision)) { isLeftLegUp = true; }
        else { isLeftLegUp = false; }

        if (IsWithinRange(leftController.transform.position.y, minLeftUp, detectionPrecision)) { isLeftLegDown = true; }
        else { isLeftLegDown = false; }

    }

    void CheckRightLeg()
    { 
        if (IsWithinRange(rightController.transform.position.y, maxRightUp, detectionPrecision)) { isRightLegUp = true; }
        else { isLeftLegUp = false;}

        if (IsWithinRange(rightController.transform.position.y, minRightUp, detectionPrecision)) { isRightLegDown = true;}
        else { isLeftLegDown = false;}
    }

    bool IsWithinRange(float value, float check, float range)
    {
        print((check - range) + "<-" + value + "->" + (check + range));

        if (value > check - range && value < check + range) {return true;}
        else { return false; }
    }


    bool IsWalking()
    {
        CheckLeftLeg();
        CheckRightLeg();

        if (baseLeftOrientation == Vector3.zero && baseRightOrientation == Vector3.zero)
        {
            return false;
        }

        if (isLeftLegUp && isRightLegDown || isRightLegUp && isLeftLegDown) {
            return true;
        } else {
            return false;
        }
    }

    void OrientBody()
    {
        float directionX = rightController.transform.position.x - leftController.transform.position.x;
        float directionZ = rightController.transform.position.z - leftController.transform.position.z;
        Vector2 normalA = new Vector2(-directionZ, directionX);
        bodyDirection = new Vector3(normalA.x, 0, normalA.y).normalized;
        DrawLine(leftController.transform.position, rightController.transform.position, Color.red);
        DrawLine(this.transform.position, this.transform.position + (bodyDirection * 20), Color.red);
    }
}
