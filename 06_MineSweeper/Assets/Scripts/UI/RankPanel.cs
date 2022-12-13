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
    Tab selectedTab;
    ToggleButton toggle;

    private void Awake()
    {
        tabs = GetComponentsInChildren<Tab>();

        foreach (var tab in tabs)
        {
            tab.onTabSelect += (newSelectedTab) =>
            {
                if (newSelectedTab != selectedTab)           // 서로 다를 텝일 때만 변경
                {
                    selectedTab.IsSelected = false;     // 이전 탭 끄기
                    selectedTab = newSelectedTab;
                    selectedTab.IsSelected = true;      // 새 탭 열기
                }
            };
        }

        toggle = GetComponentInChildren<ToggleButton>();
        toggle.onToggleChange += (isOn) =>
        {
            if (isOn && selectedTab != null)            // 토글 버튼이 켜지고 선택된 탭이 있을 때
            {
                selectedTab.ChildPanelOpen();           // 선택된 탭을 연다
            }
            else
            {
                foreach (var tab in tabs)
                {
                    tab.ChildPanelClose();              // 토글 버튼이 꺼지면 모든 탭을 닫는다.
                }
            }
        };
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onGameClear += Open;
        gameManager.onGameOver += Open;
        gameManager.onGameReset += Close;

        selectedTab = tabs[0];
        selectedTab.IsSelected = true;

        Close();
    }

    void Open()
    {
        this.gameObject.SetActive(true);
    }

    void Close()
    {
        this.gameObject.SetActive(false);
    }
}
