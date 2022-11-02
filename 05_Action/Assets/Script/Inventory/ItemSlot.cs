using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리 한칸의 정보를 나타내는 클래스
public class ItemSlot
{
    uint slotIndex;                   // 슬롯의 인덱스(인벤토리의 몇 번째 슬로인가?)
    uint itemCount = 0;                // 슬롯의 들어있는 아이템 개수
    ItemData slotItemData = null;     // 슬롯의 들어있는 아이템 (null이면 아이템이 없다)

    /// <summary>
    /// 이 슬롯의 인덱스
    /// </summary>
    public uint SlotIndex => slotIndex;

    /// <summary>
    /// 이 슬롯이 비어있는지 여부(ture면 비었고, false 무엇인가 들어있다.)
    /// </summary>
    public bool IsEmpty => slotItemData == null;

    /// <summary>
    /// 이 슬롯에 들어있는 아이템 데이터
    /// </summary>
    public ItemData ItemData => slotItemData;

    /// <summary>
    /// 아 슬롯에 들어있는 아이템 갯수
    /// </summary>
    public uint ItemCount => itemCount;

    public ItemSlot(uint index)
    {
        slotIndex = index;
    }

    /// <summary>
    /// 이 슬롯에 지정된 아이템을 지정된 갯수로 넣는 함수
    /// </summary>
    /// <param name="data">추가할 아이템</param>
    /// <param name="count">설정된 갯수</param>
    public void AssignSlotItem(ItemData data, uint count = 1)
    {
        itemCount = count;
        slotItemData = data;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{slotItemData.itemName}\" 아이템 {ItemCount}개 설정");
    }

    /// <summary>
    /// 이 슬롯에서 아이템을 제거하는 함수
    /// </summary>
    public void ClearSlotItem()
    {
        slotItemData = null;
        itemCount = 0;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯을 비웁니다.");
    }

    /// <summary>
    /// 이 슬롯에 아이템 갯수를 증가시키는 함수
    /// </summary>
    /// <param name="count">증가 시킬 아이템 갯수</param>
    public void IncreaseSlotItem(uint count = 1)
    {
        itemCount += count;
        Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{slotItemData.itemName}\" 아이템 {count}개 만큼 증가. 현재 {itemCount}개");
    }

    /// <summary>
    /// 이 슬롯에 아이템 갯수를 감소 시키는 함수
    /// </summary>
    /// <param name="count">감소시킬 아이템 갯수</param>
    public void DeCreaseSlotItem(uint count = 1)
    {
        int newCount = (int)itemCount - (int)count;

        if (newCount < 1)
        {
            ClearSlotItem();
            Debug.Log($"인벤토리 {slotIndex}번 슬롯을 비웁니다.");
        }
        else
        {
            itemCount = (uint)newCount;
            Debug.Log($"인벤토리 {slotIndex}번 슬롯에 \"{slotItemData.itemName}\" 아이템 {count}개 만큼 감소. 현재 {itemCount}개");
        }
    }
}