using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

public class spawnerManager : MonoBehaviour
{
    GridMap gridMap;
    Tilemap background;
    Tilemap obstacle;

    Spawner[] spawners;

    List<Slime> spawnedList;

    public List<Slime> SpawnedList => spawnedList;

    public GridMap GridMap => gridMap;

    private void Awake()
    {
        Transform grid = transform.parent;
        Transform child = grid.GetChild(0);
        background = child.GetComponent<Tilemap>();                         // 타일맵 가져오기
        child = grid.GetChild(1);
        obstacle= child.GetComponent<Tilemap>();

        gridMap = new GridMap(background, obstacle);                        // 그리드 맵 만들기

        spawners = GetComponentsInChildren<Spawner>();                      // 자식으로 있는 스포너 가져오기

        //foreach (var spawner in spawners)
        //{
        //    spawner.onSpawned += (slime) =>
        //    {
        //        spawnedList.Add(slime);
        //        slime.onDie += () => spawnedList.Remove(slime);
        //    };
        //}

        //spawnedList = new List<Slime>();

        //StartCoroutine(GetSpawnerData());
    }

    IEnumerator GetSpawnerData()
    {
        string url = "http://go26652.dothome.co.kr/HTTP_Data/SpawnerData.txt";

        UnityWebRequest www = UnityWebRequest.Get(url);         // 웹에 http를 통해 데이터를 가져오는 요청을 만듬

        yield return www.SendWebRequest();                      // 만든 요청을 실제로 보내고 결과가 도착할 때까지 대기

        if (www.result != UnityWebRequest.Result.Success)        // 결과가 정상적으로 돌아왔는지 확인
        {
            Debug.LogError(www.error);                               // 성공이 아니면 에러 출력
        }
        else
        {
            //Debug.Log($"Result : {www.downloadHandler.text}");  // 성공이면 데이터 받아와서 처리
            string json = www.downloadHandler.text;
            SpawnerData data = JsonUtility.FromJson<SpawnerData>(json);

            foreach (var spawner in spawners)
            {
                spawner.delay = data.delay;
                spawner.capacity = data.capacity;
            }
        }
    }

    /// <summary>
    /// 랜덤하게 스폰할 위치를 구하는 함수
    /// </summary>
    /// <param name="pos">스포너의 위치</param>
    /// <param name="size">스포너의 스폰 영역 크기</param>
    /// <returns>몬스터가 스폰될 위치</returns>
    //public Vector3 GetRandomSpawnPosition(Vector3 pos, Vector2 size)
    //{
    //    List<Vector2Int> result = new List<Vector2Int>();
    //    Vector2Int min = gridMap.WorldToGrid(pos);                                          // 그리드 좌표의 최소 값 계산
    //    Vector2Int max = gridMap.WorldToGrid(pos + (Vector3)size);   // 그리드 좌표의 최대 값 계산
    //    for (int y = min.y; y < max.y; y++)
    //    {
    //        for (int x = min.x; x < max.x; x++)
    //        {
    //            if (gridMap.IsSpawnable(x, y))                                              // 스폰 가능한 위치면
    //            {
    //                result.Add(new(x,y));                                                   // 기록해 놓기
    //            }
    //        }
    //    }

    //    return gridMap.GridToWorld(result[Random.Range(0, result.Count)]);                  // 기록한 위치중에서 하나를 랜덤으로 골라 월드 좌표로 변경해서 리턴
    //}

    /// <summary>
    /// 스포너의 스폰 영역 중에서 벽이 아닌 노드들만 찾아서 돌려주는 함수
    /// </summary>
    /// <param name="spawner">계산할 스포너</param>
    /// <returns>스포너의 스폰 영역에 있는 벽이 아닌 노드들</returns>
    public List<Node> CalcSpawnArea(Spawner spawner)
    {
        List<Node> nodes = new List<Node>();

        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int min = gridMap.WorldToGrid(spawner.transform.position);                           // 그리드 좌표의 최소 값 계산
        Vector2Int max = gridMap.WorldToGrid(spawner.transform.position + (Vector3)spawner.size);   // 그리드 좌표의 최대 값 계산
        for (int y = min.y; y < max.y; y++)
        {
            for (int x = min.x; x < max.x; x++)
            {
                if (gridMap.IsSpawnable(x, y))                                              // 스폰 가능한 위치면
                {
                    nodes.Add(gridMap.GetNode(x,y));                                        // 기록해 놓기
                }
            }
        }

        return nodes;
    }
}
