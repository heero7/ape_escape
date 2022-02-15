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

        public void ClearJumpPressCache() => JumpPressed = false;

        private void OnMove(Vector2 dir) => MoveDirection = dir;

        private void OnJumpPress()
        {
            JumpPressed = true;
            _whenJumpWasPressed = Time.time;
        }

        private void OnEnable()
        {
            inputReader.OnMoveEvent += OnMove;
            inputReader.OnCameraMoveEvent += OnCameraMove;
            inputReader.OnJumpEvent += OnJumpPress;
        }

        private void OnDisable()
        {
            inputReader.OnMoveEvent -= OnMove;
            inputReader.OnCameraMoveEvent -= OnCameraMove;
            inputReader.OnJumpEvent -= OnJumpPress;
        }

        private void OnCameraMove(Vector2 dir) => CameraMoveDirection = dir;

        private void Update()
        {
            if (Time.time > _whenJumpWasPressed + TimeToWaitForInputRead)
                JumpPressed = false;
        }
    }
}