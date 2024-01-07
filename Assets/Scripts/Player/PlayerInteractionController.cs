using System;
using System.ComponentModel;
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
            InputController.Instance.OnTooltipPressedAction += OnPlayerActivateTooltips;
        }

        /// <summary>
        /// Called when the player attempts to interact with an object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
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

        /// <summary>
        /// Called when the player attempts to activate tooltips.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnPlayerActivateTooltips(object sender, EventArgs eventArgs)
        {
            GameManager.Instance.ToggleTooltips();
        }

        /// <summary>
        /// Called when the player attempts to drop their currently held object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnPlayerDrop(object sender, EventArgs eventArgs)
        {
            if (currentHeldObject == null) return;
            
            MakeDropQuery();
        }

        /// <summary>
        /// Called when the player attempts to throw their currently held object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnPlayerThrow(object sender, EventArgs eventArgs)
        {
            if (currentHeldObject == null) return;
            
            Pickup tempPickup = currentHeldObject;
            
            MakeDropQuery();
            tempPickup.Throw(throwForce, Camera.main!.transform.forward);
        }
        
        /// <summary>
        /// Attempts to pickup the given pickup.
        /// </summary>
        /// <param name="pickup"></param>
        public void MakePickupQuery(Pickup pickup)
        {
            if (currentHeldObject != null) return;
            
            currentHeldObject = pickup;
            pickup.DoPickup(playerPickupTransform);
        }

        /// <summary>
        /// Attempts to drop the current held pickup.
        /// </summary>
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

        public bool PlayerHasArtifact()
        {
            return currentHeldObject != null;
        }

        /// <summary>
        /// Attempts to return the current artifact to a station to be held by the station.
        /// </summary>
        /// <returns></returns>
        public Pickup TakeArtifactQuery()
        {
            Pickup tempPickup = currentHeldObject;
            
            MakeDropQuery();

            return tempPickup;
        }
    }
}