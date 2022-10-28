using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템을 생성만하는 클래스. 팩토리 디자인 패턴
/// </summary>
public class ItemFactory
{
    static int itemCount = 0;   // 생성된 아이템 총 갯수. 아이템 생성 아이디의 역할도 함.

    public static GameObject MakeItem(ItemIDCode code)
    {
        GameObject obj = new GameObject();

        Item item = obj.AddComponent<Item>();               // Item 컴포넌트추가하기
        item.data = Gamemanager.Inst.ItemData[code];

        string[] itemName = item.data.name.Split("_");      // 00_Name
        obj.name = $"{itemName[1]}_{itemCount++}";          // 오브젝트 이름 설정하기
        obj.layer = LayerMask.NameToLayer("Item");          // 레이어 설정

        SphereCollider sc = obj.AddComponent<SphereCollider>();     // 컬라이더 추가
        sc.isTrigger = true;                                        // 컬라이더 트리커 켜기
        sc.radius = 0.5f;                                           // 컬라이더 반지름 0.5로 설정
        sc.center = Vector3.up;                                     // 컬라이더 위치 변경

        return obj;
    }

    public static GameObject MakeItem(uint id)
    {
        return MakeItem((ItemIDCode)id);
    }
}
