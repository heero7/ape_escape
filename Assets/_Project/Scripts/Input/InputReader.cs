using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ApeEscape.Input
{
    [CreateAssetMenu(fileName = "Input Reader", menuName = "Input/InputReader", order = 0)]
    public class InputReader : ScriptableObject, ApeEscapeControls.IGameplayActions
    {
        public event Action<Vector2> MoveEvent = delegate { };
        public event Action<Vector2> CameraMoveEvent = delegate { };
        public event Action JumpEvent = delegate { };
        public event Action FreeLookEvent = delegate { };
        public event Action ResetCameraEvent = delegate { };
        public event Action LeftAnalogClicked = delegate { };
        public event Action<bool> LeftAnalogHeld = delegate { };
        public event Action RightAnalogClicked = delegate { };
        public event Action<bool> RightAnalogHeld = delegate { };

        private ApeEscapeControls _controls;

        public void OnMovement(InputAction.CallbackContext context)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnCamera(InputAction.CallbackContext context)
        {
            CameraMoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                JumpEvent.Invoke();
        }

        public void OnFreeLook(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                FreeLookEvent.Invoke();
        }

        public void OnResetCamera(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                ResetCameraEvent.Invoke();
        }

        public void OnLeftAnalogClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                LeftAnalogClicked.Invoke();
            if (context.phase == InputActionPhase.Performed)
                LeftAnalogHeld.Invoke(context.ReadValueAsButton());
            if (context.phase == InputActionPhase.Canceled)
                LeftAnalogHeld.Invoke(context.ReadValueAsButton());
        }

        public void OnRightAnalogClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
                RightAnalogClicked.Invoke();
            if (context.phase == InputActionPhase.Performed)
                RightAnalogHeld.Invoke(context.ReadValueAsButton());
            if (context.phase == InputActionPhase.Canceled)
                RightAnalogHeld.Invoke(context.ReadValueAsButton());
        }

        private void OnEnable()
        {
            if (_controls == null)
                _controls = new ApeEscapeControls();

            _controls.Gameplay.SetCallbacks(this);
            _controls.Enable();
        }

        private void OnDisable()
        {
            _controls?.Disable();
        }

        public void DisableMovement()
        {
            _controls.Gameplay.Movement.Disable();
        }

        public void EnableMovement()
        {
            _controls.Gameplay.Movement.Enable();
        }
    }
}