using System;
using Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class FirstPersonController : MonoBehaviour
    {
        private enum PlayerStatus
        {
            Active, Locked
        }

        public static FirstPersonController Instance;

        [Header("Player Status")] 
        [SerializeField] private PlayerStatus playerStatus;
        
        [Header("Speed Fields")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;

        [Header("Move Interpolation")] 
        [SerializeField] private float moveInterpolationSpeed;
        
        [Header("Values")] 
        [SerializeField] private float currentSpeed;
        [SerializeField] private float lookInput;
        [SerializeField] private float lookOutput;
        [SerializeField] private Vector2 moveInput;
        [SerializeField] private Vector3 moveOutput;

        private CharacterController _characterController;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            currentSpeed = walkSpeed;

            InputController.Instance.OnRunPressedAction += StartRun;
            InputController.Instance.OnRunReleasedAction += EndRun;
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentGameStatus is GameManager.GameStatus.Paused or GameManager.GameStatus.GameOver) return;
            if (playerStatus != PlayerStatus.Active) return;
            
            // Gather Input
            lookInput = InputController.Instance.GetLookInput().x;
            moveInput = InputController.Instance.GetMoveInput();
            
            // Adjust Output
            lookOutput = lookInput * (GameManager.Instance.baseLookSensitivity * GameManager.Instance.lookMultiplier);

            Vector2 adjustedMoveInput = moveInput * currentSpeed;
            Vector3 moveGoal = (adjustedMoveInput.x * transform.right) + (adjustedMoveInput.y * transform.forward);

            moveOutput = Vector3.Lerp(moveOutput, moveGoal, moveInterpolationSpeed * Time.deltaTime);
            moveOutput.y = -1f * Time.deltaTime;
            
            // Rotate Player
            transform.Rotate(Vector3.up, lookOutput * Time.deltaTime);
            
            // Move Player
            _characterController.Move(moveOutput * Time.deltaTime);
        }

        private void StartRun(object sender, EventArgs eventArgs)
        {
            currentSpeed = runSpeed;
        }

        private void EndRun(object sender, EventArgs eventArgs)
        {
            currentSpeed = walkSpeed;
        }
    }
}
