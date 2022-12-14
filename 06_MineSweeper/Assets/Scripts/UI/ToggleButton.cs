using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    /// <summary>
    /// 토글 버튼이 켜졌을 때 보일 이미지
    /// </summary>
    public Sprite onSprite;

    /// <summary>
    /// 토글 버튼이 꺼졌을 때 보일 이미지
    /// </summary>
    public Sprite offSprite;

    /// <summary>
    /// 토글 버튼이 상태가 변경될 대 실행될 델리게이트
    /// 파라메터 : true면 켜진 상태로 변경되었다. false면 꺼진 상태로 변경되었다.
    /// </summary>
    public Action<bool> onToggleChange;

    /// <summary>
    /// 토클 버튼의 현재 상태
    /// </summary>
    bool isOn = false;

    Image buttonImage;
    Button toggleButton;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        toggleButton = GetComponent<Button>();
        toggleButton.onClick.AddListener(ToggleClick);
    }

    /// <summary>
    /// 현재 상태와는 다른 상태로 변경하는 함수
    /// </summary>
    private void ToggleClick()
    {
        isOn = true;                // 토글을 켠상태로 만들기
        SetToggleState(!isOn);      // 버튼 켜기
    }

    /// <summary>
    /// 지정된 상태로 버튼의 상태로 변경하는 함수
    /// </summary>
    /// <param name="on">true면 켠다. false면 끈다.</param>
    public void SetToggleState(bool on)
    {
        // 변경할 상태에 따라 이미지 변경
        if(on) 
        {
            buttonImage.sprite = onSprite;
        }
        else
        {
            buttonImage.sprite = offSprite;
        }
        isOn = on;                          // 상태 데이터 변경
        onToggleChange?.Invoke(isOn);       // 델리게이트로 알림
    }
}
