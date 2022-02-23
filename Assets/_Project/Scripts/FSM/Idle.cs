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
        
        private static readonly int RunSpeedAnimParam = Animator.StringToHash("RunSpeed");


        public override void OnEnter()
        {
            Animator.SetFloat(RunSpeedAnimParam, 0);
        }

        public override void OnUpdate()
        {
            CharacterController.Move(Vector3.down * Time.deltaTime);
        }
    }
}