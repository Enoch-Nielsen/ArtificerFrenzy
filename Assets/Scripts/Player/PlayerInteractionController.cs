using System;
using Artificing;
using Input;
using Interface;
using Objects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInteractionController : MonoBehaviour
    {
        public static PlayerInteractionController Instance;
        
        [Header("References")]
        [SerializeField] private Transform playerPickupTransform;

        [Header("Fields")] 
        [SerializeField] private float pickupDistance;
        [SerializeField] private float throwForce;
        
        [Header("Values")] 
        [SerializeField] private Pickup currentHeldObject;

        private void Start()
        {
            Instance = this;
            
            InputController.Instance.OnInteractPressedAction += OnPlayerInteract;
            InputController.Instance.OnThrowPressedAction += OnPlayerThrow;
            InputController.Instance.OnDropPressedAction += OnPlayerDrop;
        }

        private void OnPlayerInteract(object sender, EventArgs eventArgs)
        {
            RaycastHit[] hits = new RaycastHit[10];
            var size = Physics.RaycastNonAlloc(Camera.main!.transform.position, Camera.main.transform.forward, hits, pickupDistance);
            
            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    if (hit.transform == null)
                        continue;

                    if (currentHeldObject != null)
                        if (hit.transform.Equals(currentHeldObject.transform))
                            continue; 
                    
                    if (hit.transform.CompareTag("Interactable"))
                        if (hit.transform.TryGetComponent(out I_Interactable iComponent))
                        {
                            iComponent.Interact();
                            break;
                        }
                }
            }
        }

        private void OnPlayerDrop(object sender, EventArgs eventArgs)
        {
            if (currentHeldObject == null) return;
            
            MakeDropQuery();
        }

        private void OnPlayerThrow(object sender, EventArgs eventArgs)
        {
            if (currentHeldObject == null) return;
            
            Pickup tempPickup = currentHeldObject;
            
            MakeDropQuery();
            tempPickup.Throw(throwForce);
        }

        public void MakePickupQuery(Pickup pickup)
        {
            if (currentHeldObject != null) return;
            
            currentHeldObject = pickup;
            pickup.DoPickup(playerPickupTransform);
        }

        public void MakeDropQuery()
        {
            if (currentHeldObject == null) return;
            
            currentHeldObject.Drop();
            currentHeldObject = null;
        }

        public float GetPickupDistance()
        {
            return pickupDistance;
        }
    }
}