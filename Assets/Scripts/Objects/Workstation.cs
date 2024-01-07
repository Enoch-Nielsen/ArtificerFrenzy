using Interface;
using Player;
using UnityEngine;

namespace Objects
{
    public class Workstation : MonoBehaviour, I_Interactable
    {
        [SerializeField] private Transform cameraBoom, lookAt;
        [SerializeField] private bool isTargeted = false;
        
        public void Interact()
        {
            if (!isTargeted)
                CameraController.Instance.SwitchCameraTarget(cameraBoom, true, true, lookAt);
            else
                CameraController.Instance.SwitchCameraTarget(FirstPersonController.Instance.transform);
            
            isTargeted = !isTargeted;
        }

        public bool GetIsTargeted()
        {
            return isTargeted;
        }
    }
}