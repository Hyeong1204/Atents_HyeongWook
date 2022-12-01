#define TEST_CODE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 타이머 관련   -------------------------------------------------------------
    private Timer timer;
    private int timeCount = 0;

    // 깃발 갯수 관련 -------------------------------------------------------------
    private int flagCount = 0;

    // 지뢰 관련    ---------------------------------------------------------------
    public int minCount = 10;
    public int boardWidth = 8;
    public int boardHeight = 8;
    Board board;

    public Board Board => board;

    public int FlagCount
    {
        get => flagCount;
        private set
        {
            flagCount = value;
            onFlagCountChange?.Invoke(flagCount);
        }
    }

    public int TimeCount
    {
        get => timeCount;
        private set
        {
            if (timeCount != value)
            {
                timeCount = value;
                onTimeCountChange?.Invoke(timeCount);
            }
        }
    }

    public Action<int> onFlagCountChange;
    public Action<int> onTimeCountChange;

    protected override void Initialize()
    {
        base.Initialize();

        timer = GetComponent<Timer>();
        board = FindObjectOfType<Board>();
        board.Initialize(boardWidth, boardHeight, minCount);
    }

    private void Update()
    {
        TimeCount = (int)timer.ElapsedTime;
    }

#if TEST_CODE
    public void TestTimerPlay()
    {
        timer.Play();
    }

    public void TestTimerStop()
    {
        timer.Stop();
    }

    public void TestTimerReset()
    {
        timer.TimerReset();
    }

    public void TestFlag_Increase()
    {
        FlagCount++;
    }

    public void TestFlag_Decrease()
    {
        FlagCount--;
    }
#endif
}
