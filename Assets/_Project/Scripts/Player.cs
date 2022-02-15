using System;
using ApeEscape.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ApeEscape
{
    public class Player : MonoBehaviour
    {
        private CharacterController _characterController;

        private Keyboard _keyboard;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            
            _keyboard = Keyboard.current;
        }

        private void Update()
        {
            var v = new Vector2();
            
            if (_keyboard.wKey.IsPressed())
            {
                v.y = 1.0f;
            }
            else if (_keyboard.sKey.IsPressed())
            {
                v.y = -1.0f;
            }

            _characterController.Move(v * Time.deltaTime * 5.0f);
        }
    }

    public class Mover : MonoBehaviour
    {
        [SerializeField] private float speed;
        
        private CharacterController _characterController;
        private VirtualController _virtualController;

        private Vector3 _moveVector;
        

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _virtualController = GetComponent<VirtualController>();
        }

        private void Update()
        {
            _moveVector.x = _virtualController.MoveDirection.x;
            _moveVector.z = _virtualController.MoveDirection.y;

            _characterController.Move(_moveVector * Time.deltaTime * speed);
        }
    }
}
