using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // ItemSlotUI가 있는 프리팹. 인벤토리 크키 변화에 대비해서 가지고 있기
    public GameObject slotPrefab;

    /// <summary>
    /// 이 UI가 보여줄 이벤토리
    /// </summary>
    Inventory inven;

    ItemSlotUI[] slotUIs;
    TempItemSlotUI tempSlot;

    private void Awake()
    {
        Transform slotParent = transform.GetChild(0);               // 가져오기 용도
        slotUIs = new ItemSlotUI[slotParent.childCount];
        for (int i = 0; i < slotParent.childCount; i++)
        {
            Transform child = slotParent.GetChild(i);
            slotUIs[i] = child.GetComponent<ItemSlotUI>();
        }
        tempSlot = GetComponentInChildren<TempItemSlotUI>();
    }

    /// <summary>
    /// 입력받은 인벤토리에 맞게 각종 초기화 작업을하는 함수
    /// </summary>
    /// <param name="playerInven">이 UI로 표시할 인벤토리</param>
    public void InitializeInventoty(Inventory playerInven)
    {
        inven = playerInven;

        Transform slotParent = transform.GetChild(0);               // 가져오기 용도
        GridLayoutGroup grid = slotParent.GetComponent<GridLayoutGroup>();

        if ( Inventory.Default_Inventory_Size != inven.SlotCount)       // 인벤토리 쿠키가 기본과 다른 때의 처리
        {
            // 기본 사이즈와 다르면 기존 슬롯을 전부 삭제하고 새로 만들기
            foreach (var slot in slotUIs)
            {
                Destroy(slot.gameObject);       // 기본적으로 가지고 있던 슬롯 모두 삭제
            }


            // 인벤토리 크기에 따라 ItemSlotUI의 크기 변경
            RectTransform rectParent = (RectTransform)slotParent;
            float totalarea = rectParent.rect.width * rectParent.rect.height;       // slotParent의 전체 면적 계산
            float slotArea = totalarea / inven.SlotCount;                           // slot하나의 면적 구하기

            float slotSideLength = Mathf.Floor(Mathf.Sqrt(slotArea)) - grid.spacing.x;      // spacing 크기 고려해서 Slot 한변의 길이 구하기
            grid.cellSize = new Vector2(slotSideLength, slotSideLength);                    // 계산 결과 적용

            // 슬롯 새롭게 생성
            slotUIs = new ItemSlotUI[inven.SlotCount];          // 슬롯 배열을 새 크기에 맞게 새로 생성
            for (uint i = 0; i < inven.SlotCount; i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);       // 슬롯을 하나씩 새성
                obj.name = $"{slotParent.name}_{i}";                        // 슬롯 이름이 안겹치게 변경
                slotUIs[i] = obj.GetComponent<ItemSlotUI>();                // 슬롯을 배열에 저장
            }
        }

        // 공통 처리부분
        for (uint i = 0; i < inven.SlotCount; i++)
        {
            slotUIs[i].InitializeSlot(i, inven[i]);                     // 각 슬롯 초기화
            slotUIs[i].Resize(grid.cellSize.x * 0.75f);                 // 슬롯 크기에 맞게 내부 크기 리사이즈
            slotUIs[i].onDragStart += OnItemDragStart;                  // 슬롯에서 드래그시작될 때 실행될 함수 연결
            slotUIs[i].onDragEnd += OnItemDragEnd;                      // 슬롯에서 드래그가 끝날 떄 실행될 함수 연결
            slotUIs[i].onDragCanel += OnItemDragEnd;                  // 드래그가 실패했을 때 실행될 함수 연결
        }

        // 임시 슬롯 초기화 처리
        tempSlot.InitializeSlot(Inventory.TempSlotIndex, inven.TempSlot);   // 임시 슬롯 초기화
        tempSlot.Close();           // 기본적으로 닫아 놓기
    }

    /// <summary>
    /// 슬롯에 드래그를 시작햇을 때 실행될 함수
    /// </summary>
    /// <param name="slotID">드래그가 시작된 슬롯의 ID</param>
    private void OnItemDragStart(uint slotID)
    {
        inven.MoveItem(slotID, Inventory.TempSlotIndex);    // 슬롯에 있는 아이템들을 임시 슬롯으로 모두 옮김
        tempSlot.Open();                                    // 임시 슬롯을 보여주기
    }

    /// <summary>
    /// 드래그가 슬롯에서 끝날을 때나 실패 했을 때 실행될 함수
    /// </summary>
    /// <param name="slotID">드래그가 끝난 슬롯의 ID</param>
    private void OnItemDragEnd(uint slotID)
    {
        inven.MoveItem(Inventory.TempSlotIndex, slotID);    // 임시 슬롯의 아이템들을 슬롯에 모두 옮김
        if (tempSlot.ItemSlot.IsEmpty)
        {
            tempSlot.Close();                                   // 임시 슬롯을 안뵝게 만들기
        }
    }
}
