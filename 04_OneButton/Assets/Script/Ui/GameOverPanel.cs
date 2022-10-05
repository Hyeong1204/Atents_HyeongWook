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

    private void Awake()
    {
        reusltPanel = GetComponentInChildren<ResultPanel>();
        nextButton = transform.GetChild(2).GetComponent<Button>();
        rankButton = transform.GetChild(3).GetComponent<Button>();

        nextButton.onClick.AddListener(onClick_Next);
        rankButton.onClick.AddListener(onClick_Rank);
    }

    private void Start()
    {
        gameObject.SetActive(false);
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
        gameObject.SetActive(true);
    }
}
