using ApeEscape.Input;
using UnityEngine;

namespace ApeEscape.FSM
{
    public class Idle : State
    {
        public Idle(CharacterController characterController, VirtualController virtualController, Animator animator) 
            : base(characterController, virtualController, animator)
        {
        }
        
        private static readonly int IdleAnim = Animator.StringToHash("Idle");


        public override void OnEnter()
        {
            Animator.SetBool(IdleAnim, true);
        }

        public override void OnUpdate()
        {
            CharacterController.Move(Vector3.down * Time.deltaTime);
        }
        
        public override void OnExit()
        {
            Animator.SetBool(IdleAnim, false);
        }
    }
}