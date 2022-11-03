using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 인벤토리의 정보만 갖지는 클래스
public class Inventory
{
    // 상수 ------------------------------------------------------------------------------------
    // 기본 인벤토리 칸 수
    public const int Default_Inventory_Size = 6;
    public const uint TempSlotIndex = 9999999;              // 어떤 숫자든 상관없다. slots의 인덱스가 될수 없는 값만 아니면 된다.

    // 변수 ------------------------------------------------------------------------------------

    ItemSlot[] slots = null;      // 이 이벤토리가 가지고 있는 아이템 슬롯의 배열
    ItemSlot tempSlot = null;

    ItemDataManager dataManager;  // 게임 메니저가 가지는 아이템 데이터 매니저 캐싱용


    // 프로퍼티 --------------------------------------------------------------------------------

    public int SlotCount => slots.Length;

    public ItemSlot TempSlot => tempSlot;

    // 함수 ------------------------------------------------------------------------------------

    public Inventory(int size = Default_Inventory_Size)
    {
        Debug.Log("인벤토리 생성");
        slots = new ItemSlot[size];
        for (int i = 0; i < size; i++)
        {
            slots[i] = new ItemSlot((uint)i);
        }
        tempSlot = new ItemSlot(TempSlotIndex);
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
                Debug.Log("실패 : 인벤토리가 가득참");
            }
        }

        return result;
    }

    /// <summary>
    /// 아이템을 인벤토리의 특정 슬롯에 1개 추가하는 함수
    /// </summary>
    /// <param name="code">추가할 아이템 코드</param>
    /// <param name="index">아이템이 추가될 슬롯의 인덱스</param>
    /// <returns>true면 성공, false면 실패</returns>
    public bool AddItem(ItemIDCode code, uint index)
    {
        return AddItem(dataManager[code], index);
    }

    /// <summary>
    /// 아이템을 인벤토리의 특정 슬롯에 1개 추가하는 함수
    /// </summary>
    /// <param name="data">추가할 아이템 데이터</param>
    /// <param name="index">아이템이 추가될 슬롯의 인덱스</param>
    /// <returns>true면 성공, false면 실패</returns>
    public bool AddItem(ItemData data, uint index)
    {
        bool result = false;

        if (IsValidSlotIndex(index))        // 인덱스가 적절한가.
        {
            ItemSlot slot = slots[index];

            if (slot.IsEmpty)               // 해당 슬롯에 아이템이 있는가?
            {
                slot.AssignSlotItem(data);
                result = true;
            }
            else
            {
                if(slot.ItemData == data)   // 같은 종류의 아이템이 있는가?
                {
                    // 같은 종류의 아이템이 들어왔다.
                    slot.IncreaseSlotItem();
                    result = true;
                }
                else
                {
                    // 다른 종류의 아이템이 들어있다.
                    Debug.Log($"실패 : 인벤토리 {index}번 슬롯에 다른 아이템이 들어있습닏나.");
                }
            }
        }
        else
        {
            Debug.Log($"실패 : {index}는 잘못된 인덱스입니다.");
        }

        return result;
    }

    /// <summary>
    /// 아이템을 인벤토리 슬롯에서 특정 갯수만큼 제거하는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 제거할 슬롯의 인덱스</param>
    /// <param name="decreaseCount">제거할 갯수(기본적으로 1)</param>
    /// <returns>성공이면 true. 실패면 false</returns>
    public bool RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        bool result = false;

        if (IsValidSlotIndex(slotIndex))
        {
            ItemSlot slot = slots[slotIndex];           // 해당 슬롯에
            slot.DeCreaseSlotItem(decreaseCount);       // decreaseCount만큼 갯수 감소
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
