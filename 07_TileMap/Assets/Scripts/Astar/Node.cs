using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>
{
    // 축 기죽 : 왼쪽 아래가 원점(0,0)
    // 축 방향 : 오른쪽으로 갈 수록 x+, 위로 갈 수록 y+

    /// <summary>
    /// 그리드 맵에서 x좌표
    /// </summary>
    public int x;

    /// <summary>
    /// 그리도 맵에서 y좌표
    /// </summary>
    public int y;

    public enum GridType
    {
        Plain = 0,      // 바닥
        Wall,           // 벽
        Monster         // 몬스터
    }

    /// <summary>
    /// 이 노드의 종류
    /// </summary>
    public GridType gridType = GridType.Plain;

    /// <summary>
    /// A* 알고리즘의 G값(시작점에서 이 노드까지 오는데 걸린 실제 거리)
    /// </summary>
    public float G;

    /// <summary>
    /// A* 알고리즘의 H값(이 노드에서 도착점까지의 예상 거리)
    /// </summary>
    public float H;

    /// <summary>
    /// A* 알고리즘으로 이 노드를 지나서 시작점에서 도착점까지 이동할 때의 거리(반쯤 예상 거리)
    /// </summary>
    public float F => G + H;

    /// <summary>
    /// 이 노드의 앞 노드
    /// </summary>
    public Node parent;

    /// <summary>
    /// Node 생성자
    /// </summary>
    /// <param name="x">위치 x좌표</param>
    /// <param name="y">위치 y좌표</param>
    /// <param name="gridType">노드의 종류. 기본적으로 평지</param>
    public Node(int x, int y, GridType gridType = GridType.Plain)
    {
        this.x = x;
        this.y = y;
        this.gridType = gridType;

        G = float.MaxValue;
        H = float.MaxValue;
        parent = null;
    }

    /// <summary>
    /// A* 세이터 초기화. 다시 길 찾기를 할 때 초기화 용도
    /// </summary>
    public void ClearAStartData()
    {
        G = float.MaxValue;
        H = float.MaxValue;
        parent = null;
    }

    /// <summary>
    /// 순서 비교용으로 IComparable를 상속받을 경우 구현해야 하는 함수
    /// </summary>
    /// <param name="other">비교 대상</param>
    /// <returns></returns>
    public int CompareTo(Node other)
    {
        // 리턴이 0보다 작다 : this < other
        // 리턴이 0 : this, other가 같다
        // 리턴이 0보다 크다: this > other
        Node node = other;
        if (other == null)
        {
            return 1;
        }

        //if(this.F < node.F)
        //{
        //    return -1;
        //}
        //else if(this.F > node.F)
        //{
        //    return 1;
        //}
        //else
        //{
        //    return 0;
        //}
        return this.F.CompareTo(node.F);
    }

    public override bool Equals(object obj)
    {
        return obj is Node other && this.x == other.x && this.y == other.y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    /// <summary>
    /// == 명령어 오러로딩(==와 !=은 반드시 쌍으로 구현으로 구현해야 한다)
    /// </summary>
    /// <param name="op1">연산자의 왼쪽에 있는 변수</param>
    /// <param name="op2">연산자의 오른쪽에 있는 변수</param>
    /// <returns></returns>
    public static bool operator == (Node op1, Vector2Int op2)
    {
        return op1.x == op2.x && op1.y == op2.y;
    }

    public static bool operator !=(Node op1, Vector2Int op2)
    {
        return op1.x != op2.x || op1.y != op2.y;
    }
}
