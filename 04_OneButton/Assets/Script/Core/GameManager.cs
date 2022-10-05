using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public ImageNumber socreUI;
    PipeRotator pipeRotator;
    Bird player;

    int score = 0;
    public Bird Player => player;

    private int Score
    {
        get => score;
        set
        {
            score = value;
            socreUI.maxNumber = score;
        }
    }

    protected override void Initaialize()
    {
        player = FindObjectOfType<Bird>();
        pipeRotator = FindObjectOfType<PipeRotator>();
        pipeRotator.SetPipeScoreDelegate(AddScore);
    }

    void AddScore(int point)
    {
        Score += point;
        
    }

    public void TestSetScore(int newScore)
    {
        Score = newScore;
    }
}
