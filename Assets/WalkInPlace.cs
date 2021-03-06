using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bhaptics.Tact.Unity;


[RequireComponent(typeof(CharacterController))]
public class WalkInPlace : MonoBehaviour
{
    [SerializeField] AudioSource feedSource;

    public GameObject leftController;
    public GameObject rightController;
    public GameObject HMD;
    public VestHapticClip leftStep;
    public VestHapticClip rightStep;


    public float walkSpeed = 2.0f;
    public float detectionPrecision = 0.05f;
    public float minLeftUp = -0.02f;
    public float minRightUp = -0.02f;
    public float legUptime = 1.8f;

    public bool isLeftLegUp = false;
    public bool isLeftLegDown = false;
    public bool isRightLegUp = false;
    public bool isRightLegDown = false;
    private bool hasLeftPlayed = false;
    private bool hasRightPlayed = false;

    public Vector3 bodyDirection;
    public Vector3 baseLeftPosition;
    public Vector3 baseRightPosition;
    public Vector3 baseLeftOrientation;
    public Vector3 baseRightOrientation;

    private float leftLegUpTime = 0f;
    private float rightLegUpTime = 0f;
    public float headHeight;
    public float headSize = 0.23f;
    public GameObject debugMenu;


    private float currentWalkingSpeed = 0f;
    public float heightVariation = 0.005f;

    CharacterController controller;
    public MoveState state { get; private set; }

    private void Awake()
    {
        SaveDataSystem.instance.saveDataLoadedEvent.AddListener(LoadData);
        SaveDataSystem.instance.saveGameEvent.AddListener(SaveData);

        state = MoveState.Idle;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (baseLeftPosition == Vector3.zero)
        {
            Calibrate();
        }
        if (headHeight == 0)
        {
            SetHeadHeight();
        }
        if (leftLegUpTime > 0) { leftLegUpTime -= Time.deltaTime; }
        if (rightLegUpTime > 0) { rightLegUpTime -= Time.deltaTime; }
        OrientBody();
        if (IsControllerAtWaist() && IsWalking())
        {
            currentWalkingSpeed = Mathf.Clamp(currentWalkingSpeed += 0.1f, 0, walkSpeed);
            state = MoveState.Walking;
        }
        else
        {
            currentWalkingSpeed *= 0.9f;
            state = MoveState.Idle;
        }
        controller.SimpleMove(bodyDirection * currentWalkingSpeed);
        if (Input.GetKeyDown("space"))
        {
            ToggleDebug();
        }

        float speed = controller.velocity.sqrMagnitude;
        if ((speed > 0 || speed < 0) && !feedSource.isPlaying)
        {
            feedSource.Play();
        }
        else if (speed == 0 && feedSource.isPlaying)
        {
            feedSource.Stop();
        }
    }

    void ToggleDebug()
    {
        debugMenu.SetActive(!debugMenu.active);
    }
    // this method is called during a tracking event change. on the left and right hand.
    public void Calibrate()
    {
        if (leftController.transform.localEulerAngles == Vector3.zero && rightController.transform.localEulerAngles != Vector3.zero)
        {
            //leftController.transform.position = rightController.transform.position + Vector3.left;
            baseLeftPosition = baseRightPosition;

        }
        else if (rightController.transform.localEulerAngles == Vector3.zero && leftController.transform.localEulerAngles != Vector3.zero)
        {
            //rightController.transform.position = leftController.transform.position + Vector3.right;
            baseRightPosition = baseLeftPosition;
        }

        if (rightController.transform.localEulerAngles != Vector3.zero && leftController.transform.localEulerAngles != Vector3.zero)
        {
            print("-- calibrating controller positions --");
            baseLeftPosition = leftController.transform.localPosition;
            baseRightPosition = rightController.transform.localPosition;
            //baseLeftOrientation = leftController.transform.localEulerAngles;
            //baseRightOrientation = rightController.transform.localEulerAngles;
        }



    }

    void SetHeadHeight()
    {
        headHeight = HMD.transform.localPosition.y;
        headSize = headHeight / 7.5f;
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

    void CheckLeftLeg()
    {
        var angle = leftController.transform.localEulerAngles;
        var isAngleUp = angle.x > minLeftUp + baseLeftOrientation.x;
        var isAngleDown = IsWithinRange(angle.x, baseLeftOrientation.x, detectionPrecision);
        var heightVaries = rightController.transform.position.y < leftController.transform.position.y;


        if (isAngleUp && heightVaries) { 
            isLeftLegUp = true; 
            leftLegUpTime = legUptime;
            if (leftStep != null && leftStep.IsPlaying() == false && hasLeftPlayed == false)
            {
                leftStep.Play();
                hasLeftPlayed = true;
            }


        }
        else { isLeftLegUp = false; }

        if (isAngleDown) { isLeftLegDown = true; hasLeftPlayed = false;}
        else { isLeftLegDown = false; }

    }

    void CheckRightLeg()
    {
        var angle = rightController.transform.localEulerAngles;
        var isAngleUp = angle.x > minRightUp + baseRightOrientation.x;
        var isAngleDown = IsWithinRange(angle.x, baseRightOrientation.x, detectionPrecision);
        var heightVaries = rightController.transform.position.y > leftController.transform.position.y;

        if (isAngleUp && heightVaries) { 
            isRightLegUp = true; 
            rightLegUpTime = legUptime;
            if (rightStep != null && rightStep.IsPlaying() == false && hasRightPlayed == false)
            {
                rightStep.Play();
                hasRightPlayed = true;

            }
        }
        else { isRightLegUp = false; }

        if (isAngleDown) { isRightLegDown = true; hasRightPlayed = false; }
        else { isRightLegDown = false; }
    }

    bool IsWithinRange(float value, float check, float range)
    {
        if (value > check - range && value < check + range) { return true; }
        else { return false; }
    }


    bool IsControllerAtWaist()
    {
        var distanceLeft = HMD.transform.localPosition.y - leftController.transform.localPosition.y;
        var distanceRight = HMD.transform.localPosition.y - rightController.transform.localPosition.y;
        var distanceToHips = headSize * 2.5;
        var leftInPocket = distanceLeft > distanceToHips;
        if (leftInPocket && distanceRight > distanceToHips)
        {
            return true;
        }
        else
        {
            return false;

        }
    }

    public float getHipHeight()
    {
        return headSize * 4.5f;
    }

    bool IsWalking()
    {
        CheckLeftLeg();
        CheckRightLeg();


        if (baseLeftOrientation == Vector3.zero && baseRightOrientation == Vector3.zero)
        {
            return false;
        }

        if (leftLegUpTime > 0 && rightLegUpTime > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OrientBody()
    {

        float directionX = rightController.transform.position.x - leftController.transform.position.x;
        float directionZ = rightController.transform.position.z - leftController.transform.position.z;
        Vector2 normalA = new Vector2(-directionZ, directionX);
        bodyDirection = new Vector3(normalA.x, 0, normalA.y).normalized;
        //DrawLine(leftController.transform.position, rightController.transform.position, Color.red);
        //DrawLine(this.transform.position, this.transform.position + (bodyDirection * 20), Color.red);
    }

    public float getLeftLegUpTime()
    {
        return leftLegUpTime;
    }

    public float getRightLegUpTime()
    {
        return rightLegUpTime;
    }

    private void LoadData()
    {
        transform.position = SaveDataSystem.instance.loadedSaveData.playerPosition;
    }

    private void SaveData()
    {
        SaveDataSystem.instance.loadedSaveData.playerPosition = transform.position;
    }
}

public enum MoveState
{
    Idle,
    Walking
}