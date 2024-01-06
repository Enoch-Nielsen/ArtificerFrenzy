using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton, settingsButton, exitButton;
        
        private void Start()
        {
            playButton.onClick.AddListener(OnPlayPressed);
            settingsButton.onClick.AddListener(OnSettingsPressed);
            exitButton.onClick.AddListener(OnExitPressed);
        }

        private void OnPlayPressed()
        {
            MenuManager.Instance.Play();
        }

        private void OnSettingsPressed()
        {
            MenuManager.Instance.SetCurrentMenuState(MenuManager.MenuState.Settings);
        }

        private void OnExitPressed()
        {
            MenuManager.Instance.Exit();
        }
    }
}