using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputController : MonoBehaviour
    {
        public static InputController Instance;
        private InputActions _inputActions;

        public event EventHandler OnInteractPressedAction;
        public event EventHandler OnTooltipPressedAction;
        public event EventHandler OnThrowPressedAction;
        public event EventHandler OnMenuPressedAction;
        public event EventHandler OnRunPressedAction;
        public event EventHandler OnRunReleasedAction;
        public event EventHandler OnDropPressedAction;
        public event EventHandler OnSelectPressedAction;
    
        private void Awake()
        {
            Instance = this;
            
            // Initialize input actions.
            _inputActions = new InputActions();
            _inputActions.Enable();

            // Initialize events.
            _inputActions.Primary.Interact.performed += OnInteractPerformed;
            _inputActions.Primary.Tooltip.performed += OnTooltipPerformed;
            _inputActions.Primary.Menu.performed += OnMenuPerformed;
            _inputActions.Primary.Throw.performed += OnThrowPerformed;
            _inputActions.Primary.Drop.performed += OnDropPerformed;
            _inputActions.Primary.Select.performed += OnSelectPerformed;
            
            _inputActions.Primary.Run.performed += OnRunPerformed;
            _inputActions.Primary.Run.canceled += OnRunReleased;
        }

        /**
         * Called when the player interacts with an object.
         */
        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            OnInteractPressedAction?.Invoke(this, EventArgs.Empty);        
        }
        
        
        /**
         * Called when the player attempts to press for tooltip.
         */
        private void OnTooltipPerformed(InputAction.CallbackContext context)
        {
            OnTooltipPressedAction?.Invoke(this, EventArgs.Empty);
        }

        /**
         * Called when the player attempts to throw a held object.
         */
        private void OnThrowPerformed(InputAction.CallbackContext context)
        {
            OnThrowPressedAction?.Invoke(this, EventArgs.Empty);
        }

        /**
         * Called when the player attempts to open the menu.
         */
        private void OnMenuPerformed(InputAction.CallbackContext context)
        {
            OnMenuPressedAction?.Invoke(this, EventArgs.Empty);
        }

        /**
         * Called when the player attempts to run.
         */
        private void OnRunPerformed(InputAction.CallbackContext context)
        {
            OnRunPressedAction?.Invoke(this, EventArgs.Empty);
        }

        /**
         * Called when the player attempts to stop running.
         */
        private void OnRunReleased(InputAction.CallbackContext context)
        {
            OnRunReleasedAction?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the player pressed the drop button.
        /// </summary>
        /// <param name="context"></param>
        private void OnDropPerformed(InputAction.CallbackContext context)
        {
            OnDropPressedAction?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the player attempts to select.
        /// </summary>
        /// <param name="context"></param>
        private void OnSelectPerformed(InputAction.CallbackContext context)
        {
            OnSelectPressedAction?.Invoke(this, EventArgs.Empty);
        }

        /**
         * Returns the players current move input.
         */
        public Vector2 GetMoveInput()
        {
            return _inputActions.Primary.Walk.ReadValue<Vector2>().normalized;
        }

        /**
         * Returns the players current look input.
         */
        public Vector2 GetLookInput()
        {
            return _inputActions.Primary.Look.ReadValue<Vector2>();
        }
    }
}
