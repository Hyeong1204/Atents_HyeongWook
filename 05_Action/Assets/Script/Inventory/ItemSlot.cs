using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리 한칸의 정보를 나타내는 클래스
public class ItemSlot
{
    uint slotIndex;                   // 슬롯의 인덱스(인벤토리의 몇 번째 슬로인가?)
    uint itemCount = 0;                // 슬롯의 들어있는 아이템 개수
    ItemData slotItemData = null;     // 슬롯의 들어있는 아이템 (null이면 아이템이 없다)

    public uint SlotIndex => slotIndex;

    public bool IsEmpty => slotItemData == null;

    public ItemSlot(uint index)
    {
        slotIndex = index;
    }

    public void AssignSlotItem(ItemData data, uint count = 1)
    {
        itemCount = count;
        slotItemData = data;
    }
}
