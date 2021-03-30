using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(CapsuleCollider))]
public class JoystickMovement2 : MonoBehaviour
{
    public SteamVR_Action_Vector2 touchpadInput;
    public Transform cameraTransform;
    private CapsuleCollider collider;

    private void Start()
    {
        collider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        Vector3 movementDir = Player.instance.hmdTransform.TransformDirection(new Vector3(touchpadInput.axis.x, 0, touchpadInput.axis.y));
        transform.position += (Vector3.ProjectOnPlane(movementDir * Time.fixedDeltaTime, Vector3.up));

        float distanceFromFloor = Vector3.Dot(cameraTransform.localPosition, Vector3.up);
        collider.height = Mathf.Max(collider.radius, distanceFromFloor);

        collider.center = cameraTransform.localPosition - 0.5f * distanceFromFloor * Vector3.up;
    }
}
