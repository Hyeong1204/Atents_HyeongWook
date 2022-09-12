using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreUi : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    TextMeshProUGUI scoreText2P;

    private void Awake()
    {
        scoreText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        scoreText2P = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Killzone killzone = GameObject.FindObjectOfType<Killzone>();
        killzone.onPlayerScore += SetPlayerScore;
        killzone.onPlayerScore2p += SetPlayerScore2p;
    }

    void SetPlayerScore2p(int score)
    {
        scoreText2P.text = score.ToString();
    }

    void SetPlayerScore(int score)
    {
        scoreText.text = score.ToString();
    }

}
