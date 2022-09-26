using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Action<bool> ScoreTime;

    bool targetGoal = false;

    private void Start()
    {
        ScoreTime?.Invoke(targetGoal);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetGoal = true;
            ScoreTime?.Invoke(targetGoal);
        }
    }
}
