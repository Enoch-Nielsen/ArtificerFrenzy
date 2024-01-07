using UnityEngine;

namespace Interface
{
    public interface I_Interactable
    {
        /// <summary>
        /// Called on various objects when the player needs to interact with them.
        /// </summary>
        public void Interact()
        {
            Debug.Log("Interaction occured}");
        }
    }
}
