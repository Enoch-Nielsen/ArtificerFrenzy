using System;
using Input;
using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;
    
        [Header("Base Fields")]
        [SerializeField] private Transform followTransform;
        [SerializeField] private Transform lookAtTarget;
        
        [Header("Offset")]
        [SerializeField] private Vector3 baseOffset;
        [SerializeField] private Vector3 shakeOffset;
        
        [Header("Camera Options")]
        [SerializeField] private bool doInterpolation;
        [SerializeField] private float interpolationSpeed;
        [SerializeField] private bool lockCamera;

        [Header("Values")]
        [SerializeField] private float lookInput;
        [SerializeField] private float lookOutput;
        [SerializeField] private float xRotation;
        private Quaternion previousLookAt;

        private void Awake()
        {
            transform.parent = null;
        }

        private void Start()
        {
            Instance = this;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            lookInput = -InputController.Instance.GetLookInput().y;
            lookOutput = lookInput * (GameManager.Instance.baseLookSensitivity * GameManager.Instance.lookMultiplier * Time.deltaTime);
            
            if (!lockCamera)
            {
                // Adjust Rotation
                xRotation += lookOutput;

                xRotation = Mathf.Clamp(xRotation, -85f, 85f);
                transform.localRotation = Quaternion.Euler(new Vector3(xRotation, followTransform.eulerAngles.y, 0f));
            }
            else
                transform.LookAt(lookAtTarget);
        
            if (!doInterpolation)
            {
                // Do Follow
                transform.position = followTransform.position + baseOffset + shakeOffset;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, followTransform.position + shakeOffset,
                    interpolationSpeed * Time.deltaTime);
            }
        }

        public void SwitchCameraTarget(Transform target, bool doLock = false, bool interpolate = false, Transform lookAt = null)
        {
            followTransform = target;
            lockCamera = doLock;
            doInterpolation = interpolate;
            lookAtTarget = lookAt;

            if (target.Equals(FirstPersonController.Instance.transform))
            {
                transform.rotation = previousLookAt;
            }
            else
            {
                previousLookAt = transform.rotation;
            }
        }

        public void Shake()
        {
        
        }
    }
}
