using UnityEngine;

interface IEquipItem
{
    // 이 장비아이템이 장착될 파츠
    EquipPartType EquipPart { get; }

    /// <summary>
    /// 아이테 장비하기
    /// </summary>
    /// <param name="target">아이템을 장비할 대상</param>
    void EquipItem(GameObject target);

    /// <summary>
    /// 아이템을 해제하기
    /// </summary>
    /// <param name="target">아이템을 하제할 대상</param>
    void UnEquipItem(GameObject target);

    /// <summary>
    /// 아이템이 장뵈어있으면 해제하고 해제되었으면 장비하는 함수
    /// </summary>
    /// <param name="target">장비하고 해제할 대상</param>
    void ToggleEquipItem(GameObject target);
}