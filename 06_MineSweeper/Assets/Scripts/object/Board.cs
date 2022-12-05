using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 생설할 셀의 프리팹
    /// </summary>
    public GameObject cellprefab;

    /// <summary>
    /// 보드가 가지는 가로 셀의 길이. (가로 줄의 셀 개수)
    /// </summary>
    int width = 16;

    /// <summary>
    /// 보드가 가지는 세로 셀의 길이. (세로 줄의 셀 개수)
    /// </summary>
    int height = 16;

    /// <summary>
    /// 셀 한 번의 길이(셀은 정사각형)
    /// </summary>
    const float Distance = 1.0f;                    // 1일 때 카메라 크기 9.

    /// <summary>
    /// 이 보드가 가지는 모든 셀
    /// </summary>
    Cell[] cells;

    /// <summary>
    /// 열린 셀에서 표시될 이미지
    /// </summary>
    public Sprite[] openCellImages;

    /// <summary>
    /// OpenCellType으로 이미지를 받아오는 인덱서
    /// </summary>
    /// <param name="type">필요한 이미지에 enum타입</param>
    /// <returns>enum타입에 맞는 이미지</returns>
    public Sprite this[OpenCellType type] => openCellImages[(int)type];

    PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.RightClick.performed += OnRightClick;
        inputActions.Player.LeftClick.performed += OnLeftClick;
    }

    private void OnDisable()
    {
        inputActions.Player.LeftClick.performed -= OnLeftClick;
        inputActions.Player.RightClick.performed -= OnRightClick;
        inputActions.Player.Disable();
    }


    /// <summary>
    /// 이 보드가 가질 모든 셀을 생성
    /// </summary>
    public void Initialize(int newWidth, int newHeigh, int minCount)
    {
        ClearCells();           // 기존에 존재하던 셀 다지우기

        width = newWidth;
        height = newHeigh;

        Vector3 basePos = transform.position;           // 기준 위치 설정(보드의 위치)
        Vector3 offset = new Vector3(-(width-1) * Distance * 0.5f, (height-1) * Distance * 0.5f);   // 보드의 피벗을 중심으로 셀이 생성되게 하기 위해 셀이 생성될 시작점 계산용

        cells = new Cell[width * height];                               // 셀들의 배열 생성

        // 셀들을 하나씩 생성하기 위한 이중 for
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject cellObj = Instantiate(cellprefab, this.transform);       // 이 보드를 부모로 삼고 생성
                Cell cell = cellObj.GetComponent<Cell>();                           // 생성한 오브젝트에서 Cell 컴포넌트 찾기
                cellObj.transform.position = basePos + offset + new Vector3(j * Distance, -i * Distance, 0);  // 적절한 위치에 배치
                cell.ID = i * width + j;                                // ID 설정(ID를 통해 위치도 확인 가능)
                cell.Board = this;                                      // 보드 설정
                cellObj.name = $"Cell_{cell.ID}_[{i}, {j}]";            // 셀의 이름 변경
                cells[cell.ID] = cell;                                  // 셀 배열에 셀 저장
            }
        }

        // 만들어진 셀에 지뢰를 minCount만큼 설치하기 (피셔 예이츠 알고리즘 사용)
        int[] ids = new int[cells.Length];
        for (int i = 0; i < cells.Length; i++)
        {
            ids[i] = i;
        }
        Shuffle(ids);
        for (int i = 0; i < minCount; i++)
        {
            cells[ids[i]].SetMine();
        }
    }

    /// <summary>
    /// 파라메터로 받은 배열내부의 데이터 순서를 섞는 함수
    /// </summary>
    /// <param name="source"></param>
    public void Shuffle(int[] source)
    {
        int count = source.Length - 1;
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, source.Length - i);
            int lastIndex = count - i;
            (source[randomIndex], source[lastIndex]) = (source[lastIndex], source[randomIndex]);        // temp 변수 없시 스왑처리
        }
    }

    public List<Cell> GetNeihtbors(int id)
    {
        List<Cell> result = new List<Cell>(8);
        Vector2Int grid = IdToGrid(id);

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int index = GridToID(j + grid.x, i + grid.y);
                if((i == 0 && j == 0) && index != Cell.ID_NOT_VALID)
                {
                    result.Add(cells[index]);
                }
            }
        }

        return result;
    }

    public List<Cell> GetNeihtborsMy(int id)
    {
        List<Cell> result = new List<Cell>();
        int index = 0;

        Vector2Int dir = new Vector2Int(id % width, id / width);
        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                Vector2Int cellDir = new Vector2Int(dir.x + x, dir.y + y);
                if (cellDir.x > -1 && cellDir.x < width && cellDir.y > -1 && cellDir.y < height)
                {
                    index = cellDir.x + cellDir.y * width;

                    result.Add(cells[index]);
                }
            }
        }
        return result;
    }

    Vector2Int IdToGrid(int id)
    {
        return new Vector2Int(id % width, id / width);
    }

    int GridToID(int x, int y)
    {
        if (IsValidGrid(x,y))
         return x + y * width;

        return Cell.ID_NOT_VALID;
    }

    /// <summary>
    /// 보드의 모든 셀을 제거하는 함수
    /// </summary>
    public void ClearCells()
    {
        // 이미 만들어진셀 오브젝트를 모두 삭제하기
        if (cells != null)      // 기존에 만들어진 셀이 있으면
        {
            foreach (var cell in cells)
            {
                Destroy(cell.gameObject);
            }
        }
        cells = null;           // 안의 내용을 다 제거했다고 표시
    }

    private void OnLeftClick(InputAction.CallbackContext _)
    {
        // 왼쪽 클릭
    }

    private void OnRightClick(InputAction.CallbackContext _)
    {
        // 우클릭
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2Int grid = ScreenToGrid(screenPos);
        if (IsValidGrid(grid))
        {
            Cell target = cells[GridToID(grid.x, grid.y)];
        }
        else
        {

        }
    }

    //int ScreenToID()
    //{
    //    int id = -1;

    //    Vector2 screenPos = Mouse.current.position.ReadValue();
    //    Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
    //    worldPos.z = 0;

    //    int gridX = 0;
    //    int gridY = 0;
    //    if((int)worldPos.x > 0)
    //    {
    //        gridX = Mathf.CeilToInt(worldPos.x) + (width >> 1)-1;
    //    }
    //    else
    //    {
    //        gridX = Mathf.CeilToInt(worldPos.x) + (width >> 1)-1;
    //    }
    //    if((int)worldPos.y > 0)
    //    {
    //        gridY = (Mathf.CeilToInt(worldPos.y) - (height >> 1)) * -1;
    //    }
    //    else
    //    {
    //        gridY = (Mathf.CeilToInt(worldPos.y) - (height >> 1)) * -1;
    //    }

    //    id = GridToID(gridX, gridY);
        
    //    return id;
    //}

    /// <summary>
    /// 입력받은 스크린 좌표가 몇 번째 그리드에 있는지 알려주는 함수
    /// </summary>
    /// <param name="screenPos">확인할 스크린 좌표</param>
    /// <returns>스크린좌표와 매칭되는 보드 위의 그리드 좌표</returns>
    Vector2Int ScreenToGrid(Vector2 screenPos)
    {
        // 보드의 왼쪽 위 (시작 좌표) 구하기
        Vector2 startPos = new Vector3(-width * Distance * 0.5f, height * Distance * 0.5f) + transform.position;

        // 보드의 왼쪽 위에서 마우스가 얼마만큼 떨어져 있는지 확인
        Vector2 diff = (Vector2)Camera.main.ScreenToWorldPoint(screenPos) - startPos;

        // Distance로 나누어서 Grid좌표로 변환
        return new((int)(diff.x / Distance), (int)(diff.y / Distance));
    }

    int ScreenToID(Vector2 screenpos)
    {
        Vector2Int grid = ScreenToGrid(screenpos);
        return GridToID(grid.x, grid.y);
    }

    bool IsValidGrid(int x, int  y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    bool IsValidGrid(Vector2Int grid)
    {
        return IsValidGrid(grid.x, grid.y);
    }
}
