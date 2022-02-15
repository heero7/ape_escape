using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ApeEscape.Input
{
    [CreateAssetMenu(fileName = "Input Reader", menuName = "Input/InputReader", order = 0)]
    public class InputReader : ScriptableObject, ApeEscapeControls.IGameplayActions
    {
        public event Action<Vector2> OnMoveEvent = delegate {  };
            
        public void OnMovement(InputAction.CallbackContext context) 
            => OnMoveEvent.Invoke(context.ReadValue<Vector2>());
    }
}