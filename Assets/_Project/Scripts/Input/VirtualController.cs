using UnityEngine;

namespace ApeEscape.Input
{
    public class VirtualController : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader = default!;
        
        private float _whenJumpWasPressed;
        private const float TimeToWaitForInputRead = 0.2f;

        public Vector2 MoveDirection { get; private set; }
        public Vector2 CameraMoveDirection { get; private set; }
        public bool JumpPressed { get; private set; }
        public bool LeftAnalogPressed { get; private set; }
        public bool LeftAnalogHeld { get; private set; }
        public bool RightAnalogPressed { get; private set; }
        public bool RightAnalogHeld { get; private set; }

        // Reset flags
        public void ResetRightAnalogPressed() => RightAnalogPressed = false;
        public void ResetLeftAnalogPressed() => LeftAnalogPressed = false;
        public void ClearJumpPressCache() => JumpPressed = false;

        private void OnMove(Vector2 dir) => MoveDirection = dir;

        private void OnJumpPress()
        {
            JumpPressed = true;
            _whenJumpWasPressed = Time.time;
        }
        
        private void OnRightAnalogHeld(bool isHeld) => RightAnalogHeld = isHeld;

        private void OnRightAnalogClicked() => RightAnalogPressed = true;

        private void OnLeftAnalogHeld(bool isHeld) => LeftAnalogHeld = isHeld;

        private void OnLeftAnalogClicked() => LeftAnalogPressed = true;

        private void OnCameraMove(Vector2 dir) => CameraMoveDirection = dir;

        private void OnEnable()
        {
            inputReader.OnMoveEvent += OnMove;
            
            inputReader.OnCameraMoveEvent += OnCameraMove;
            inputReader.OnJumpEvent += OnJumpPress;
            
            inputReader.OnLeftAnalogClicked += OnLeftAnalogClicked;
            inputReader.OnLeftAnalogHeld += OnLeftAnalogHeld;
            
            inputReader.OnRightAnalogClicked += OnRightAnalogClicked;
            inputReader.OnRightAnalogHeld += OnRightAnalogHeld;
        }

        private void OnDisable()
        {
            inputReader.OnMoveEvent -= OnMove;
            
            inputReader.OnCameraMoveEvent -= OnCameraMove;
            inputReader.OnJumpEvent -= OnJumpPress;
            
            inputReader.OnLeftAnalogClicked -= OnLeftAnalogClicked;
            inputReader.OnLeftAnalogHeld -= OnLeftAnalogHeld;
            
            inputReader.OnRightAnalogClicked -= OnRightAnalogClicked;
            inputReader.OnRightAnalogHeld -= OnRightAnalogHeld;
        }

        private void Update()
        {
            if (Time.time > _whenJumpWasPressed + TimeToWaitForInputRead)
                JumpPressed = false;
        }
    }
}