using System;
using Artificing;
using Interface;
using Player;
using UnityEngine;

namespace Objects
{
    public class Workstation : MonoBehaviour, I_Interactable
    {
        
        [Header("References")]
        [SerializeField] private Transform cameraBoom, lookAt;
        
        [Header("Fields")]
        [SerializeField] private bool hasStation;
        
        [Header("Values")]
        [SerializeField] private bool isTargeted = false;
        
        public void Interact()
        {
            if (!isTargeted)
                CameraController.Instance.SwitchCameraTarget(cameraBoom, true, true, lookAt);
            else
                CameraController.Instance.SwitchCameraTarget(FirstPersonController.Instance.transform);
            
            isTargeted = !isTargeted;

            if (isTargeted)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (hasStation)
            {
                if (PlayerInteractionController.Instance.PlayerHasArtifact())
                {
                    if (TryGetComponent<Station>(out Station station))
                    {
                        station.SetArtifact(PlayerInteractionController.Instance.TakeArtifactQuery());
                    }
                }
                else if(TryGetComponent<Station>(out Station station))
                {
                    station.RemoveArtifact();
                }
            }
        }

        public bool GetIsTargeted()
        {
            return isTargeted;
        }
    }
}