using ApeEscape.Input;
using ApeEscape.Stats;
using UnityEngine;

namespace ApeEscape.FSM.Player
{
    public class ButtSlam : PlayerMovement
    {
        private JumpStats _jumpStats;
        
        public ButtSlam(CharacterController characterController, VirtualController virtualController,
            Animator animator, Transform thirdPersonCamera, JumpStats jumpStats) 
            : base(characterController, virtualController, animator, thirdPersonCamera)
        {
            _jumpStats = jumpStats;
        }
    }
}