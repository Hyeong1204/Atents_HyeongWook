using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerInventory : Test_Base
{
    Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        player.MP = 30.0f;   
    }

    protected override void Test1(InputAction.CallbackContext _)
    {

    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        player.MP = 120.0f;
    }
}
