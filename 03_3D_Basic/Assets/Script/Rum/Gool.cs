using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gool : MonoBehaviour
{
    public Action<bool> ScoreTime;

    bool targetGool = false;

    private void Start()
    {
        ScoreTime?.Invoke(targetGool);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetGool = true;
            ScoreTime?.Invoke(targetGool);
        }
    }
}
