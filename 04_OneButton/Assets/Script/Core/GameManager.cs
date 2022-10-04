using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public ImageNumber socreUI;
    PipeRotator pipeRotator;

    int score = 0;

    //public int Score
    //{
    //    get => score;
    //    set
    //    {
    //        score = value;
    //    }
    //}

    protected override void Initaialize()
    {
        pipeRotator = FindObjectOfType<PipeRotator>();
        pipeRotator.SetPipeScoreDelegate(AddScore);
    }

    void AddScore(int point)
    {
        score += point;
        socreUI.maxNumber = score;
    }
}
