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
        
        [Header("Offset")]
        [SerializeField] private Vector3 baseOffset;
        [SerializeField] private Vector3 shakeOffset;
        
        [Header("Camera Options")]
        [SerializeField] private bool doInterpolation;
        [SerializeField] private bool lockCamera;

        [Header("Values")]
        [SerializeField] private float lookInput;
        [SerializeField] private float lookOutput;
        [SerializeField] private float xRotation;

        private void Awake()
        {
            transform.parent = null;
        }

        private void Start()
        {
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
            }
        
            if (!doInterpolation)
            {
                // Do Follow
                transform.position = followTransform.position + baseOffset + shakeOffset;
                transform.localRotation = Quaternion.Euler(new Vector3(xRotation, followTransform.eulerAngles.y, 0f));
            }
        }
    
        public void Shake()
        {
        
        }
    }
}
