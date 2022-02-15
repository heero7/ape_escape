using System;
using UnityEngine;

namespace ApeEscape.Input
{
    public class VirtualController : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader = default!;
        private InputReader InputReader => inputReader;
        
        public Vector2 MoveDirection { get; private set; }
        

        private void OnMove(Vector2 dir)
        {
            MoveDirection = dir;
        }
        
        private void OnEnable()
        {
            inputReader.OnMoveEvent += OnMove;
        }

        private void OnDisable()
        {
            inputReader.OnMoveEvent -= OnMove;
        }
    }
}