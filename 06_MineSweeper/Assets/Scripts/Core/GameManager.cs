#define TEST_CODE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // 게임 상태 관련 ------------------------------------------------------------
    enum GameState
    {
        Ready = 0,      // 게임 시작전(첫번째 셀이 안열린 상황)
        Play,           // 게임 진행 중
        GameClear,      // 모든 지뢰를 찾았을 때
        GameOver        // 지뢰가 있는 셀을 열었을 때
    }
    
    GameState state = GameState.Ready;

    /// <summary>
    /// 게임 진행중인지 확인하는 프로퍼티
    /// </summary>
    public bool IsPlaying => state == GameState.Play;

    /// <summary>
    /// Play 상태로 들어갓을 때 실행될 델리게이트
    /// </summary>
    public Action onGameStart;

    /// <summary>
    /// 게임이 재시작될 때 실행될 델리게이트(리셋버튼 눌렀을 때 실행. 보드 초기화 될 때. Ready 상태로 변경)
    /// </summary>
    public Action onGameReset;

    public Action onGameClear;

    public Action onGameOver;

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

    public Action<int> onFlagCountChange;

    protected override void Initialize()
    {
        base.Initialize();

        FlagCount = minCount;

        board = FindObjectOfType<Board>();
        board.Initialize(boardWidth, boardHeight, minCount);
    }


    public void IncreaseFlagCount()
    {
        FlagCount++;
    }

    public void DecreaseFlagCount()
    {
        FlagCount--;
    }

    public void GameStart()
    {
        if (state == GameState.Ready)
        {
            state = GameState.Play;
            onGameStart?.Invoke();
            Debug.Log("Play 상태");
        }
    }

    public void GameReset()
    {
        state = GameState.Ready;
        FlagCount = minCount;
        onGameReset?.Invoke();
        Debug.Log("Ready 상태");
    }

    public void GameClear()
    {
        state = GameState.GameClear;
        onGameClear?.Invoke();
        Debug.Log("Clear 상태");
    }

    public void GameOver()
    {
        state = GameState.GameOver;
        onGameOver?.Invoke();
        Debug.Log("GameOver 상태");
    }

#if TEST_CODE
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
