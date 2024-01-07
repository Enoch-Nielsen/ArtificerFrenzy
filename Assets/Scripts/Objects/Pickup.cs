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

        /// <summary>
        /// Picks up this pickup, and adjusts accordingly.
        /// </summary>
        /// <param name="target"></param>
        public void DoPickup(Transform target)
        {
            targetTransform = target;
            isBeingHeld = true;
            
            Debug.Log("Picked Up: " + targetTransform.name);
        }

        /// <summary>
        /// Reactivates physics and drops this pickup.
        /// </summary>
        public void Drop()
        {
            targetTransform = null;
            isBeingHeld = false;
            
            Debug.Log("Dropped");
        }

        /// <summary>
        /// Launches the pickup in a specified direction and force.
        /// </summary>
        /// <param name="force"></param>
        /// <param name="direction"></param>
        public void Throw(float force, Vector3 direction)
        {
            rb.isKinematic = false;
            
            rb.AddForce(direction.normalized * force);
            
            Debug.Log("Thrown");
        }
        
        public bool GetIsHeld()
        {
            return isBeingHeld;
        }
    }
}