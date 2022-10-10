using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUi : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    TextMeshProUGUI hiScoreText;

    public float scoreUpdateSpeed = 1.0f;           // 점수 증가 속도
    float score = 0;            // 현재 점수
    int hiScore = 0;

    bool updateScore = true;        // true면 점수 갱신

    float Score
    {
        get => score;
        set
        {
            score = value;

            scoreText.text = $"{(int)score}";
        }
    }

    private void Awake()
    {
        scoreText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        hiScoreText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GameManager.Inst.Dino.isDie += StopScore;
        hiScore = GameManager.Inst.HiScore;
        hiScoreText.text = hiScore.ToString();
        updateScore = true;
    }

    private void Update()
    {
        if(updateScore)
        Score += Time.deltaTime * scoreUpdateSpeed;
    }

    void StopScore()
    {
        updateScore = false;
        GameManager.Inst.Score = (int)score;
    }
}
