using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mana Potion", menuName = "Scriptable Object/Item Data - Mana Potion", order = 3)]
public class ItemData_ManaPotion : ItemData, IUsable
{
    [Header("마나 포션 데이터")]
    public float manaPoint = 10.0f;

    public bool Use(GameObject target = null)
    {
        bool result = false;
        IMana mana = target.GetComponent<IMana>();
        if(mana != null)
        {
            //mana.ManaRegenerate();

            result = true;
        }
        
        return result;
    }
}
