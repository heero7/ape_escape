using ApeEscape.Input;
using ApeEscape.Stats;
using UnityEngine;

namespace ApeEscape.FSM.Player
{
    public class GroundLocomotion : PlayerMovement
    {
        private MoveStats _moveStats;
        
        private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

        public GroundLocomotion(CharacterController characterController, VirtualController virtualController,
            Animator animator, Transform thirdPersonCamera, MoveStats moveStats) : 
            base(characterController, virtualController, animator, thirdPersonCamera)
        {
            _moveStats = moveStats;
        }

        public override void OnEnter()
        {
            Animator.SetFloat(MoveSpeed, 0f);
        }

        public override void OnUpdate()
        {
            ApplyGravity();
            
            var horizontalInput = VirtualController.MoveDirection.x;
            var verticalInput = VirtualController.MoveDirection.y; 
            
            HandleHorizontalPlaneMovement(horizontalInput, verticalInput);
            CalculateCameraAngleChange(horizontalInput, verticalInput);
        }

        public override void OnExit()
        {
            Animator.SetFloat(MoveSpeed, 0f);
        }
        
        private void ApplyGravity()
        {
            if (ApeEscape.Player.IsGrounded) return;
            
            Debug.Log("This object hasn't touched the ground yet, attempting to push it down.");
            CharacterController.Move(Vector3.down * Time.deltaTime);
        }

        private void HandleHorizontalPlaneMovement(float horizontalInput, float verticalInput)
        {
            var move = new Vector3(horizontalInput, 0, verticalInput);
            move = ThirdPersonCamera.forward * move.z + ThirdPersonCamera.right * move.x;
            move.y = 0;

            var moveInputMagnitude = VirtualController.MoveDirection.magnitude;

            var moveSpeed = 1f;
            
            Animator.SetFloat(MoveSpeed, moveInputMagnitude);

            // Sneaking
            if (moveInputMagnitude > 0 && moveInputMagnitude < 0.7f)
            {
                moveSpeed = _moveStats.SneakMovementSpeed;
            }
            // Running
            else if (moveInputMagnitude >= 0.7f)
            {
                moveSpeed = _moveStats.RunMovementSpeed;
            }
            
            CharacterController.Move(move * (Time.deltaTime * moveSpeed));
            
            if (move != Vector3.zero)
            {
                CharacterController.gameObject.transform.forward = move;
            }
        }

        private void CalculateCameraAngleChange(float horizontalInput, float verticalInput)
        {
            if (VirtualController.MoveDirection == Vector2.zero) return;
            
            var targetAngle = Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg + ThirdPersonCamera.eulerAngles.y;
            var rotation = Quaternion.Euler(0f, targetAngle, 0f);
            CharacterController.transform.rotation = Quaternion.Lerp(CharacterController.transform.rotation, rotation, Time.deltaTime * 4f);
        }
    }
}