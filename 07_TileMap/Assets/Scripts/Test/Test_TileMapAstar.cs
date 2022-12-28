using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_TileMapAstar : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;

    public LineRenderer lineRenderer;
    public Transform start;
    public Transform goal;


    GridMap map;
    List<Vector2Int> path;

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.left.performed += Test_LeftClick;
        inputActions.Test.right.performed += Test_RightClick;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        inputActions.Test.right.performed -= Test_RightClick;
        inputActions.Test.left.performed -= Test_LeftClick;
    }

    void Start()
    {
        //background.size.x;          // 타일 맵의 가로 크기
        //background.size.y;          // 타일맵의 세로 크기
        path = new List<Vector2Int>();
        map = new GridMap(background, obstacle);
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        path = AStar.PathFind(map, map.WorldToGrid(start.transform.position), map.WorldToGrid(goal.transform.position));
        string pathstring = "Path : ";
        foreach (var node in path)
        {
            pathstring += $"( {node.x}, {node.y} ) ->";
        }
        pathstring += " 끝";

        Debug.Log(pathstring);

        lineRenderer.positionCount = path.Count;
        int index = 0;
        foreach (var node in path)
        {
            Vector2 wolrdPos = map.GridToWorld(node);
            lineRenderer.SetPosition(index, new(wolrdPos.x - lineRenderer.transform.position.x, wolrdPos.y - lineRenderer.transform.position.y, 1));
            index++;
        }
    }

    private void Test_RightClick(InputAction.CallbackContext _)
    {
        // 시작 지점 옮기기
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);

        if (!map.IsWall(gridPos))
        {
            Vector2 finalPos = map.GridToWorld(gridPos);
            goal.transform.position = finalPos;
        }
    }

    private void Test_LeftClick(InputAction.CallbackContext _)
    {
        // 도착 지점 옮기기
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);

        if (!map.IsWall(gridPos))
        {
            Vector2 finalPos = map.GridToWorld(gridPos);
            start.transform.position = finalPos;
        }
    }
}
