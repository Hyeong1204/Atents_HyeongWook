using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_TlimemapAStarSlime : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;
    public Slime slime;
    GridMap map;
    public GridMap Map => map;

    protected override void Awake()
    {
        base.Awake();
        map = new GridMap(background, obstacle);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.left.performed += Test_LeftClick;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        inputActions.Test.left.performed -= Test_LeftClick;
    }

    private void Test_LeftClick(InputAction.CallbackContext obj)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);

        if (!map.IsWall(gridPos))
        {
            slime.SetDestination(gridPos);
        }
    }
}
