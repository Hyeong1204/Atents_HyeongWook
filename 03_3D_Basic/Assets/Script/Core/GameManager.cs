using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Action onGameStart;

    Player player;
    Timer timer;

    bool isGameStart = false;

    public Player Player { get => player; }

    public bool IsGameStasrt
    {
        get => isGameStart;
        private set
        {
            isGameStart = value;
            if (isGameStart)
            {
                onGameStart?.Invoke();
            }
        }
    }
    protected override void Initaialize()
    {
        player = FindObjectOfType<Player>();
        timer = FindObjectOfType<Timer>();
    }

    public void GameStart()
    {
        if (!isGameStart)
        {
            IsGameStasrt = true;
        }
    }
}
