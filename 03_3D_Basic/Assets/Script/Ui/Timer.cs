using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI timeText;

    float currentTime = 0.0f;
    bool isStart = false;

    private void Awake()
    {
        timeText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    //{
    //    StartTimer();
    //}

    private void Start()
    {
        Goalll goal = FindObjectOfType<Goalll>();
        goal.onGoalIn += StopTimer;
        

        CurrentTime = 0.0f;
        GameManager.Inst.onGameStart += StartTimer;
    }

    float CurrentTime
    {
        get => currentTime;
        set
        {
            currentTime = value;
            timeText.text = $"{currentTime:f2}초";
        }
    }

    private void Update()
    {
        if (isStart)
        {
            CurrentTime += Time.deltaTime;
        }
    }

    public float ResultTime { get => currentTime; }

    void StartTimer()
    {
        isStart = true;
        currentTime = 0.0f;
    }

    void StopTimer()
    {
        isStart = false;
    }

   
}
