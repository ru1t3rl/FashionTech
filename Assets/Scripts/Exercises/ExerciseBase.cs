using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRolijk.Excercises
{
    public class ExerciseBase : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] int repetitions = 5;
        [SerializeField] Instruction[] instructions;

        bool active = false;

        int currentInstruction = -1;
        int nextInstructionTime = 0;
        int repeated = 0;

        private void Start()
        {
            StareManager.current.OnStartStaring += Activate;
            StareManager.current.OnStopStaring += Deactivate;
        }

        private void Update()
        {
            if (active && currentInstruction < instructions.Length)
            {
                if (Time.time > nextInstructionTime)
                {
                    SelectNextInstruction();

                    if (active)
                    {
                        // Start the new instruction
                        instructions[currentInstruction].Play(audioSource);
                        Debug.Log($"<b>[Base Exercise]</b> Instruction:\n{instructions[currentInstruction].TextInstruction}");

                        // Set the time when the next instruction may start
                        nextInstructionTime = Mathf.RoundToInt(Time.time + instructions[currentInstruction].Duration);
                    }
                }
            }
        }

        void SelectNextInstruction()
        {
            currentInstruction++;

            if (currentInstruction < instructions.Length)
            {
                // Check if the next is also repeatable
                if (instructions[currentInstruction].Type != InstructionType.Start &&
                    instructions[currentInstruction].Type != InstructionType.Repeatable &&
                    instructions[currentInstruction - 1].Type == InstructionType.Repeatable)
                {
                    // Check if the max amount of repetitions has been reached
                    if (repeated < repetitions - 1)
                    {
                        // Find the first repeatable task
                        currentInstruction = 0;
                        for (; currentInstruction < instructions.Length; currentInstruction++)
                        {
                            if (instructions[currentInstruction].Type == InstructionType.Repeatable)
                            {
                                repeated++;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                active = false;
            }
        }

        void OnDestroy()
        {
            StareManager.current.OnStartStaring -= Activate;
            StareManager.current.OnStopStaring -= Deactivate;
        }

        #region Excersise State
        public void Activate(Vector3 position)
        {
            active = true;
        }

        public void Deactivate(Vector3 position)
        {
            active = false;
        }

        public bool IsActive => active;
        #endregion
    }

    [System.Serializable]
    public class Instruction
    {
        [SerializeField] string name;
        [SerializeField] string textInstruction;
        public string TextInstruction => textInstruction;

        [SerializeField] AudioClip audioInstruction;
        public AudioClip AudioInstruction => audioInstruction;

        [SerializeField] float duration;
        public float Duration => duration;

        [SerializeField] InstructionType instructionType = InstructionType.None;
        public InstructionType Type => instructionType;

        public UnityEvent onPlay;

        public void Play(AudioSource source)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }

            if (AudioInstruction != null)
            {
                source.clip = AudioInstruction;
                source.Play();
            }

            onPlay?.Invoke();
        }
    }

    public enum InstructionType
    {
        None,
        Start,
        Repeatable,
        Finish
    }
}
