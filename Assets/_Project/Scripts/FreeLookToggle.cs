using ApeEscape.Input;
using Cinemachine;
using UnityEngine;

namespace ApeEscape
{
    public class FreeLookToggle : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader = default!;
        
        private CinemachineFreeLook _thirdPersonCamera;
        private CinemachineVirtualCamera _firstPersonCamera;

        private bool _isFreeLooking;
        
        private void Awake()
        {
            _thirdPersonCamera = FindObjectOfType<CinemachineFreeLook>();
            _firstPersonCamera = FindObjectOfType<CinemachineVirtualCamera>();

            _thirdPersonCamera.gameObject.SetActive(true);
            _firstPersonCamera.gameObject.SetActive(false);
        }

        private void OnEnable() => inputReader.OnFreeLookEvent += OnFreeLookPressed;
        private void OnDisable() => inputReader.OnFreeLookEvent -= OnFreeLookPressed;

        private void OnFreeLookPressed()
        {
            _isFreeLooking = !_isFreeLooking;

            if (_isFreeLooking)
            {
                inputReader.DisableMovement();
                _thirdPersonCamera.gameObject.SetActive(false);
                _firstPersonCamera.gameObject.SetActive(true);
            }
            else
            {
                inputReader.EnableMovement();
                _thirdPersonCamera.gameObject.SetActive(true);
                _firstPersonCamera.gameObject.SetActive(false);
            }
        }
    }
}