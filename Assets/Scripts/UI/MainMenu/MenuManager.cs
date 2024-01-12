using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;
        public enum MenuState
        {
            Main, Settings
        }

        [field: SerializeField] public MenuState CurrentMenuState { get; private set; }

        [SerializeField] private GameObject mainMenu, settingsMenu;
        
        private void Start()
        {
            Instance = this;
            
            SetCurrentMenuState(MenuState.Main);
        }

        public void Play()
        {
            SceneManager.LoadSceneAsync(1);
        }

        public void Exit()
        {
            Application.Quit();
            //EditorApplication.ExitPlaymode();
        }

        public void SetCurrentMenuState(MenuState targetState)
        {
            CurrentMenuState = targetState;
            UpdateMenu();
        }

        private void UpdateMenu()
        {
            switch (CurrentMenuState)
            {
                case MenuState.Main:
                    mainMenu.SetActive(true);
                    settingsMenu.SetActive(false);
                    break;
                case MenuState.Settings:
                    mainMenu.SetActive(false); 
                    settingsMenu.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
