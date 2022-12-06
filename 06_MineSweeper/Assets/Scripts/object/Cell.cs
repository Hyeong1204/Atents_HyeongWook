using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cell : MonoBehaviour
{

    /// <summary>
    /// 닫힌 셀에 어떤 표시가 있는지 나타내는 enum
    /// </summary>
    enum CellMarkState
    {
        None =0,        // 아무것도 표시 안됨
        Flag,           // 깃발 설치됨
        Question        // 물음표(?) 표시됨
    }

    // 변수 =======================================================================================

    /// <summary>
    /// ID가 잘못 되었다고 알려주는 상수
    /// </summary>
    public const int ID_NOT_VALID = -1;

    /// <summary>
    /// 셀의 ID이면서 위치를표시하는 역할
    /// </summary>
    public int id = ID_NOT_VALID;          // 셀의 아이디이면서 위치를 표시하는 역할

    /// <summary>
    /// 셀이 열려있는지 닫혀있는지 여부. true면 열려있고 false면 닫혀있다.
    /// </summary>
    bool isOpen = false;            // 셀이 닫혀있는지 여부

    /// <summary>
    /// 이 셀에 지뢰가 있는지 없는지 여부. true면 지뢰가 있다. false면 지뢰가 없다.
    /// </summary>
    bool hasMine = false;            // 지뢰가 있는지 여부

    /// <summary>
    /// 이 셀이 닫혀있을 때 표시 되고 있는 것
    /// </summary>
    CellMarkState markState = CellMarkState.None;       // 닫힌 셀에 표시된 것

    /// <summary>
    /// 주변 셀의 지뢰 갯수. 셀이 열렸을 때 표시할 이미지 결정
    /// </summary>
    int aroundMineCount = 0;        // 주변 셀의 지뢰 갯수

    /// <summary>
    /// 이 셀이 들어있는 보드
    /// </summary>
    Board parentBoard;

    /// <summary>
    /// 닫혔을 때 보일 스프라이트 렌더러
    /// </summary>
    SpriteRenderer cover;

    /// <summary>
    /// 열렸을 때 보일 스프라잍트 렌더러
    /// </summary>
    SpriteRenderer inside;

    /// <summary>
    /// 이 셀에 의해 눌러진 셀의 목록(자기자신 or 자기 주변에 닫혀 있던 셀)
    /// </summary>
    List<Cell> pressedCells;

    // 프로퍼티 ===========================================================================

    /// <summary>
    /// ID 확인 및 설정용 프로퍼티 (설정은 한번만 가능)
    /// </summary>
    public int ID
    {
        get => id;
        set
        {
            if(id == ID_NOT_VALID)      // ID는 처음 한번만 설정가능
            {
                id = value;
            }
        }
    }

    /// <summary>
    /// 이 셀이 소속되어있는 보드 확인 및 설정용 프로퍼티(설정은 한번만 가능)
    /// </summary>
    public Board Board
    {
        get => parentBoard;
        set
        {
            if(parentBoard == null)
            {
                parentBoard = value;
            }
        }
    }

    /// <summary>
    /// 셀이 열렸는지 닫혔는지 확인하는 프로퍼티
    /// </summary>
    public bool IsOpen => isOpen;

    /// <summary>
    /// 셀이 지뢰가 있는지 없는지 확인하는 프로퍼티
    /// </summary>
    public bool HasMine => hasMine;

    /// <summary>
    /// 셀에 깃발이 표시되어있는지 확인하는 프로퍼티
    /// </summary>
    public bool IsFlaged => markState == CellMarkState.Flag;

    /// <summary>
    /// 셀에 물음표가(?) 표시되어있는지 확인하는 프로퍼티
    /// </summary>
    public bool IsQuestion => markState == CellMarkState.Question;

    private void Awake()
    {
        pressedCells = new List<Cell>(8);                               // 새로 메모리 할당

        cover = transform.GetChild(0).GetComponent<SpriteRenderer>();
        inside = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"{ID}들어옴");
        if(Mouse.current.leftButton.ReadValue() > 0)
        {
            PressCover();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log($"{ID}나감");
        if (Mouse.current.leftButton.ReadValue() > 0)
        {
            RestoreCover();
        }
    }

    // 델리게이트 =================================================================

    public Action onFlagUse;
    public Action onFlagReturn;

    // 함수 =======================================================================

    /// <summary>
    /// 셀을 여는 함수
    /// </summary>
    public void Open()
    {
        if (!isOpen && !IsFlaged)                                // 닫혀있고 깃발 표시가 안되었을 때만 연다.
        {
            isOpen = true;
            cover.gameObject.SetActive(false);      // 셀을 열릴 때 커버를 비활성화

            if (aroundMineCount == 0 && !HasMine)               // 주변 지뢰 갯수가 0이면
            {
                List<Cell> neighbors = Board.GetNeihtbors(this.ID);     // 주변 셀들을
                foreach (var cell in neighbors)
                {
                    cell.Open();                                        // 모두 연다
                }
            }
        }
    }

    /// <summary>
    /// 셀이 눌러졌을 때 실행될 함수
    /// </summary>
    public void CellPress()
    {
        pressedCells.Clear();           // 새롭게 눌렸으니 기존에 눌러져 있던 대한 기록을 제거
        if (IsOpen)
        {
            // 이 셀이 열려져 있으면, 자신 주변의 닫힌 셀을 모두 누른 표시를 해야한다.
            List<Cell> neighbors = Board.GetNeihtbors(this.ID);     // 주변 셀을 모두 가져온다.
            foreach (var cell in neighbors)
            {
                if (!cell.IsOpen)                       // 주변 셀중에 닫혀있는 셀만
                {
                    pressedCells.Add(cell);             // 누르고 있는 셀이라고 표시
                    cell.CellPress();                   // 누르고 있는 표시
                }
            }
        }
        else
        {
            // 이 셀이 닫힌 셀이 때 자신을 누른 표시를 한다.
            PressCover();
        }

    }

    /// <summary>
    /// 누른 셀을 땠을 때 실해될 함수
    /// </summary>
    public void CellRelease()
    {
        RestoreCovers();            // 눌렀다고 표시한 모든 셀을 복구 시키고
        Open();                     // 자신을 열기
    }

    void PressCover()
    {
        switch (markState)
        {
            case CellMarkState.None:
                cover.sprite = Board[CloseCellType.Close_Press];
                break;
            case CellMarkState.Question:
                cover.sprite = Board[CloseCellType.Question_Press];
                break;
            case CellMarkState.Flag:
            default:
                break;
        }
        pressedCells.Add(this);
    }

    void RestoreCovers()
    {
        foreach (var cell in pressedCells)
        {
            cell.RestoreCover();
        }
        pressedCells.Clear();
    }

    void RestoreCover()
    {
        switch (markState)
        {
            case CellMarkState.None:
                cover.sprite = Board[CloseCellType.Close];
                break;
            case CellMarkState.Question:
                cover.sprite = Board[CloseCellType.Question];
                break;
            case CellMarkState.Flag:
            default:
                break;
        }
    }

    /// <summary>
    /// 주변8칸에 지뢰가 추가될 때 실행되는 함수
    /// </summary>
    public void IncressAroundMineCount()
    {
        if (!hasMine)
        {
            aroundMineCount++;
            inside.sprite = Board[(OpenCellType)aroundMineCount];
        }
    }

    /// <summary>
    /// 이 셀에 지뢰를 추가하는 함수
    /// </summary>
    public void SetMine()
    {
        hasMine = true;             // 지뢰 설치 되었다고 표시
        inside.sprite = Board[OpenCellType.Mine_NotFound];      // 지뢰로 이미지 변경

        // 이 셀 주변 셀들의 IncressAroundMineCount함수 실행(aroundMineCount를 1씩 증가)
        List<Cell> cellList = Board.GetNeihtborsMy(ID);
        foreach (var cell in cellList)
        {
            cell.IncressAroundMineCount();
        }
    }

    /// <summary>
    /// 셀에 빈것 -> 깃발 -> 물음표 -> 빈것 -> ... 순서로 표시하는 함수
    /// </summary>
    public void CellRightPress()
    {
        if (!IsOpen)
        {
            switch (markState)
            {
                case CellMarkState.None:                            //
                    markState = CellMarkState.Flag;
                    cover.sprite = Board[CloseCellType.Flag];
                    onFlagUse?.Invoke();
                    break;
                case CellMarkState.Flag:                            //
                    markState = CellMarkState.Question;
                    cover.sprite = Board[CloseCellType.Question];
                    onFlagReturn?.Invoke();
                    break;
                case CellMarkState.Question:                        //
                    markState = CellMarkState.None;
                    cover.sprite = Board[CloseCellType.Close];
                    break;
                default:
                    break;
            }
        }
    }
}
