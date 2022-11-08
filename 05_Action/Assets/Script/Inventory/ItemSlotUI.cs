using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerClickHandler ,IPointerUpHandler
{
    // 변수  ------------------------------------------------------------------------------------------------
    private uint id;        // 몇번째 슬롯인가?

    protected ItemSlot itemSlot; // 이 UI와 연결된 ItemSlot

    Image itemImage;
    TextMeshProUGUI itemCountText;

    // 프로퍼티 ----------------------------------------------------------------------------------------------
    public uint ID => id;

    public ItemSlot ItemSlot => itemSlot;

    // 델리게이트  --------------------------------------------------------------------------------------------
    public Action<uint> onDragStart;
    public Action<uint> onMouseUp;

    // 함수 --------------------------------------------------------------------------------------------------
    private void Awake()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemCountText = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// 슬롯 초기화 함수
    /// </summary>
    /// <param name="id">슬롯의 ID</param>
    /// <param name="slot">이 UI가 보여줄 ItemSlot</param>
    public void InitializeSlot(uint id, ItemSlot slot)
    {
        this.id = id;
        this.itemSlot = slot;
        this.itemSlot.onSlotItemChange = Refresh;

        Refresh();
    }

    /// <summary>
    /// 자식 게임 오브젝트들의 크기 변경
    /// </summary>
    /// <param name="iconSize">아이콘 한변의 크기</param>
    public void Resize(float iconSize)
    {
        RectTransform rectTransform = (RectTransform)itemImage.gameObject.transform;
        rectTransform.sizeDelta = new Vector2(iconSize, iconSize);
    }

    /// <summary>
    /// 슬롯에 보이는 모습 갱신용도의 함수. itemSlot의 내부 데디어터가 변경될 때마다 실행.
    /// </summary>
    void Refresh()
    {
        if( itemSlot.IsEmpty)
        {
            // 슬롯에 아이템이 비었으면
            itemImage.sprite = null;                // 스프라이트 빼기
            itemImage.color = Color.clear;          // 투명화 (스프라이트 빼면 흰색 네모만 나옴)
            itemCountText.text = null;               // 갯수 비우기
        }
        else
        {
            itemImage.sprite = itemSlot.ItemData.itemIcon;      // 해당 아이콘 이미지 표시
            itemImage.color = Color.white;                      // 불투명화
            itemCountText.text = $"{ItemSlot.ItemCount}";       // 아이템 갯수 설정
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // OnBeginDrag, OnEndDrag를  실해시키기 위해 추가

        //eventData.position : 마우스 포인터의 스크린 좌표값
        //eventData.delta : 마우스 포인터의 위치 변화량
        //eventData.button == PointerEventData.InputButton.Left : 마우스 왼쪽 버튼이 눌러져 있다.
        //eventData.button == PointerEventData.InputButton.Right : 마우스 오른쪽 버튼이 눌러져 있다.
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"드래그 시작 : {ID}번 슬롯");
        onDragStart?.Invoke(ID);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // OnPointerUp를 실해시키 위해 추가
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"마우스 업 : {ID}번 슬롯");
        onMouseUp?.Invoke(ID);
    }
}
