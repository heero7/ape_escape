using System;
using ApeEscape.FSM;
using ApeEscape.Input;
using UnityEngine;

namespace ApeEscape
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform thirdPersonCameraTransform;
        [SerializeField] private JumpStats jumpStats = new JumpStats();
        
        private StateMachine _stateMachine;
        private CharacterController _characterController;
        private VirtualController _virtualController;
        

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _virtualController = GetComponent<VirtualController>();
            
            _stateMachine = new StateMachine();

            var idle = new Idle(_characterController, _virtualController);
            
            var run = new Run(_characterController, _virtualController, thirdPersonCameraTransform);
            var jump = new Jump(_characterController, _virtualController, jumpStats);
            
            _stateMachine.SetState(idle);

            var jumpCondition = new Func<bool>(() =>
            {
                if (!_virtualController.JumpPressed) return false;
                _virtualController.ClearJumpPressCache();
                return true;

            });
            

            _stateMachine.AddTransition(idle, run, () => _virtualController.MoveDirection != Vector2.zero);
            _stateMachine.AddTransition(run, idle, () => _virtualController.MoveDirection == Vector2.zero);
            _stateMachine.AddTransition(run, jump, jumpCondition);
            _stateMachine.AddTransition(idle, jump, jumpCondition);
            _stateMachine.AddTransition(jump, idle, () => _virtualController.MoveDirection == Vector2.zero 
                                                          && _characterController.isGrounded);
            _stateMachine.AddTransition(jump, run, () =>  _virtualController.MoveDirection != Vector2.zero 
                                                          && _characterController.isGrounded);
        }

        private void Update() => _stateMachine.Tick();

        private void OnValidate() => jumpStats?.CalculateJumpData();
    }

    [Serializable]
    public class JumpStats
    {
        [SerializeField] private float maxJumpHeight = 2.0f;
        [SerializeField] private float minJumpHeight = 1.0f;
        [SerializeField] private float timeToJumpApex = .5f;

        public float Gravity { get; private set; }
        public float MaxJumpForce { get; private set; }
        public float MinJumpForce { get; private set; }

        public JumpStats()
        {
            CalculateJumpData();
        }
        
        public void CalculateJumpData()
        {
            Gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            MaxJumpForce = Mathf.Abs(Gravity) * timeToJumpApex;
            MinJumpForce = Mathf.Sqrt(2 * Mathf.Abs(Gravity) * minJumpHeight);
        }
    }
}
