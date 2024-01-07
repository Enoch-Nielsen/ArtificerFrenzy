using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Look Options")]
    public float baseLookSensitivity;
    [Range(0.01f, 10f)] public float lookMultiplier;
    
    private void Start()
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
