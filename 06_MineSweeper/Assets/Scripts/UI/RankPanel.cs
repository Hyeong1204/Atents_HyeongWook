using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    // 과제 : 
    // 게임이 끝나면 패널 열리기
    // 열릴때 무조건 패널이 하나만 켜져야함
    // 버튼이 눌렸을 때 해당 패널이 열려야함

    Tab[] tabs;
    private void Awake()
    {
        tabs = GetComponentsInChildren<Tab>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onGameClear += Open;
        gameManager.onGameOver += Open;
        gameManager.onGameReset += Close;
        Close();
    }

    void Open()
    {
        this.gameObject.SetActive(true);
        if (!tabs[0].gameObject.activeSelf && !tabs[1].gameObject.activeSelf)
        {
            tabs[0].ChildPanelOpen();
        }
    }

    void Close()
    {
        this.gameObject.SetActive(false);
    }
}
