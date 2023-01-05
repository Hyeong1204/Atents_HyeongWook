using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Seamless : TestBase
{
    public int x = 0;
    public int y = 0;

    Player player;
    MapManager mapManagr;

    private void Start()
    {
        mapManagr = GameManager.Inst.MapManager;
        player = GameManager.Inst.Player;
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        mapManagr.Test_LoadScene(x, y);
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        mapManagr.Test_LoadUnScene(x, y);
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        Debug.Log(mapManagr.WorldToGrid(player.transform.position));
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                mapManagr.Test_LoadScene(y, x);
            }
        }
    }

    protected override void Test5(InputAction.CallbackContext _)
    {
        Vector2Int grid = mapManagr.WorldToGrid(player.transform.position);
        mapManagr.Test_RefreshScenes(grid.x, grid.y);
    }
}
