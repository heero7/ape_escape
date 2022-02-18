using ApeEscape.Input;
using Cinemachine;
using UnityEngine;

namespace ApeEscape
{
    public class FreeLookToggle : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader = default!;
        
        [SerializeField] private CinemachineFreeLook thirdPersonCamera;
        [SerializeField] private CinemachineVirtualCamera firstPersonCamera;

        private Transform _currentPlayerTransform;

        private bool _isFreeLooking;
        
        private void ResetCameraBehindPlayer()
        {
            // Todo: 
            // thirdPersonCamera.m_XAxis = 
            
            var rotation = _currentPlayerTransform.rotation.eulerAngles.y;
            

            thirdPersonCamera.m_XAxis.Value = rotation;
            thirdPersonCamera.m_YAxis.Value = 0.5f;
        }
        
        private void OnFreeLookPressed()
        {
            _isFreeLooking = !_isFreeLooking;

            if (_isFreeLooking)
            {
                inputReader.DisableMovement();
                thirdPersonCamera.Priority = 0;
                firstPersonCamera.Priority = 1;
            }
            else
            {
                inputReader.EnableMovement();
                
                thirdPersonCamera.Priority = 1;
                firstPersonCamera.Priority = 0;
            }
        }
        
        private void Awake()
        {
            _currentPlayerTransform = FindObjectOfType<CharacterController>().transform;
            
            thirdPersonCamera.Priority = 1;
            firstPersonCamera.Priority = 0;
        }

        private void OnEnable()
        {
            inputReader.OnFreeLookEvent += OnFreeLookPressed;
            inputReader.OnResetCameraEvent += ResetCameraBehindPlayer;
        }

        private void OnDisable()
        {
            inputReader.OnFreeLookEvent -= OnFreeLookPressed;
            inputReader.OnResetCameraEvent -= ResetCameraBehindPlayer;
        }

        private void OnValidate()
        {
            if (thirdPersonCamera == null)
                Debug.LogError("Missing field for third person camera");
            
            if (firstPersonCamera == null)
                Debug.LogError("Missing field for first person camera");
        }
    }
}