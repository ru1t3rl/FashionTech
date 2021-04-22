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

    private bool isLeftLegUp = false;
    private bool isLeftLegDown = false;
    private bool isRightLegUp = false;
    private bool isRightLegDown = false;
    private Vector3 bodyDirection;
    CharacterController controller;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        OrientBody();
        float curSpeed = walkSpeed * Input.GetAxis("Vertical");
        controller.SimpleMove(bodyDirection * curSpeed);
    }

    void getForwardDirection()
    {

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

    bool IsWalking()
    {
       // if () {
            return true;
        //} else {
            return false;
        //}
    }

    void OrientBody()
    {
        float directionX = rightController.transform.position.x - leftController.transform.position.x;
        float directionZ = rightController.transform.position.z - leftController.transform.position.z;
        Vector2 normalA = new Vector2(-directionZ, directionX);
        bodyDirection = new Vector3(normalA.x, 0, normalA.y).normalized;
        //DrawLine(leftController.transform.position, rightController.transform.position, Color.red);
        DrawLine(this.transform.position, this.transform.position + (bodyDirection * 20), Color.red);
    }
}
