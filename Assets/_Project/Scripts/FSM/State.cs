using ApeEscape.Input;
using UnityEngine;

namespace ApeEscape.FSM
{
    public class State : IState
    {
        protected CharacterController CharacterController { get; }
        protected VirtualController VirtualController { get; }

        public State(CharacterController characterController, VirtualController virtualController)
        {
            VirtualController = virtualController;
            CharacterController = characterController;
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