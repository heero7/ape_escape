using ApeEscape.FSM;
using ApeEscape.FSM.Player;
using ApeEscape.Input;
using ApeEscape.Stats;
using UnityEngine;

namespace ApeEscape
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform thirdPersonCameraTransform = default!;
        [SerializeField] private JumpStats jumpStats = new JumpStats();
        [SerializeField] private MoveStats moveStats = new MoveStats();
        [SerializeField] private Transform groundCheck = default!;
        [SerializeField] private float groundCheckRadius = 0.25f;
        [SerializeField] private LayerMask groundLayer;
        
        private StateMachine _movement;
        private CharacterController _characterController;
        private VirtualController _virtualController;
        private Animator _animator;

        private readonly Collider[] _contactColliders = new Collider[10];

        public static bool IsGrounded { get; private set; }

        private void CheckGrounded()
        {
            var numberOfColliders = Physics.OverlapSphereNonAlloc(groundCheck.transform.position, groundCheckRadius, _contactColliders, groundLayer);
            
            IsGrounded = numberOfColliders != 0;
        }

        private void InitializeStates()
        {
            var groundLocomotion = new GroundLocomotion(_characterController, _virtualController, _animator, 
                thirdPersonCameraTransform, moveStats);
            var aerialLocomotion = new AerialLocomotion(_characterController, _virtualController, _animator,
                thirdPersonCameraTransform, jumpStats);
            var crawlLocomotion = new CrawlLocomotion(_characterController, _virtualController, _animator, 
                thirdPersonCameraTransform, moveStats);
            var buttSlamAction = new ButtSlam(_characterController, _virtualController, _animator,
                thirdPersonCameraTransform, jumpStats);
            
            // Start in the ground state.
            _movement.SetState(groundLocomotion);
            
            // Jump Requested
            _movement.AddTransition(groundLocomotion, aerialLocomotion, () =>
            {
                if (_virtualController.JumpPressed)
                {
                    _virtualController.ClearJumpPressCache();
                    AerialLocomotion.MarkTransitionAsRising();
                    return true;
                }

                return false;
            });
            
            // Ground no longer detected from Ground state.
            _movement.AddTransition(groundLocomotion, aerialLocomotion, () =>
            {
                if (!IsGrounded)
                {
                    AerialLocomotion.MarkTransitionAsFalling();
                    return true;
                }

                return false;
            });
            
            // Aerial to ground detected 
            _movement.AddTransition(aerialLocomotion, groundLocomotion, () => IsGrounded);
            
            // Grounded to crawl
                // TODO: Transition should restrict player movement for X frames
            _movement.AddTransition(groundLocomotion, crawlLocomotion, () =>
            {
                if (_virtualController.LeftAnalogPressed)
                {
                    _virtualController.ResetLeftAnalogPressed();
                    return true;
                }

                return false;
            });
            
            // Crawl to grounded
                // TODO: Transition should restrict player movement for X frames
            _movement.AddTransition(crawlLocomotion, groundLocomotion, () =>
            {
                if (!_virtualController.LeftAnalogHeld)
                {
                    return true;
                }

                return false;
            });
            
            // ButtSlam detected.
                // TODO: Before going downward, full animation should play. (Check animation end event)
            _movement.AddTransition(aerialLocomotion, buttSlamAction, () =>
            {
                if (_virtualController.RightAnalogPressed)
                {
                    _virtualController.ResetRightAnalogPressed();
                    return true;
                }

                return false;
            });
            
            // ButtSlam reached ground.
            _movement.AddTransition(buttSlamAction, groundLocomotion, () => IsGrounded);
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _virtualController = GetComponent<VirtualController>();

            _animator = GetComponentInChildren<Animator>();
            
            _movement = new StateMachine();

            InitializeStates();
        }

        private void Update()
        {
            CheckGrounded();
            _movement.Tick();
        }

        private void OnValidate() => jumpStats?.CalculateJumpData();

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
        }
    }
}
