using ApeEscape.Input;
using UnityEngine;

namespace ApeEscape.FSM
{
    public class Run : State
    {
        private readonly Transform _thirdPersonCameraTransform;
        
        private static readonly int RunAnim = Animator.StringToHash("Run");


        public override void OnEnter()
        {
            Animator.SetBool(RunAnim, true);
        }

        public override void OnUpdate()
        {
            CharacterController.Move(Vector3.down * Time.deltaTime);
            
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
            
            // Calculate the angle
            var targetAngle = Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg + _thirdPersonCameraTransform.eulerAngles.y;
            var rotation = Quaternion.Euler(0f, targetAngle, 0f);
            CharacterController.transform.rotation = Quaternion.Lerp(CharacterController.transform.rotation, rotation, Time.deltaTime * 4f);
        }

        public override void OnExit()
        {
            Animator.SetBool(RunAnim, false);
        }

        public Run(CharacterController characterController, VirtualController virtualController, Transform thirdPersonCameraTransform, Animator animator) 
            : base(characterController, virtualController, animator)
        {
            _thirdPersonCameraTransform = thirdPersonCameraTransform;
        }
    }
}