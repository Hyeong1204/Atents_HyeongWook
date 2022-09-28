using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Action onGameStart;

    Player player;
    ResultPanel resultPanel;
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
        isGameStart = false;
        timer = FindObjectOfType<Timer>();
        player = FindObjectOfType<Player>();
        resultPanel = FindObjectOfType<ResultPanel>();
        resultPanel?.gameObject.SetActive(false);       // resultPanel이 널이 아니면 실행
    }

    public void GameStart()
    {
        if (!isGameStart)
        {
            IsGameStasrt = true;
        }
    }

    public void ShowResultPanel()
    {
        if (resultPanel != null)
        {
            resultPanel?.gameObject.SetActive(true);
            resultPanel.ClearTime = timer.ResultTime;
        }
    }
}
