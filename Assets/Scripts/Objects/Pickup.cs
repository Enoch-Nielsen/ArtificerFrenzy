using System;
using Interface;
using Player;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class Pickup : MonoBehaviour, I_Interactable
    {
        [Header("References")]
        [SerializeField] private Transform targetTransform;
        private Rigidbody rb;
        private BoxCollider bc;
        
        [Header("Fields")]
        [SerializeField] private float positionInterpolationSpeed;
        [SerializeField] private float rotationInterpolationSpeed;
        
        [Header("Values")] 
        [SerializeField] private bool isBeingHeld;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            bc = GetComponent<BoxCollider>();
        }

        private void Update()
        {
            if (isBeingHeld && targetTransform != null)
            {
                transform.position =
                    Vector3.Lerp(transform.position, targetTransform.position, positionInterpolationSpeed * Time.deltaTime);
                
                transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation, rotationInterpolationSpeed * Time.deltaTime);
            }

            rb.isKinematic = isBeingHeld;
            bc.enabled = !isBeingHeld;
        }

        public void Interact()
        {
            if (!isBeingHeld)
                PlayerInteractionController.Instance.MakePickupQuery(this);
        }

        public void DoPickup(Transform target)
        {
            targetTransform = target;
            isBeingHeld = true;
        }

        public void Drop()
        {
            targetTransform = null;
            isBeingHeld = false;
        }

        public void Throw(float force)
        {
            rb.isKinematic = false;
            
            rb.AddForce(Camera.main!.transform.forward * force);
        }

        public bool GetIsHeld()
        {
            return isBeingHeld;
        }
    }
}