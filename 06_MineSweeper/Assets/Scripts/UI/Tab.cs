using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    /// <summary>
    /// 이 탭이 선택 되었는지 여부. true면 선택 되어있다. false면 석택되어 있지 않다.
    /// </summary>
    bool isSelected = true;

    /// <summary>
    /// 컴포넌트들
    /// </summary>
    Button tabButton;       // 탭에 달린 버튼
    Image tabImage;         // 탭 버튼의 이미지
    TabSubPanel subPanel;   // 탭의 자식 패널
    /// <summary>
    /// 선택되지 않았을 때의 색상 정보(투명하게 보이기)
    /// </summary>
    readonly Color UselectedColor = new Color(1, 1, 1, 0.2f);

    /// <summary>
    /// 이 탭이 선택 되었을 때 실행될 델리게이트
    /// </summary>
    public Action<Tab> onTabSelect;

    /// <summary>
    /// 선택 상태가 변경 되었을 때의 처리와 상태 확인용 프로퍼티
    /// </summary>
    public bool IsSelected
    {
        get => isSelected;
        set
        {
            isSelected = value;
            if (isSelected)         // 탭 선택 여부에 따라 색상 변경 및 처리
            {
                // 선택 되었을 때 처리
                tabImage.color = Color.white;       // 색상 변경
                onTabSelect?.Invoke(this);          // 선택 되었다는 알리 처리
                ChildPanelOpen();                   // 자식 패널 열기
            }
            else
            {
                // 선택되지 않았을 때 처리
                tabImage.color = UselectedColor;    // 색상 변경(투명하게)
                ChildPanelClose();                  // 자식 패널 닫기
            }
        }
    }

    private void Awake()
    {
        subPanel = transform.GetComponentInChildren<TabSubPanel>();
        tabButton = GetComponent<Button>();
        tabButton.onClick.AddListener(() =>
        {
            IsSelected = true;                  // 클릭하면 IsSelected 프로퍼티를 true로 변경
        });
        tabImage = GetComponent<Image>();

        IsSelected = false;                     // 기본적으로는 선택 되어있지 않은 것으로 처리        
    }

    /// <summary>
    /// 자식 패널 여는 함수
    /// </summary>
    public void ChildPanelOpen()
    {
        if(IsSelected) 
        {
            subPanel.gameObject.SetActive(true);      // 선택되었을 때만 열기
        }
    }

    /// <summary>
    /// 자식 패널 닫는 함수
    /// </summary>
    public void ChildPanelClose()
    {
        subPanel.gameObject.SetActive(false);         // 자식 패널 닫기
    }
}
