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
            path = new List<Vector2Int>();

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

                if(current != goal)
                {

                }
                else
                {
                    break;
                }
            }
        }



        return path;
    }

    public static float GetHeauristic(Node current, Vector2Int goal)
    {
        return Mathf.Abs(goal.x - current.x) + Mathf.Abs(goal.y - current.y);
    }
}
