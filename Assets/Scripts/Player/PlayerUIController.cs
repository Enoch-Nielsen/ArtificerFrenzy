using System;
using Input;
using UnityEngine;

namespace Player
{
    public class PlayerUIController : MonoBehaviour
    {
        public static PlayerUIController Instance;
        
        [SerializeField] private GameObject settingsMenu;
        private bool isMenuActive;

        private void Start()
        {
            Instance = this;
            
            InputController.Instance.OnMenuPressedAction += OnMenuPressed;
        }

        private void OnMenuPressed(object sender, EventArgs eventArgs)
        {
            isMenuActive = !isMenuActive;
            SetMenuActive(isMenuActive);
        }

        public void SetMenuActive(bool active)
        {
            settingsMenu.SetActive(active);
            GameManager.Instance.SetGameStatus(active ? GameManager.GameStatus.Paused : GameManager.GameStatus.Active);

            Cursor.lockState = active ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = active;
        }
    }
}
