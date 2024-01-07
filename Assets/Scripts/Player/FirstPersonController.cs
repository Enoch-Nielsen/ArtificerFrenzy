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
        [SerializeField] private float gravity;
        
        [Header("Values")] 
        [SerializeField] private float currentSpeed;
        [SerializeField] private float lookInput;
        [SerializeField] private float lookOutput;
        [SerializeField] private Vector2 moveInput;
        [SerializeField] private Vector3 moveOutput;

        private CharacterController _characterController;

        private void Start()
        {
            Instance = this;
            
            _characterController = GetComponent<CharacterController>();
            currentSpeed = walkSpeed;

            InputController.Instance.OnRunPressedAction += StartRun;
            InputController.Instance.OnRunReleasedAction += EndRun;
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentGameStatus is GameManager.GameStatus.Paused or GameManager.GameStatus.GameOver) return;
            if (playerStatus != PlayerStatus.Active) return;
            
            RotatePlayer();
            MovePlayer();
        }

        /// <summary>
        /// Gathers and Applies player input to rotate the player.
        /// </summary>
        private void RotatePlayer()
        {
            // Gather Input
            lookInput = InputController.Instance.GetLookInput().x;
            
            // Adjust Output
            lookOutput = lookInput * (GameManager.Instance.baseLookSensitivity * GameManager.Instance.lookMultiplier);
            
            // Rotate Player
            transform.Rotate(Vector3.up, lookOutput * Time.deltaTime);
        }

        /// <summary>
        /// Gathers and applies player input to move the player.
        /// </summary>
        private void MovePlayer()
        {
            // Gather Input
            moveInput = InputController.Instance.GetMoveInput();
            
            // Adjust Output
            Vector2 adjustedMoveInput = moveInput * currentSpeed;
            Vector3 moveGoal = (adjustedMoveInput.x * transform.right) + (adjustedMoveInput.y * transform.forward);

            moveOutput = Vector3.Lerp(moveOutput, moveGoal, moveInterpolationSpeed * Time.deltaTime);
            moveOutput.y = gravity;
            
            // Move Player
            _characterController.Move(moveOutput * Time.deltaTime);
        }

        /// <summary>
        /// Starts the running state on the player.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void StartRun(object sender, EventArgs eventArgs)
        {
            currentSpeed = runSpeed;
        }

        /// <summary>
        /// Ends the running state on the player.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void EndRun(object sender, EventArgs eventArgs)
        {
            currentSpeed = walkSpeed;
        }
    }
}
