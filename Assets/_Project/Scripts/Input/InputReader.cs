using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ApeEscape.Input
{
    [CreateAssetMenu(fileName = "Input Reader", menuName = "Input/InputReader", order = 0)]
    public class InputReader : ScriptableObject, ApeEscapeControls.IGameplayActions
    {
        public event Action<Vector2> OnMoveEvent = delegate { };
        public event Action<Vector2> OnCameraMoveEvent = delegate { };
        public event Action OnJumpEvent = delegate { };
        public event Action OnFreeLookEvent = delegate { };
        public event Action OnResetCameraEvent = delegate { };
        public event Action OnLeftAnalogClicked = delegate { };
        public event Action<bool> OnLeftAnalogHeld = delegate { };
        public event Action OnRightAnalogClicked = delegate { };
        public event Action<bool> OnRightAnalogHeld = delegate { };

        private ApeEscapeControls _controls;

        public void OnMovement(InputAction.CallbackContext context)
            => OnMoveEvent.Invoke(context.ReadValue<Vector2>());

        public void OnCamera(InputAction.CallbackContext context) 
            => OnCameraMoveEvent.Invoke(context.ReadValue<Vector2>());

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                OnJumpEvent.Invoke();
        }

        public void OnFreeLook(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                OnFreeLookEvent.Invoke();
        }

        public void OnResetCamera(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                OnResetCameraEvent.Invoke();
        }

        public void OnLeftAnalogClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                OnLeftAnalogClicked.Invoke();
            if (context.phase == InputActionPhase.Performed)
                OnLeftAnalogHeld.Invoke(context.ReadValueAsButton());
            if (context.phase == InputActionPhase.Canceled)
                OnLeftAnalogHeld.Invoke(context.ReadValueAsButton());
        }

        public void OnRightAnalogClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                OnRightAnalogClicked.Invoke();
            if (context.phase == InputActionPhase.Performed)
                OnRightAnalogHeld.Invoke(context.ReadValueAsButton());
            if (context.phase == InputActionPhase.Canceled)
                OnRightAnalogHeld.Invoke(context.ReadValueAsButton());
        }

        private void OnEnable()
        {
            if (_controls == null)
                _controls = new ApeEscapeControls();

            _controls.Gameplay.SetCallbacks(this);
            _controls.Enable();
        }

        private void OnDisable() => _controls?.Disable();

        public void DisableMovement() => _controls.Gameplay.Movement.Disable();

        public void EnableMovement() => _controls.Gameplay.Movement.Enable();
    }
}