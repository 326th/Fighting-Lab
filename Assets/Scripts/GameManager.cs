using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public static event Action<GameState> OnGameStateChanged;
    [SerializeField] GameObject selectCharacterMenu;
    [SerializeField] GameObject countdown;

    public GameState State;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.Countdown);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Countdown:
                HandleCountdown();
                break;
            case GameState.Play:
                break;
            case GameState.Endscreen:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void HandleCountdown()
    {
        countdown.SetActive(true);
    }


    void Update()
    {
        
    }

    public enum GameState
    {
        Countdown,
        Play,
        Endscreen
    }
}
