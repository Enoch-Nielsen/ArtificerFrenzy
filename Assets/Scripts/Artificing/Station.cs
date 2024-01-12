using System;
using Objects;
using UnityEngine;

namespace Artificing
{
    public class Station : MonoBehaviour
    {
        public event Action OnPlayerExitStation;

        [Header("Fields")] 
        [SerializeField] private Transform holdTransform;
        [SerializeField] private float throwForce;
        
        [Header("Values")]
        [SerializeField] private Pickup currentHeldObject;
        
        /// <summary>
        /// Sets the players currently held artifact onto the station.
        /// </summary>
        /// <param name="pickup"></param>
        public void SetArtifact(Pickup pickup)
        {
            if (currentHeldObject != null) return;

            if (pickup.TryGetComponent<Artifact>(out Artifact artifact))
            {
                Debug.Log("Received Artifact");
            }
            else
            {
                pickup.Throw(throwForce, transform.position - Camera.main!.transform.position);
                return;
            }
            
            currentHeldObject = pickup;
            pickup.DoPickup(holdTransform);
        }

        /// <summary>
        /// Removes the artifact from the station.
        /// </summary>
        public void RemoveArtifact()
        {
            if (currentHeldObject == null)
            {
                OnPlayerExitStation?.Invoke();
                return;
            }
            
            Pickup tempPickup = currentHeldObject;
            
            OnPlayerExitStation?.Invoke();
            
            MakeDropQuery();
        }
        
        /// <summary>
        /// Makes a query to drop the artifact.
        /// </summary>
        public void MakeDropQuery()
        {
            if (currentHeldObject == null) return;
            
            currentHeldObject.Drop();
            currentHeldObject.Interact();
            currentHeldObject = null;
        }

        public bool TryGetArtifact(out Artifact artifact)
        {
            artifact = null;
            if (currentHeldObject == null)
                return false;
            
            bool obtained = currentHeldObject.TryGetComponent(out Artifact a);
            artifact = a;
            return obtained;
        }
    }
}