using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                cellObj.name = $"Cell_{cell.ID}_[{i}, {j}]";            // 셀의 이름 변경
                cells[cell.ID] = cell;                                  // 셀 배열에 셀 저장
            }
        }

        // 만들어진 셀에 지뢰를 minCount만큼 설치하기
        for (int i = 0; i < minCount; i++)
        {
            int rand = Random.Range(0, cells.Length);
            if (!cells[rand].HasMine)
            {
                cells[rand].SetMine();
                Debug.Log($"{cells[rand].ID}번째에 지뢰 새성");
            }
            else
            {
                i--;
            }
        }
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
}
