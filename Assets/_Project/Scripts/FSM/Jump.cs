using ApeEscape.Input;
using UnityEngine;

namespace ApeEscape.FSM
{
    public class Jump : State
    {
        private readonly JumpStats _jumpStats;
        
        private bool _isGrounded;
        private Vector3 _playerVelocity;
        private int _currentJumps;
        private readonly Transform _thirdPersonCameraTransform;
        
        private static readonly int JumpAnim = Animator.StringToHash("Jump");
        private static readonly int Flip = Animator.StringToHash("Flip");

        public override void OnEnter()
        {
            Animator.SetBool(JumpAnim, true);
            
            _currentJumps = 0;
            _playerVelocity.y = 0f;

            _playerVelocity.y = _jumpStats.MaxJumpForce;
            _currentJumps++;
        }

        public override void OnUpdate()
        {
            _isGrounded = CharacterController.isGrounded;
            if (_isGrounded && CharacterController.velocity.y < 0)
            {
                _currentJumps = 0;
                _playerVelocity.y = 0f;
            }
            
            var horizontalInput = VirtualController.MoveDirection.x;
            var verticalInput = VirtualController.MoveDirection.y;

            var move = new Vector3(horizontalInput, 0, verticalInput);
            move = _thirdPersonCameraTransform.forward * move.z + _thirdPersonCameraTransform.right * move.x;
            move.y = 0;
            CharacterController.Move(move * (Time.deltaTime * 5f));
            
            if (move != Vector3.zero)
            {
                CharacterController.gameObject.transform.forward = move;
            }
            
            
            if (VirtualController.JumpPressed && _currentJumps < 2)
            {
                var jumpMultiplier = _currentJumps > 0 ? .95f : 1f;
                _playerVelocity.y = 0f;
                _playerVelocity.y += _jumpStats.MaxJumpForce / jumpMultiplier;
                VirtualController.ClearJumpPressCache();
                _currentJumps++;
                Animator.SetBool(Flip, true);
            }
            
            _playerVelocity.y += _jumpStats.Gravity * Time.deltaTime;
            CharacterController.Move(_playerVelocity * Time.deltaTime);
        }

        public override void OnExit()
        {
            Animator.SetBool(JumpAnim, false);
            Animator.SetBool(Flip, false);
        }

        public Jump(CharacterController characterController, VirtualController virtualController,
            JumpStats jumpStats, Transform thirdPersonCameraTransform, Animator animator) 
            : base(characterController, virtualController, animator)
        {
            _jumpStats = jumpStats;
            _thirdPersonCameraTransform = thirdPersonCameraTransform;
        }
    }
}