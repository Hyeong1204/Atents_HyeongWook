using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReStart : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        gameObject.SetActive(false);        // 패널 끄기
        GameManager.Inst.Dino.isDie += Open;
        button.onClick.AddListener(GameReStart);
    }

    private void OnDisable()
    {
        GameManager temp = GameManager.Inst;
        if(temp != null)
        {
            temp.Dino.isDie -= Open;
        }
    }

    void GameReStart()              // 재시작 버튼
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);       // 현재 신 다시 로드
    }

    void Open()
    {
        gameObject.SetActive(true);         // 다시 시작버튼 패널 열기
    }
}
