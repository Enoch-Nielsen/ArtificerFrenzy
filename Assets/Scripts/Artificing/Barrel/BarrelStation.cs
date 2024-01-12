using System.Collections.Generic;
using Interface;
using Objects;
using Player;
using UnityEngine;

namespace Artificing.Barrel
{
    public class BarrelStation : MonoBehaviour, I_Interactable
    {
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private GameObject artifactPrefab;
        
        public void Interact()
        {
            if (PlayerInteractionController.Instance.PlayerHasArtifact())
                return;

            GameObject newArtifact = Instantiate(artifactPrefab, transform.position + new Vector3(0f, 1.6f, 0f), Quaternion.identity);
            
            newArtifact.GetComponent<Artifact>().SetWeaponType(weaponType);
            newArtifact.GetComponent<ArtifactMeshSwitcher>().SetWeaponType(weaponType);
            newArtifact.GetComponent<Pickup>().Interact();
        }
    }
}