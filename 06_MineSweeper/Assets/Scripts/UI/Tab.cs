using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    bool isSelected = true;
    Button tabButton;
    Image tabImage;
    readonly Color UselectedColor = new Color(1, 1, 1, 0.2f);
    Transform childPanel;

    public Action<Tab> onTabSelect;

    public bool IsSelected
    {
        get => isSelected;
        set
        {
            isSelected = value;
            TabSelect(isSelected);
        }
    }

    private void Awake()
    {
        tabButton = GetComponent<Button>();
        tabButton.onClick.AddListener(() =>
        {
            if (!IsSelected)
            {
                IsSelected = true;
            }
        });
        tabImage = GetComponent<Image>();
        childPanel = transform.GetChild(0);
        IsSelected = false;
    }

    private void Start()
    {
    }

    void TabSelect(bool selected)
    {
        if (selected)
        {
            // 선택 되었을 때 처리
            tabImage.color = Color.white;
            onTabSelect?.Invoke(this);
            ChildPanelOpen();
        }
        else
        {
            // 선택되지 않았을 때 처리
            tabImage.color = UselectedColor;
            ChildPanelClose();
        }
    }

    public void ChildPanelOpen()
    {
        if(IsSelected) 
        {
            childPanel.gameObject.SetActive(true);
        }
    }

    public void ChildPanelClose()
    {
        childPanel.gameObject.SetActive(false);
    }
}
