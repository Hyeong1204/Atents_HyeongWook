using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    ImageNumber imageNumber;
    Timer timer;

    private void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        timer = gameManager.GetComponent<Timer>();
        gameManager.onTimeCountChange += Refresh;
        gameManager.onGameStart += OnStart;
        gameManager.onGameClear += OnStop;
        gameManager.onGameOver += OnStop;
        gameManager.onGameReset += OnReset;
    }

    private void OnReset()
    {
        OnStop();
        imageNumber.Number = 0;
    }

    private void OnStop()
    {
        timer.Stop();
    }

    private void OnStart()
    {
        timer.Play();
    }

    private void Refresh(int count)
    {
        imageNumber.Number = count;
    }
}
