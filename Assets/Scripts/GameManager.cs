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
    }

    private void Update()
    {
        Time.timeScale = CurrentGameStatus is GameStatus.Paused or GameStatus.GameOver ? 0f : 1f;
    }

    public void SetGameStatus(GameStatus targetStatus)
    {
        CurrentGameStatus = targetStatus;
    }

    public void UpdateInGameSettings()
    {
        if (PlayerPrefs.HasKey("Look"))
            lookMultiplier = PlayerPrefs.GetFloat("Look");
    }

    public void ExitGame()
    {
        Application.Quit();
        EditorApplication.ExitPlaymode();
    }
}
