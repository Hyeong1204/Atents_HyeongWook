using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GridMap
{
    /// <summary>
    /// 이 맵에 있는 전체 노드들의 배열
    /// </summary>
    Node[] nodes;

    /// <summary>
    /// 맵의 가로 크기
    /// </summary>
    int width;

    /// <summary>
    /// 맵의 세로 크기
    /// </summary>
    int height;

    /// <summary>
    /// 원점의 그리즈 좌표(맨 왼쪽 아래 끝 부분의 그리드 좌표)
    /// </summary>
    Vector2Int origin;

    /// <summary>
    /// 배경용 타일맵
    /// </summary>
    Tilemap background;

    /// <summary>
    /// 이 맵에서 이동 가능한 지점의 배열(이동 간으한 모든 위치)
    /// </summary>
    Vector2Int[] movablePositions;

    /// <summary>
    /// 그리드맵을 만들기 위한 생성자
    /// </summary>
    /// <param name="width">생성할 맵의 가로 크기</param>
    /// <param name="height">생성할 맵의 세로 크기</param>
    public GridMap(int width, int height)
    {
        this.width = width;                 // 가로 세로 길이 기록
        this.height = height;

        nodes = new Node[height * width];   // 노드 배열 생성

        List<Vector2Int> movable = new List<Vector2Int>(width * height);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(x, y);
                nodes[index] = new Node(x, y);  // 노드 전부 
                movable.Add(new Vector2Int(x,y));
            }
        }

        movablePositions = movable.ToArray();               // 이동 가능한 위치 기록
    }

    /// <summary>
    /// 그리드 맵을 Tilemap을 사용해 생성하는 생성자
    /// </summary>
    /// <param name="backgound">그리맵의 전체 크기를 결정할 타일맵</param>
    /// <param name="obstacle">그리드맵에서 벽으로 설정될 타일을 가지는 타일맵(벽 위치 결정)</param>
    public GridMap(Tilemap backgound, Tilemap obstacle)
    {
        // backgound 크기를 기반으로 nodes 생성하기
        this.width = backgound.size.x;          // background의 크기 받아와서 가로 세로 길이로 사용
        this.height = backgound.size.y;

        nodes = new Node[height * width];       // 전체 노드가 들어갈 배열 생성

        // 새로 생성하는 Node의 x, y 좌표는 타일맵에서의 좌표와 같아야 한다.
        origin = (Vector2Int)backgound.origin;  // 타일맵에 기록된 원점 저장
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = GridToIndex(origin.x + x, origin.y + y);
                nodes[index] = new Node(origin.x + x, origin.y + y);            // 노드 전부 생성해서 배열의 넣기
                //TileBase tile = obstacle.GetTile(new Vector3Int(origin.x + x, origin.y + y));

                //if (tile != null)
                //{
                //    nodes[index].gridType = Node.GridType.Wall;
                //}
            }
        }

        // 갈 수 없는 지역 표시(obstacle에 타일이 있는 부분은 Wall로 표시)
        List<Vector2Int> movable = new List<Vector2Int>(width * height);
        for (int y = backgound.cellBounds.yMin; y < backgound.cellBounds.yMax; y++)
        {
            for (int x = backgound.cellBounds.xMin; x < backgound.cellBounds.xMax; x++)
            {
                // background 영역위에 있는 obstacle만 확인
                TileBase tile = obstacle.GetTile(new(x, y));
                if (tile != null)                                // 타일이 있으면 벽지역이다.
                {
                    Node node = GetNode(x, y);
                    node.gridType = Node.GridType.Wall;         // 벽으로 표시
                }
                else
                {
                    movable.Add(new Vector2Int(x, y));          // 이동 가능한 위치 기록
                }
            }
        }
        movablePositions = movable.ToArray();                   // 이동 가능한 위치기록을 배열로 변경

        // 배경만 기록
        this.background = backgound;
    }

    /// <summary>
    /// 그리드 맵에서 특정 그리드 좌표에 존재하는 노드 찾는 함수
    /// </summary>
    /// <param name="x">타일맵 기준 x 좌표</param>
    /// <param name="y">타일맵 기준 y 좌표</param>
    /// <returns>찾은 노드(없으면 null)</returns>
    public Node GetNode(int x, int y)
    {
        if (IsValidPostion(x, y))
        {
            return nodes[GridToIndex(x, y)];
        }

        return null;
    }

    /// <summary>
    /// 그리드 맵에서 특정 그리드 좌표에 존재하는 노드 찾는 함수
    /// </summary>
    /// <param name="pos">타일맵 기준으로 한 좌표</param>
    /// <returns></returns>
    public Node GetNode(Vector2Int pos)
    {
        return GetNode(pos.x, pos.y);
    }

    /// <summary>
    /// 맵이 가지는 모든 노드들의 A* 데이터 초기화
    /// </summary>
    public void ClearAstarDate()
    {
        foreach (var node in nodes)
        {
            node.ClearAStartData();
        }
    }

    /// <summary>
    /// 입력 받은 좌표가 맵 내부인지 확인하는 함수
    /// </summary>
    /// <param name="x">확인할 위치의 x</param>
    /// <param name="y">확인할 위치의 y</param>
    /// <returns>맵 안이면 true, 아니면 false</returns>
    public bool IsValidPostion(int x, int y)
    {
        return x >= origin.x && y >= origin.y && x < (origin.x + width) && y < (origin.y + height);
    }

    /// <summary>
    /// 입력 받은 좌표가 맵 내부인지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치의 좌표</param>
    /// <returns>맵 안이면 true, 아니면 false</returns>
    public bool IsValidPostion(Vector2Int pos)
    {
        return IsValidPostion(pos.x, pos.y);
    }

    /// <summary>
    /// 해당 위치가 벽인지 아닌지 확이하는 함수
    /// </summary>
    /// <param name="x">확인할 위치의 x</param>
    /// <param name="y">확인할 위치의 y</param>
    /// <returns>벽이면 true, 아니면 false</returns>
    public bool IsWall(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.gridType == Node.GridType.Wall;
    }

    /// <summary>
    /// 해당 위치가 벽인지 아닌지 확이하는 함수
    /// </summary>
    /// <param name="pos">확인할 위치의 좌표</param>
    /// <returns>벽이면 true, 아니면 false</returns>
    public bool IsWall(Vector2Int pos)
    {
        return IsWall(pos.x, pos.y);
    }

    /// <summary>
    /// 해당 위치가 스폰 가능한 지역인지 확인하는 함수
    /// </summary>
    /// <param name="x">확인할 x좌표</param>
    /// <param name="y">확인할 y 좌표</param>
    /// <returns>true면 스폰 가능. false면 불가능</returns>
    public bool IsSpawnable(int x, int y)
    {
        Node node = GetNode(x, y);
        return node != null && node.gridType == Node.GridType.Plain;
    }

    /// <summary>
    /// 해당 위치가 스폰 가능한 지역인지 확인하는 함수
    /// </summary>
    /// <param name="pos">확인할 그리드 좌표</param>
    /// <returns>true면 스폰 가능. false면 불가능</returns>
    public bool IsSpawnable(Vector2Int pos)
    {
        return IsSpawnable(pos.x, pos.y);
    }

    /// <summary>
    /// Grid좌표를 index로 변경하기 위한 함수. (GetNode에서 사용하기 위한 함수.)
    /// </summary>
    /// <param name="x">그리드 좌표 x</param>
    /// <param name="y">그리드 좌표 y</param>
    /// <returns>그리드 좌표가 변경된 인덱스 값(nodes의 특정 노드를 얻기 위한 인덱스)</returns>
    private int GridToIndex(int x, int y)
    {
        // -9, -5가 되었을 때 162이 나와야 한다.
        // 8, 4가 되었을 때 17이 나온다.
        // (x, y) = x + y * 가로길이;                   // 원점이 왼쪽위에 있을 때
        // (x, y) = x + ((높이 - 1) - y) * 가로길이     // 원점이 왼족아래에 있을 때
        return (x - origin.x) + ((height - 1) - y + origin.y) * width;          // 왼쪽 아래가 (0,0)이고 x+는 오른쪽, y+는 위쪽이기 떄문에 이렇게 변환
    }

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="pos">월드 좌표</param>
    /// <returns>변환된 그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 pos)
    {
        if (background != null)
        {
            return (Vector2Int)background.WorldToCell(pos);
        }
        else
        {
            return new Vector2Int((int)pos.x, Mathf.FloorToInt(pos.y));
        }
    }

    /// <summary>
    /// 그리드 좌표를 월드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="gridPos">그리드 좌표</param>
    /// <returns>월드 좌표</returns>
    public Vector2 GridToWorld(Vector2Int gridPos)
    {
        if (background != null)
        {
            return background.CellToWorld((Vector3Int)gridPos) + new Vector3(0.5f, 0.5f);
        }
        else
        {
            return new Vector2(gridPos.x + 0.5f, gridPos.y + 0.5f);
        }
    }

    /// <summary>
    /// 매ㅐㅂ에서 이동 가능한 랜덤한 지점을 하나 골라 리턴하는 함수
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetRandomMoveable()
    {
        int index = UnityEngine.Random.Range(0, movablePositions.Length);       // 미리 계산해 놓은 movablePositions 중에서 하나 고르기

        return movablePositions[index];
    }
}
