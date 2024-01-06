using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider, lookSlider;
        [SerializeField] private TextMeshProUGUI volumeText, lookText;
        [SerializeField] private Button backButton, saveButton;

        [Header("Min|Max")] 
        [SerializeField] private float volumeMax;
        [SerializeField] private float lookMax;

        private void Start()
        {
            if (!PlayerPrefs.HasKey("Volume"))
                PlayerPrefs.SetFloat("Volume", 1f);
        
            if (!PlayerPrefs.HasKey("Look"))
                PlayerPrefs.SetFloat("Look", 1f);


            volumeSlider.value = PlayerPrefs.GetFloat("Volume") / volumeMax;
            lookSlider.value = PlayerPrefs.GetFloat("Look") / lookMax;
        
            backButton.onClick.AddListener(OnBackPressed);
            saveButton.onClick.AddListener(OnSaveClicked);
        }

        private void Update()
        {
            volumeText.text = $"{Math.Round(volumeSlider.value * volumeMax, 1)}";
            lookText.text = $"{Math.Round(lookSlider.value * lookMax, 1)}";
        }

        private void OnSaveClicked()
        {
            PlayerPrefs.SetFloat("Volume", volumeSlider.value * volumeMax);
            PlayerPrefs.SetFloat("Look", lookSlider.value * lookMax);
        }

        private void OnBackPressed()
        {
            MenuManager.Instance.SetCurrentMenuState(MenuManager.MenuState.Main);
        }
    }
}
