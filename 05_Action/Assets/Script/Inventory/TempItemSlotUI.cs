using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TempItemSlotUI : ItemSlotUI
{
    /// <summary>
    /// 임시 슬롯이 열리고 닫힘을 알리는 델리게이트 true면 열림. false면 닫힘
    /// </summary>
    public Action<bool> onTempSlotOpenClose;


    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();        // 매 프레임마다 마우스 위치로 이동
    }

    /// <summary>
    /// 슬롯 초기화 함수
    /// </summary>
    /// <param name="id">슬롯의 ID</param>
    /// <param name="slot">이 UI가 보여줄 ItemSlot</param>
    public override void InitializeSlot(uint id, ItemSlot slot)
    {
        onTempSlotOpenClose = null;                 // 델리게이트 초기화
        base.InitializeSlot(id, slot);
    }

    /// <summary>
    /// TempItemSlot를 여는 함수
    /// </summary>
    public void Open()
    {
        if (!ItemSlot.IsEmpty)                  // 아이템이 들어잇을 때만 열기
        {
            transform.position = Mouse.current.position.ReadValue();        // 열릴 때 마우스 위치로 이동
            onTempSlotOpenClose?.Invoke(true);  // 열렸다고 알림
            gameObject.SetActive(true);         // 활성화
        }
    }

    /// <summary>
    /// TempItemSlot를 닫는함수
    /// </summary>
    public void Close()
    {
        onTempSlotOpenClose?.Invoke(false);     // 닫혔다고 알림
        gameObject.SetActive(false);            // 비활성화
    }

    public void OnDrop(InputAction.CallbackContext _)
    {
        if (!ItemSlot.IsEmpty)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, LayerMask.GetMask("Ground")))
            {
                ItemFactory.MakeItems((int)ItemSlot.ItemData.id, (int)ItemSlot.ItemCount, hit.point, true);
                ItemSlot.ClearSlotItem();
                Close();
            }
        }
    }
}
