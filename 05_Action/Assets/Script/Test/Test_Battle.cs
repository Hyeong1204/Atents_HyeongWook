using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Battle : Test_Base
{
    Player player;

    private void Start()
    {
        player = Gamemanager.Inst.Player;
    }

    protected override void Test1(InputAction.CallbackContext obj)
    {
        player.Defence(60.0f);
    }

    protected override void Test2(InputAction.CallbackContext obj)
    {
        player.HP = player.MaxHP;
    }

    protected override void Test3(InputAction.CallbackContext obj)
    {
        GameObject Obj = ItemFactory.MakeItem(ItemIDCode.Ruby);
    }
}
