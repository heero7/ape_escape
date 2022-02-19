using ApeEscape.Input;
using UnityEngine;

namespace ApeEscape.FSM
{
    public class Idle : State
    {
        public Idle(CharacterController characterController, VirtualController virtualController) : base(characterController, virtualController)
        {
        }

        public override void OnUpdate()
        {
            CharacterController.Move(Vector3.down * Time.deltaTime);
        }
    }
}