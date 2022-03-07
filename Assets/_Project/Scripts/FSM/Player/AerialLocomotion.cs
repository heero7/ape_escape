using ApeEscape.Input;
using ApeEscape.Stats;
using UnityEngine;

namespace ApeEscape.FSM.Player
{
    public class AerialLocomotion : PlayerMovement
    {
        private static int _currentJumps;
        private const int MaxJumps = 2;

        private readonly JumpStats _jumpStats;
        
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Flip = Animator.StringToHash("DoubleJump");
        
        private static bool _markedAsFalling;
        private static bool _markedAsRising;
        
        private Vector3 _playerVelocity;
        

        public AerialLocomotion(CharacterController characterController, 
            VirtualController virtualController, Animator animator, Transform thirdPersonCamera, JumpStats jumpStats) 
            : base(characterController, virtualController, animator, thirdPersonCamera)
        {
            _jumpStats = jumpStats;
        }

        public static void MarkTransitionAsFalling() => _markedAsFalling = true;
        public static void MarkTransitionAsRising() => _markedAsRising = true;

        public override void OnEnter()
        {
            Animator.SetBool(Jump, true);   // Set this no matter what, both falling and rising need it
            
            if (_markedAsFalling)
            {
                // Handle Falling Logic
                HandleInitialFallingLogic();
            }
            
            if (_markedAsRising)
            {
                // Handle Rising Logic
                HandleInitialRisingLogic();
            }
        }

        public override void OnExit()
        {
            _markedAsFalling = false;
            _markedAsRising = false;
            
            Debug.Log("Setting flags to false");
            
            Animator.SetBool(Jump, false);
        }

        private void HandleInitialRisingLogic()
        {
            _currentJumps = 0;
            _playerVelocity.y = _jumpStats.MaxJumpForce;
        }

        private void HandleInitialFallingLogic()
        {
            _currentJumps = 1;
            _playerVelocity.y = 0f;
        }

        public override void OnUpdate()
        {
            HandleHorizontalPlaneMovement();
            
            if (VirtualController.JumpPressed && _currentJumps < MaxJumps)
            {
                var jumpMultiplier = _currentJumps > 0 ? .95f : 1f;
                _playerVelocity.y = 0f;
                _playerVelocity.y += _jumpStats.MaxJumpForce / jumpMultiplier;
                VirtualController.ClearJumpPressCache();
                _currentJumps++;
                Animator.SetTrigger(Flip);
            }

            _playerVelocity.y += _jumpStats.Gravity * Time.deltaTime;
            CharacterController.Move(_playerVelocity * Time.deltaTime);
        }

        private void HandleHorizontalPlaneMovement()
        {
            var move = new Vector3(VirtualController.MoveDirection.x, 0, VirtualController.MoveDirection.y);
            move = ThirdPersonCamera.forward * move.z + ThirdPersonCamera.right * move.x;
            move.y = 0;
            
            CharacterController.Move(move * (Time.deltaTime * _jumpStats.AerialMoveSpeed));
            
            if (move != Vector3.zero)
            {
                CharacterController.gameObject.transform.forward = move;
            }
        }
    }
}