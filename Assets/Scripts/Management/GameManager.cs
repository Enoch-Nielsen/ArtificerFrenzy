using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public enum GameStatus
    {
        Active, Paused, GameOver
    }

    public static GameManager Instance;

    [field: Header("Game Status")]
    [field: SerializeField] public GameStatus CurrentGameStatus { get; private set; }
    public List<DialogPopup> currentDialogPopups;
    [SerializeField] private bool tooltipsActive;

    [Header("Look Options")]
    public float baseLookSensitivity;
    [Range(0.01f, 10f)] public float lookMultiplier;
    
    private void Awake()
    {
        Instance = this;

        UpdateInGameSettings();

        Application.targetFrameRate = 240;
        QualitySettings.vSyncCount = 1;
    }

    private void Update()
    {
        Time.timeScale = CurrentGameStatus is GameStatus.Paused or GameStatus.GameOver ? 0f : 1f;
    }

    /// <summary>
    /// Sets the status of the game to the target.
    /// </summary>
    /// <param name="targetStatus"> The target status enum. </param>
    public void SetGameStatus(GameStatus targetStatus)
    {
        CurrentGameStatus = targetStatus;
    }

    /// <summary>
    /// Toggles all tooltips.
    /// </summary>
    public void ToggleTooltips()
    {
        if (tooltipsActive)
            DeactivateTooltips();
        else
            ActivateTooltips();
    }

    /// <summary>
    /// Activates all current tooltips.
    /// </summary>
    public void ActivateTooltips()
    {
        foreach (var popup in currentDialogPopups)
        {
            popup.Activate();
        }

        tooltipsActive = true;
    }

    /// <summary>
    /// Deactivates all tooltips.
    /// </summary>
    public void DeactivateTooltips()
    {
        foreach (var popup in currentDialogPopups)
        {
            popup.DeActivate();
        }

        tooltipsActive = false;
    }

    /// <summary>
    /// Adds a new tooltip to the list.
    /// </summary>
    /// <param name="dialog"></param>
    public void AddTooltip(DialogPopup dialog)
    {
        if (!currentDialogPopups.Contains(dialog))
            currentDialogPopups.Add(dialog);
        
        if (tooltipsActive)
            dialog.Activate();
    }

    /// <summary>
    /// Removes a tooltip from the list.
    /// </summary>
    /// <param name="dialog"></param>
    public void RemoveTooltip(DialogPopup dialog)
    {
        if (!currentDialogPopups.Contains(dialog))
            return;
        
        currentDialogPopups.Remove(dialog);
        
        dialog.DeActivate();
        dialog.Remove();
    }

    /// <summary>
    /// Updates the settings from the settings menu to apply in game.
    /// </summary>
    public void UpdateInGameSettings()
    {
        if (PlayerPrefs.HasKey("Look"))
            lookMultiplier = PlayerPrefs.GetFloat("Look");
    }

    /// <summary>
    /// Exits the game, either closing the application or exiting play mode.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
        EditorApplication.ExitPlaymode();
    }
}
