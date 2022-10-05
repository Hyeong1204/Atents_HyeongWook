using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    ResultPanel reusltPanel;
    Button nextButton;
    Button rankButton;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        reusltPanel = GetComponentInChildren<ResultPanel>();
        nextButton = transform.GetChild(2).GetComponent<Button>();
        rankButton = transform.GetChild(3).GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();

        nextButton.onClick.AddListener(onClick_Next);
        rankButton.onClick.AddListener(onClick_Rank);
    }

    private void Start()
    {
        Close();
        GameManager.Inst.Player.onDead += Open;
    }

    void onClick_Next()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   // 현재 열린 씬을 새로 열기
    }

    void onClick_Rank()
    {

    }

    private void Open()
    {
        reusltPanel.RefreshScore();
        StartCoroutine(OpenDelay());
    }

    void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    IEnumerator OpenDelay()
    {
        yield return new WaitForSeconds(2.0f);
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
