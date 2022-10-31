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

    public bool AddItem(ItemIDCode code)
    {
        return AddItem(dataManager[code]);
    }

    public bool AddItem(ItemData data)          // 아이템 추가
    {
        bool result = false;

        ItemSlot emptySlot = FindEmptySlot();
        if(emptySlot != null)
        {
            // 비어있는 슬롯을 찾았다.
            emptySlot.AssignSlotItem(data);

            result = true;
            Debug.Log($"인벤토리 {emptySlot.SlotIndex}번 슬롯에 \"{data.name}\" 아이템 추가");
        }
        else
        {
            // 인벤토리가 가득 찼다.
            Debug.Log("인벤토리가 가득참");
        }

        return result;
    }

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
}
