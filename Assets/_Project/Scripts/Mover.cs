using System;
using ApeEscape.Input;
using UnityEngine;

namespace ApeEscape
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float speed = 3f;
        [SerializeField] private float rotationSpeed = 4f;
        [SerializeField] private float maxJumpHeight = 2.0f;
        [SerializeField] private float timeToJumpApex = .5f;

        private float _gravity;
        private float _maxJumpForce;
        
        private Vector3 _playerVelocity;
        private bool _isGrounded;
        private const int MaxJumps = 2;
        private int _currentJumps = 0; 
        
        private CharacterController _characterController;
        private VirtualController _virtualController;
        private Transform _thirdPersonCameraTransform;
        

        private void CalculateJumpData()
        {
            _gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            _maxJumpForce = Mathf.Abs(_gravity) * timeToJumpApex;
            // MinJumpForce = Mathf.Sqrt(2 * Mathf.Abs(Gravity) * minJumpHeight);
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _virtualController = GetComponent<VirtualController>();
            if (Camera.main != null)
                _thirdPersonCameraTransform = Camera.main.transform;
            else
                throw new Exception("No camera found, game won't run properly.");
            
            CalculateJumpData();
        }

        private void Update()
        {
            _isGrounded = _characterController.isGrounded;
            if (_isGrounded && _playerVelocity.y < 0)
            {
                _currentJumps = 0;
                _playerVelocity.y = 0f;
            }
            
            var horizontalInput = _virtualController.MoveDirection.x;
            var verticalInput = _virtualController.MoveDirection.y;
            
            var move = new Vector3(horizontalInput, 0, verticalInput);
            move = _thirdPersonCameraTransform.forward * move.z + _thirdPersonCameraTransform.right * move.x;
            move.y = 0;
            
            _characterController.Move(move * (Time.deltaTime * speed));
            
            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
            
            if (_virtualController.JumpPressed && _currentJumps < MaxJumps)
            {
                var jumpMultiplier = _currentJumps > 0 ? .95f : 1f;
                _playerVelocity.y = 0f;
                _playerVelocity.y += _maxJumpForce / jumpMultiplier;
                _virtualController.ClearJumpPressCache();
                _currentJumps++;
            }
            
            _playerVelocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_playerVelocity * Time.deltaTime);

            if (horizontalInput != 0f && verticalInput != 0f)
            {
                // Calculate the angle
                var targetAngle = Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg + _thirdPersonCameraTransform.eulerAngles.y;
                var rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}