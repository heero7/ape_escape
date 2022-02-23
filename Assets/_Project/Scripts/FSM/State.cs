using ApeEscape.Input;
using UnityEngine;

namespace ApeEscape.FSM
{
    public class State : IState
    {
        protected CharacterController CharacterController { get; }
        protected VirtualController VirtualController { get; }

        protected Animator Animator { get; }

        protected State(CharacterController characterController, VirtualController virtualController,
            Animator animator)
        {
            VirtualController = virtualController;
            CharacterController = characterController;
            Animator = animator;
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