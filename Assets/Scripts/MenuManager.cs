using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject SelectCharMenu;
    void Awake()
    {
        //GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }
    void OnDestroy()
    {
        //GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }
    //private void GameManagerOnOnGameStateChanged(GameState state)
    //{
    //    SelectCharMenu.SetActive(state == GameState.SelectCharacter);
    //}

    void Start()
    {

    }


    void Update()
    {

    }

    public void SelectCharacter(string character)
    {
        SelectCharMenu.SetActive(false);
        GameManager.Instance.UpdateGameState(GameManager.GameState.Countdown);
    }
}
