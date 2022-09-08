using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score_Panel : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

    }

    private void Start()
    {
        Player player = FindObjectOfType<Player>();
        player.onScoreChange += RefreshScore;
    }

    private void RefreshScore(int totalScore)
    {
        //socreText.text = totalScore.ToString();
        scoreText.text = $"{totalScore,4}";
    }
}
