using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public static event Action<GameState> OnGameStateChanged;
    [SerializeField] GameObject countdown;
    [SerializeField] Character_Base Player;
    [SerializeField] Character_Base AI;
    [SerializeField] GameObject endGameScreen;
    //private EndgameScreenHandler endgameScreenHandler;

    public GameState State;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //endgameScreenHandler = endGameScreen.GetComponent<EndgameScreenHandler>();
        UpdateGameState(GameState.Countdown);
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        print("gameState = " + newState);

        switch (newState)
        {
            case GameState.Countdown:
                HandleCountdown();
                break;
            case GameState.Play:
                HandlePlay();
                break;
            case GameState.Endscreen:
                HandleEndscreen();
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
    public void HandlePlay()
    {
        MatchTimeController.Instance.StartDecreaseMatchTime();
    }
    public void HandleEndscreen()
    {
        endGameScreen.SetActive(true);
        Player.SavePlayer(MatchTimeController.Instance.playTime);
        endGameScreen.GetComponent<EndgameScreenHandler>().UpdateText();
        
    }

    void Update()
    {
        if(State == GameState.Play)
        {
            HandleMatch();
        }
        
    }

    private void HandleMatch()
    {
        if (MatchTimeController.Instance.matchTime == 0)
        {
            print("time out");
            UpdateGameState(GameState.Endscreen);
        }
        else if (AI.hitPoints < 0)
        {
            print("player wins");
            //MatchTimeController.Instance.StopDecreaseMatchTime();
            UpdateGameState(GameState.Endscreen);
        }
        else if (Player.hitPoints < 0)
        {
            print("AI wins");
            //MatchTimeController.Instance.StopDecreaseMatchTime();
            UpdateGameState(GameState.Endscreen);
        }
    }
    public enum GameState
    {
        Countdown,
        Play,
        Endscreen
    }
}
