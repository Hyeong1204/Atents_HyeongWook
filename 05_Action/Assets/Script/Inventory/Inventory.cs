using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리의 정보만 갖지는 클래스
public class Inventory
{
    // 기본 인벤토리 칸 수
    public const int Default_Inventory_Size = 6;

    ItemSlot[] slots = null;      // 이 이벤토리가 가지고 있는 아이템 슬롯의 배열

    ItemDataManager dataManager;  // 게임 메니저가 가지는 아이템 데이터 매니저 캐싱용

    public int SlotCount => slots.Length;

    public Inventory(int size = Default_Inventory_Size)
    {
        Debug.Log("인벤토리 생성");
        slots = new ItemSlot[size];
        for (int i = 0; i < size; i++)
        {
            slots[i] = new ItemSlot((uint)i);
        }
        dataManager = Gamemanager.Inst.ItemData;
    }

    /// <summary>
    /// 아이템을 인벤토리에 1개 추가하는 함수
    /// </summary>
    /// <param name="data">추가될 아이템 코드</param>
    /// <returns>성공 여부(true면 성공 false면 실패)</returns>
    public bool AddItem(ItemIDCode code)
    {
        return AddItem(dataManager[code]);
    }

    /// <summary>
    /// 아이템을 인벤토리에 1개 추가하는 함수
    /// </summary>
    /// <param name="data">추가될 아이템</param>
    /// <returns>성공 여부(true면 성공 false면 실패)</returns>
    public bool AddItem(ItemData data)          // 아이템 추가
    {
        bool result = false;

        // 같은 종류의 아이템이 있는가?
        // 있으면 -> 갯수 증가
        // 없으면 -> 새 슬롯에 아이템 넣기
        ItemSlot target = FindSameItem(data);

        if (target != null)
        {
            // 같은 종류의 아이템이 있다.
            target.IncreaseSlotItem();
            result = true;
        }
        else
        {
            ItemSlot emptySlot = FindEmptySlot();
            if (emptySlot != null)
            {
                // 비어있는 슬롯을 찾았다.
                emptySlot.AssignSlotItem(data);

                result = true;
            }
            else
            {
                // 인벤토리가 가득 찼다.
                Debug.Log("인벤토리가 가득참");
            }
        }

        return result;
    }

    public bool ReamoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        bool result = false;

        if (IsValidSlotIndex(slotIndex))
        {
            ItemSlot slot = slots[slotIndex];
            slot.DeCreaseSlotItem(decreaseCount);
            result = true;
        }
        else
        {
            Debug.Log($"실패 : {slotIndex}는 잘못된 인덱스입니다.");
        }

        return result;
    }

    /// <summary>
    /// 특정 슬롯에서 아이템을 제거하는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 제거할 슬롯 인덱스</param>
    /// <returns>true면 성공, false면 실패</returns>
    public bool ClearItem(uint slotIndex) // 아이템 버리기
    {
        bool result = false;

        if (IsValidSlotIndex(slotIndex))
        {
            ItemSlot slot = slots[slotIndex];
            slot.ClearSlotItem();
            result = true;
        }
        else
        {
            Debug.Log($"실패 : {slotIndex}는 잘못된 인덱스입니다.");
        }

        return result;
    }

    /// <summary>
    /// 비어 있는 슬롯을 찾는 함수
    /// </summary>
    /// <returns>비어있는 함수를 찾으면 null이 아니고 비어있는 함수가 없으면 null</returns>
    private ItemSlot FindEmptySlot()
    {
        ItemSlot result = null;
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                result = slot;
                break;
            }
        }
        return result;
    }
    
    /// <summary>
    /// 인벤토리에 파라메터와 같은 종류의 아이템이 있는지 찾아보는 함수
    /// </summary>
    /// <param name="itemdata">찾을 아이템</param>
    /// <returns>찾았으면 null 아니값(찾는 아이템이 들어있는 슬롯), 찾지 못 했으면 null</returns>
    private ItemSlot FindSameItem(ItemData itemdata)
    {
        ItemSlot findSlot = null;

        for (int i = 0; i < SlotCount; i++)
        {
            if (slots[i].ItemData == itemdata)
            {
                findSlot = slots[i];
                // Debug.Log($"{i}번째의 {slots[i].ItemData.itemName}가 있어 {itemdata.itemName}의 갯수를 증가시킵니다.");
                break;
            }
        }

        return findSlot;
    }

    /// <summary>
    /// 파라메터로 받은 인덱스가 적절한 인덱스인지 판단하는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 사용가능한 인덱스, false면 사용불가능한 인덱스</returns>
    private bool IsValidSlotIndex(uint index) => (index < SlotCount);

    public void PrintInventory()        // 인벤토리에 무엇이 들었는지 출력하기
    {
        // 출력 예시 : [ 루비(1), 에메랄드(1), 사파이어(1), 루비(1), (빈칸), (빈칸) ]
        string printText = "[";

        for (int i = 0; i < SlotCount; i++)
        {
            if(i == SlotCount - 1)
            {
                if (slots[i].ItemData != null)
                {
                    printText += $" {slots[i].ItemData.itemName} ({slots[i].ItemCount})";
                }
                else
                {
                    printText += " (빈칸)";
                }
                break;
            }

            if (slots[i].ItemData != null)
            {
                printText += $" {slots[i].ItemData.itemName} ({slots[i].ItemCount}),";
            }
            else
            {
                printText += " (빈칸),";
            }
        }

        printText += " ]";

        Debug.Log(printText);
    }
}
