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

        private ApeEscapeControls _controls;

        public void OnMovement(InputAction.CallbackContext context)
            => OnMoveEvent.Invoke(context.ReadValue<Vector2>());

        public void OnCamera(InputAction.CallbackContext context) 
            => OnCameraMoveEvent.Invoke(context.ReadValue<Vector2>());

        public void OnJump(InputAction.CallbackContext context)
        {
            Debug.Log($"Jump Action Trigger status: {context.action.triggered}");
            if (context.phase == InputActionPhase.Performed)
                OnJumpEvent.Invoke();
        }

        public void OnFreeLook(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                OnFreeLookEvent.Invoke();
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