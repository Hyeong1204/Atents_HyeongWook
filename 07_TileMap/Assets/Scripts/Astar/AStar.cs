using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public static class AStar
{
    public static List<Vector2Int> PathFind(GridMap gridMap, Vector2Int start, Vector2Int goal)
    {
        gridMap.ClearAstarDate();
        List<Vector2Int> path = null;
        if (gridMap.IsValidPostion(start) && gridMap.IsValidPostion(goal))
        {
            List<Node> open = new List<Node>();
            List<Node> close = new List<Node>();

            Node current = gridMap.GetNode(start);
            current.G = 0;
            current.H = GetHeauristic(current, goal);
            open.Add(current);

            while (open.Count > 0)
            {
                open.Sort();
                current = open[0];
                open.RemoveAt(0);

                if (current != goal)
                {
                    // 클로즈 리스트에 넣기
                    close.Add(current);

                    // 주변 8칸 open 리스트에 넣거나 g값 갱신 시도
                    for (int y = -1; y < 2; y++)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            Node node = gridMap.GetNode(current.x + x, current.y + y);
                            // 스킵할 것들
                            if (node == null)    // 맵 박일 경우
                            {
                                continue;
                            }

                            if (node == current) // current는 current의 주변이 아님
                            {
                                continue;
                            }

                            if (node.gridType == Node.GridType.Wall)     // 벽인 경우
                            {
                                continue;
                            }

                            if (close.Exists((x) => x == node))         // 클로즈 리스트에 있는 경우
                            {
                                continue;
                            }

                            bool isDiagonal = Mathf.Abs(x) == Mathf.Abs(y);     // 대각선인지 확인. true면 대각선
                            if (isDiagonal && (gridMap.GetNode(current.x + x, current.y).gridType == Node.GridType.Wall 
                                || gridMap.GetNode(current.x, current.y + y).gridType == Node.GridType.Wall))       // 대각선인데 벽에 걸리는 경우
                            {
                                continue;
                            }

                            float distance;
                            if(isDiagonal)
                            {
                                distance = 1.4f;
                            }
                            else
                            {
                                distance = 1.0f;
                            }

                            // G값이 current를 거쳐서 가는 것보다 더 작은 경우
                            if(node.G > current.G + distance)
                            {
                                if (node.parent == null)                // 처음 사용 하는 노드다 == 오픈 리스트에도 안들어가 있다.
                                {
                                    node.H = GetHeauristic(node, goal); // 휴리스틱 값 계산
                                    open.Add(node);                     // open리스트에 추가
                                }

                                node.G = current.G + distance;          // g값 갱신
                                node.parent = current;                  // 부모 설정(경로)
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            if(current == goal)
            {
                // 도작 지점에 도착했다.
                path = new List<Vector2Int>();
                Node result = current;
                while(result != null)
                {
                    path.Add(new Vector2Int(result.x, result.y));
                    result = result.parent;
                }

                path.Reverse();
            }
        }

        return path;
    }

    public static float GetHeauristic(Node current, Vector2Int goal)
    {
        return Mathf.Abs(goal.x - current.x) + Mathf.Abs(goal.y - current.y);
    }
}
