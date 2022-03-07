using ApeEscape.Input;
using UnityEngine;

namespace ApeEscape.FSM.Player
{
    public class PlayerMovement : IState
    {
        protected CharacterController CharacterController { get; }
        protected VirtualController VirtualController { get; }
        protected Animator Animator { get; }
        protected Transform ThirdPersonCamera { get; }

        protected PlayerMovement(CharacterController characterController, 
            VirtualController virtualController, Animator animator, Transform thirdPersonCamera)
        {
            VirtualController = virtualController;
            CharacterController = characterController;
            Animator = animator;
            ThirdPersonCamera = thirdPersonCamera;
        }
        public virtual void OnEnter()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}