#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

using UnityEngine;

namespace UnityTemplateProjects
{
    public class SimpleCameraController : MonoBehaviour
    {
        [SerializeField] UnityEngine.CharacterController controller;

        [Header("Movement")]
        [SerializeField] float acceleration;
        [SerializeField] float drag = 1;
        [SerializeField] float maxSpeed;
        [SerializeField] float maxYSpeed;
        Vector3 velocity = Vector3.zero;

        bool keyDown = false;
        Vector2 inputVec;

        [Header("Gravity")]
        [SerializeField] float jumpForce;
        [SerializeField] bool useGravity;
        [SerializeField] float gravity = 9.81f;
        [SerializeField] float distanceToFloor = 0.2f;
        bool onGround = false;
        private void Update()
        {
            velocity.x /= drag;
            velocity.z /= drag;

            inputVec.x = Input.GetAxis("Horizontal");
            inputVec.y = Input.GetAxis("Vertical");

            if (keyDown)
            {
                Debug.Log("Should be moving");
            }
            
            velocity.x += inputVec.x * acceleration * Time.deltaTime;
            velocity.z += inputVec.y * acceleration * Time.deltaTime;

            Truncate(ref velocity, maxSpeed, maxYSpeed);


            controller.Move(velocity);
        }

        /*
        public void OnMove(InputValue value)
        {
            keyDown = !keyDown;

            inputVec = value.Get<Vector2>();
        }
        */

        public void OnJump()
        {
            if (onGround)
                velocity.y += jumpForce * Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (!useGravity)
                return;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, distanceToFloor))
            {
                if (hit.collider.transform == null)
                {
                    if (velocity.y > 0)
                        velocity.y -= gravity * 0.5f * Time.fixedDeltaTime;
                    else
                        velocity.y -= gravity * Time.fixedDeltaTime;

                    onGround = false;
                }
                else
                {
                    velocity.y = 0;
                    onGround = true;
                }
            }
            else
            {
                if (velocity.y > 0)
                    velocity.y -= gravity * 0.5f * Time.fixedDeltaTime;
                else
                    velocity.y -= gravity * Time.fixedDeltaTime;

                onGround = false;
            }
        }

        void Truncate(ref Vector3 velocity, float maxSpeed, float maxYSpeed = 0)
        {
            if (new Vector2(velocity.x, velocity.z).magnitude > maxSpeed)
            {
                Vector2 vel = new Vector2(velocity.x, velocity.z).normalized * maxSpeed;
                velocity.x = vel.x;
                velocity.z = vel.y;
            }

            if (maxYSpeed != 0 && velocity.y > maxYSpeed || velocity.y < -maxYSpeed)
            {
                if (velocity.y > 0)
                    velocity.y = maxYSpeed * 2f;
                else
                    velocity.y = maxYSpeed;
            }
        }
    }

}