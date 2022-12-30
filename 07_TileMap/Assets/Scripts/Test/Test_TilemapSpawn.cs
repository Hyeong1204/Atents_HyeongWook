using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TilemapSpawn : TestBase
{
    SceneMonsterManager manager;

    protected override void Awake()
    {
        base.Awake();
        manager = FindObjectOfType<SceneMonsterManager>();
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        List<Slime> list = new List<Slime>(manager.SpawnedList);
        foreach (var slime in list)
        {
            slime.Die();
        }
    }
}
