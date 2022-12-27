using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public static class AStar
{
    /// <summary>
    /// 길을 탐색하는 함수
    /// </summary>
    /// <param name="gridMap">길 찾기를 진행할 맵</param>
    /// <param name="start">시작 위치(그리드 좌표)</param>
    /// <param name="goal">도착위치(그리드 좌표)</param>
    /// <returns>시작위치에서 도착위치까지의 경로.(길 찾기에 실패할 경우 null)</returns>
    public static List<Vector2Int> PathFind(GridMap gridMap, Vector2Int start, Vector2Int goal)
    {
        gridMap.ClearAstarDate();               // 맵이 이전 길찾기를 하면서 가지고 있던 값들을 초기화
        List<Vector2Int> path = null;           // 최종 경로가 저장될 리스트 변수

        // 시작지점과 도착지점이 맵안에 있을 때만 길 찾기 시작
        if (gridMap.IsValidPostion(start) && gridMap.IsValidPostion(goal))
        {
            List<Node> open = new List<Node>();         // A*용 오픈 리스트
            List<Node> close = new List<Node>();        // A*용 클로즈 리스트

            Node current = gridMap.GetNode(start);      // 시작 위치의 노드 찾기
            current.G = 0;                              // G, H 설정
            current.H = GetHeauristic(current, goal);
            open.Add(current);                          // 오픈리스트에 추가

            // A* 핵심 루틴 돌리기 시작
            while (open.Count > 0)                      // open 리스트가 빌 때까지 계속 진행. (오픈 리스트가 비면 경로를 못 찾은 것)
            {
                open.Sort();                            // F값을 기준으로 정렬
                current = open[0];                      // F값이 가장 작은 노드를 current로 설정
                open.RemoveAt(0);                       // 오픈리스트에서 current 꺼내기

                if (current != goal)                    // 꺼낸 current가 도착 지점인지 확인
                {
                    // 도착 지점이 아니면 계속 알고리즘 진행
                    // 클로즈 리스트에 넣기
                    close.Add(current);                 // current를 클로즈 리스트에 넣기

                    // 주변 8칸 open 리스트에 넣거나 이미 open리스트에 있으면 g값 갱신 시도
                    for (int y = -1; y < 2; y++)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            Node node = gridMap.GetNode(current.x + x, current.y + y);      // 주변 노드들을 하나씩 골라내기
                            // 스킵할 것들
                            if (node == null)                             // 맵 박일 경우
                            {
                                continue;
                            }

                            if (node == current)                         // current는 current의 주변이 아님 (즉 자기자신)
                            {
                                continue;
                            }

                            if (node.gridType == Node.GridType.Wall)     // 벽인 경우
                            {
                                continue;
                            }

                            if (close.Exists((x) => x == node))          // 클로즈 리스트에 있는 경우
                            {
                                continue;
                            }

                            bool isDiagonal = Mathf.Abs(x) == Mathf.Abs(y);     // 대각선인지 확인. true면 대각선
                            if (isDiagonal && (gridMap.GetNode(current.x + x, current.y).gridType == Node.GridType.Wall 
                                || gridMap.GetNode(current.x, current.y + y).gridType == Node.GridType.Wall))       // 대각선인데 벽에 걸리는 경우
                            {
                                continue;
                            }

                            // current에서 이웃인 node로 가는 거리 결정
                            float distance;
                            if(isDiagonal)
                            {
                                distance = 1.4f;            // 대각선이면 1.4
                            }
                            else
                            {
                                distance = 1.0f;            // 직선이면 1.0
                            }

                            // 노드의 G값이 current를 거쳐서 가는 것보다 더 작은 경우 G값 갱신
                            if(node.G > current.G + distance)
                            {
                                // open 리스트에 없을 경우 (G값의 초기화 max니까 무조건 들어옴)
                                if (node.parent == null)                // 처음 사용 하는 노드다 == 오픈 리스트에도 안들어가 있다.
                                {
                                    node.H = GetHeauristic(node, goal); // 휴리스틱 값 계산
                                    open.Add(node);                     // open리스트에 추가
                                }

                                // open리스트에 있든 없든 공통 처리되는 부분
                                node.G = current.G + distance;          // g값 갱신
                                node.parent = current;                  // 부모 설정(경로)
                            }
                        }
                    }
                }
                else
                {
                    // 도착 지점이면 더이상 탐색을 하지 않고 루틴 종료
                    break;
                }
            }

            // 마무리 작업
            if(current == goal)         
            {
                // 도작 지점에 도착했다.
                path = new List<Vector2Int>();          // 리스트 생성
                Node result = current;                  // current부터 시작해서
                while(result != null)                   // result가 있는 한 계속 진행(다음 부모로 계속 진행했는데 result가 null이면 시작점)
                {
                    path.Add(new Vector2Int(result.x, result.y)); // path에 지금 노드들의 위치를 기록
                    result = result.parent;             // 다음 부모로 이동
                }

                path.Reverse();                         // 도착 -> 시작으로 되어 있는 리스트를 뒤집기
            }
        }

        return path;
    }

    /// <summary>
    /// 휴리스틱 값을 구하기 위한 함수
    /// </summary>
    /// <param name="current">현재 위치의 노드(휴리스틱   값을 계산하기 위한 시작 위치)</param>
    /// <param name="goal">목적지의 그리드 좌표</param>
    /// <returns>current에서 goal까지의 예상 거리</returns>
    public static float GetHeauristic(Node current, Vector2Int goal)
    {
        return Mathf.Abs(goal.x - current.x) + Mathf.Abs(goal.y - current.y);           // 가로 세로를 직선 이동
    }
}
