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

        public override void OnEnter()
        {
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
            
            var move = new Vector3(horizontalInput, 0, verticalInput)
            {
                y = 0
            };

            CharacterController.Move(move * (Time.deltaTime * 5f));
            
            
            if (VirtualController.JumpPressed && _currentJumps < 2)
            {
                var jumpMultiplier = _currentJumps > 0 ? .95f : 1f;
                _playerVelocity.y = 0f;
                _playerVelocity.y += _jumpStats.MaxJumpForce / jumpMultiplier;
                VirtualController.ClearJumpPressCache();
                _currentJumps++;
            }
            
            _playerVelocity.y += _jumpStats.Gravity * Time.deltaTime;
            CharacterController.Move(_playerVelocity * Time.deltaTime);
        }

        public override void OnExit()
        {
            
        }

        public Jump(CharacterController characterController, VirtualController virtualController,
            JumpStats jumpStats) : base(characterController, virtualController)
        {
            _jumpStats = jumpStats;
        }
    }
}