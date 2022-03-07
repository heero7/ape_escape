using System;
using UnityEngine;

namespace ApeEscape.Stats
{
    [Serializable]
    public class JumpStats
    {
        [field: SerializeField] public float AerialMoveSpeed { get; private set; } = 3.5f;
        [SerializeField] private float maxJumpHeight = 2.0f;
        [SerializeField] private float minJumpHeight = 1.0f;
        [SerializeField] private float timeToJumpApex = 0.5f;

        
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