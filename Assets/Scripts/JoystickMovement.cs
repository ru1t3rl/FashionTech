using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace VRolijk.Movement
{
    [RequireComponent(typeof(CharacterController)), RequireComponent(typeof(AudioSource))]
    public class JoystickMovement : MonoBehaviour
    {
        [SerializeField] SteamVR_Action_Vector2 touchpadInput;
        [SerializeField] Transform bottomOfPlayerBody;

        [Header("Physics")]
        [SerializeField] float horsePower = 5f;
        [SerializeField] float maxSpeed = 2f;
        [SerializeField] float mass = 60f;
        [SerializeField] float maxYSpeed = 5f;
        [SerializeField] bool useGravity = true;
        [SerializeField] float gravity = 9.81f;
        [SerializeField] float drag = 2f;
        [SerializeField] float maxRayLength = .2f;

        Vector3 velocity = Vector3.zero;

        CharacterController cController;
        AudioSource walkAudioSource;

        private void Awake()
        {
            cController = GetComponent<CharacterController>();
            walkAudioSource = GetComponent<AudioSource>();

            SaveDataSystem.instance.saveGameEvent.AddListener(SaveData);
            SaveDataSystem.instance.saveDataLoadedEvent.AddListener(LoadData);
        }

        void FixedUpdate()
        {
            // Calculate the acceleration based on the direction the player is looking in and the joystick axes
            Vector3 vel = Player.instance.hmdTransform.forward * touchpadInput.axis.y * horsePower +
                          Player.instance.hmdTransform.right * touchpadInput.axis.x * horsePower;
            velocity.x += vel.x;
            velocity.z += vel.z;

            if (useGravity)
            {
                if (Physics.Raycast(bottomOfPlayerBody.position, Vector3.down, maxRayLength))
                {

                }
                else
                {
                    velocity.y += gravity / mass;

                    if (velocity.y > maxYSpeed)
                    {
                        velocity.y = maxYSpeed;
                    }
                }
            }

            // Update the x & z velocity
            velocity /= drag;
            Truncate(ref velocity, maxSpeed, false);

            cController.Move(Time.fixedDeltaTime * velocity);

            
            if (velocity.magnitude < .1f)
            {
                if (walkAudioSource.isPlaying)
                {
                    walkAudioSource.Stop();
                }
            }
            else
            {
                if (!walkAudioSource.isPlaying)
                {
                    walkAudioSource.pitch = Random.Range(1f, 1.05f);
                    walkAudioSource.Play();                    
                }
            }


            // Set the colliders height to Player camera's height
            cController.height = Player.instance.eyeHeight;
            cController.center = Camera.main.transform.localPosition - 0.5f * Player.instance.eyeHeight * Vector3.up;
        }


        /// <summary>
        /// Compare the length of a vector with a max value
        /// </summary>
        /// <param name="vector">Vector to check</param>
        /// <param name="maxValue">Max Length of the vector</param>
        /// <param name="includeY">Include the Y axis in the calculation</param>
        void Truncate(ref Vector3 vector, float maxValue, bool includeY = false)
        {
            if (includeY)
            {
                if (vector.sqrMagnitude > maxValue * maxValue)
                {
                    vector = vector.normalized * maxValue;
                }
            }
            else
            {
                Vector3 vec = vector;
                vec.y = 0;

                if (vec.sqrMagnitude > maxValue * maxValue)
                {
                    vec = vec.normalized * maxValue;
                    vec.y = vector.y;
                    vector = vec;
                }
            }
        }

        private void SaveData()
        {
            SaveDataSystem.instance.loadedSaveData.playerPosition = Player.instance.transform.position;
        }

        private void LoadData()
        {
            Player.instance.transform.position = SaveDataSystem.instance.loadedSaveData.playerPosition;
        }
    }
}
