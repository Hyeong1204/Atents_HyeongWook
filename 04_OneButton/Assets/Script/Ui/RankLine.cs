using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankLine : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    TextMeshProUGUI nameText;

    private void Awake()
    {
        scoreText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetDate(int score, string name)
    {
        scoreText.text = score.ToString();
        nameText.text = name;
    }
}
