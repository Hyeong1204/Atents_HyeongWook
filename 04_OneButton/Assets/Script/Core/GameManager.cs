using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    ImageNumber socreUI;
    PipeRotator pipeRotator;
    Bird player;

    int score = 0;
    int bestScore = 0;
    public Bird Player => player;

    public int BesScore
    {
        get => bestScore;
        private set
        {
            bestScore = value;
        }
    }

    public int Score
    {
        get => score;
        private set
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
        socreUI = GameObject.FindGameObjectWithTag("Score").GetComponent<ImageNumber>();

        LoadGameDate();
    }

    void AddScore(int point)
    {
        Score += point;
        
    }

    public void TestSetScore(int newScore)
    {
        Score = newScore;
    }

    void SaveGameDate()
    {

    }

    void LoadGameDate()
    {

    }

    public void BestScoreUpdate()
    {
        if(bestScore < Score)
        {
            bestScore = Score;
            SaveGameDate();
        }
    }
}
