using ApeEscape.Input;
using Cinemachine;
using UnityEngine;

namespace ApeEscape
{
    public class FreeLookToggle : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader = default!;
        
        [SerializeField] private CinemachineFreeLook thirdPersonCamera = default!;
        [SerializeField] private CinemachineVirtualCamera firstPersonCamera = default!;
        private CinemachinePOV _firstPersonPovComponent;

        private Transform _currentPlayerTransform;

        private bool _isFreeLooking;
        
        private void ResetCameraBehindPlayer()
        {   
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
                
                SetFirstPersonCameraInPlayerDirection();
            }
            else
            {
                inputReader.EnableMovement();
                
                thirdPersonCamera.Priority = 1;
                firstPersonCamera.Priority = 0;
            }
        }

        private void SetFirstPersonCameraInPlayerDirection()
        {
            _firstPersonPovComponent.m_HorizontalAxis.Value = _currentPlayerTransform.rotation.eulerAngles.y;
            _firstPersonPovComponent.m_VerticalAxis.Value = 0;
        }
        
        private void Awake()
        {
            _currentPlayerTransform = FindObjectOfType<CharacterController>().transform;

            _firstPersonPovComponent = firstPersonCamera.GetCinemachineComponent<CinemachinePOV>();
            
            thirdPersonCamera.Priority = 1;
            firstPersonCamera.Priority = 0;
        }

        private void OnEnable()
        {
            inputReader.FreeLookEvent += OnFreeLookPressed;
            inputReader.ResetCameraEvent += ResetCameraBehindPlayer;
        }

        private void OnDisable()
        {
            inputReader.FreeLookEvent -= OnFreeLookPressed;
            inputReader.ResetCameraEvent -= ResetCameraBehindPlayer;
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