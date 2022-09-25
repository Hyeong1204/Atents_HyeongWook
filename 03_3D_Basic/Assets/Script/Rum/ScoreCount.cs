using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCount : MonoBehaviour
{
    float scoreTime = 0.0f;
    int m = 0;
    int s = 0;

    bool TimeStop = true;
    TextMeshProUGUI text;

    private void Awake()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Gool gool = FindObjectOfType<Gool>();
        gool.ScoreTime += Score;
    }

    private void Update()
    {
        if (TimeStop)
        {
        scoreTime += Time.deltaTime;
        TimeSetting();
        
        text.text = $"{m.ToString("D2")} : {s.ToString("D2")}";
        }
    }

    void Score(bool gool)
    {
        if (gool)
        {
            TimeStop = false;
        }
    }

    void TimeSetting()
    {
        s = (int)scoreTime % 60;
        m = (int)scoreTime / 60;
    }
}
