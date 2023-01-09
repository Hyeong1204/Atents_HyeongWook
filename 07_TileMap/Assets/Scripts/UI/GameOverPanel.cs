using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    CanvasGroup canvas;
    TextMeshProUGUI totalText;
    Button reStartButton;

    float currentAlpha = 0.0f;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        totalText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        reStartButton = GetComponentInChildren<Button>();
        reStartButton.onClick.AddListener(() =>
        {
            GameManager.Inst.initialized = false;
            SceneManager.LoadScene(0);
        });
    }

    void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onDie += PanelUpdate;

        canvas.alpha = 0.0f;
        canvas.blocksRaycasts = false;
        currentAlpha = 0.0f;
    }

    private void PanelUpdate(float totalTime)
    {
        StartCoroutine(OnPanel(totalTime));
    }

    IEnumerator OnPanel(float totalTime)
    {
        currentAlpha = 0.0f;
        totalText.text = $"Total Play Time\n<{(int)totalTime} Sec>";

        while (currentAlpha < 1.0f)
        {
            currentAlpha += Time.deltaTime;
            canvas.alpha = currentAlpha;

            yield return null;
        }

        canvas.alpha = 1.0f;
        canvas.blocksRaycasts = true;
    }
}
