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
        [SerializeField] UnityEvent OnReset;

        bool active = false;
        bool delayedStart = false;
        public bool DelayedStart { get => delayedStart; set => delayedStart = value; }

        int currentInstruction = -1;
        int nextInstructionTime = 0;
        int repeated = 0;

        private void Start()
        {
            StareManager.current.OnStartStaring += Activate;
            StareManager.current.OnStopStaring += Reset;
        }

        private void Update()
        {

            if (active && currentInstruction < instructions.Length)
            {
                if (Time.time > nextInstructionTime && !delayedStart)
                {
                    SelectNextInstruction();
                    
                    if (active)
                    {
                        Play();
                    }
                }
                else if (delayedStart)
                {
                    Play();
                }
            }
        }

        private void Play()
        {
            // Start the new instruction
            instructions[currentInstruction].Play(audioSource, this);

            if (!delayedStart)
            {
                // Set the time when the next instruction may start
                nextInstructionTime = Mathf.RoundToInt(Time.time + instructions[currentInstruction].Duration);
            }
        }

        void SelectNextInstruction()
        {
            currentInstruction += 1;

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
                gameObject.SetActive(false);
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

        public void Reset(Vector3 position)
        {
            Deactivate(transform.position);

            currentInstruction = 0;
            repeated = 0;

            OnReset?.Invoke();
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

        public UnityEvent<float> onPlay;

        public void Play(AudioSource source, ExerciseBase parent = null)
        {

            if (source.isPlaying)
            {
                if (parent != null)
                {
                    parent.DelayedStart = true;
                }
                return;
            }

            if (AudioInstruction != null)
            {
                source.clip = AudioInstruction;
                source.Play();
            }

            onPlay?.Invoke(duration);

            if (parent != null)
            {
                parent.DelayedStart = false;
            }
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
