using ApeEscape.Input;
using ApeEscape.Stats;
using UnityEngine;

namespace ApeEscape.FSM.Player
{
    public class CrawlLocomotion : PlayerMovement
    {
        private MoveStats _moveStats;
        private static readonly int Crouching = Animator.StringToHash("Crouching");
        private bool _animationLagEnded;
        private int _frames;
        private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

        public CrawlLocomotion(CharacterController characterController, VirtualController virtualController,
            Animator animator, Transform thirdPersonCamera, MoveStats moveStats) 
            : base(characterController, virtualController, animator, thirdPersonCamera)
        {
            _moveStats = moveStats;
        }

        public override void OnEnter()
        {
            Animator.SetBool(Crouching, true);
            _frames = 0;
        }

        public override void OnUpdate()
        {
            WaitForLagToEnd();

            // if (_animationLagEnded)
            // {
            //     // Do the logic.
            // }
            
            var horizontalInput = VirtualController.MoveDirection.x;
            var verticalInput = VirtualController.MoveDirection.y; 
            
            HandleHorizontalPlaneMovement(horizontalInput, verticalInput);
            CalculateCameraAngleChange(horizontalInput, verticalInput);
        }
        
        private void HandleHorizontalPlaneMovement(float horizontalInput, float verticalInput)
        {
            var move = new Vector3(horizontalInput, 0, verticalInput);
            move = ThirdPersonCamera.forward * move.z + ThirdPersonCamera.right * move.x;
            move.y = 0;

            var movingSpeed = VirtualController.MoveDirection == Vector2.zero 
                ? 0
                : 1f;
            
            Animator.SetFloat(MoveSpeed, movingSpeed);
            
            CharacterController.Move(move * (Time.deltaTime * _moveStats.CrawlMovementSpeed));
            
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

        private void WaitForLagToEnd()
        {
            // TODO: Implement lag to prevent moving until state is ready
            
            _frames++;

            if (_frames == 10)
                _animationLagEnded = true;
        }

        public override void OnExit()
        {
            Animator.SetBool(Crouching, false);
            _frames = 0;
        }
    }
}