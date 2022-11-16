using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item Data", menuName = "Scriptable Object/Item Data - Weapon", order = 5)]
public class ItemData_Weapon : ItemData_EquipItem
{
    [Header("무기 데이터")]
    public float attacPower = 30;

    public new EquipPartType EquipPart => EquipPartType.Weapon;
}
