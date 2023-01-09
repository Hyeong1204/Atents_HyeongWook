using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public float alphaChangeSpeed = 2.0f;           // 1초에 2까지 변경되는 속도 (0.5초면 1이 된다.)
    CanvasGroup canvas;

    TextMeshProUGUI totalPlayTImeText;
    Button reStartButton;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        totalPlayTImeText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        reStartButton = GetComponentInChildren<Button>();
        reStartButton.onClick.AddListener(OnReStartClick);
    }

    void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onDie += OnPlayerDie;

        canvas.alpha = 0.0f;
        canvas.blocksRaycasts = false;
    }

    private void OnPlayerDie(float totalPlayTime)
    {
        StartCoroutine(StartAlphaChange());

        totalPlayTImeText.text = $"Total Play Time\r\n<{Mathf.FloorToInt(totalPlayTime)} Sec>";
    }

    IEnumerator StartAlphaChange()
    {
        while (canvas.alpha < 1.0f)
        {
            canvas.alpha += Time.deltaTime * alphaChangeSpeed;

            yield return null;
        }

        canvas.alpha = 1.0f;
        canvas.blocksRaycasts = true;
    }

    void OnReStartClick()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
