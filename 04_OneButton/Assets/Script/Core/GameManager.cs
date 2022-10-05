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
    public Bird Player => player;

    public int Score
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
        socreUI = GameObject.FindGameObjectWithTag("Score").GetComponent<ImageNumber>();
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
