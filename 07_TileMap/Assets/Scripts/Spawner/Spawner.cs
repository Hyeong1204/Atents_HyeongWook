using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 
public class Spawner : MonoBehaviour
{
    //public GameObject monsterPrefab;    // 생성할 몬스터의 프리팹
    public float delay = 1.0f;     // 몬스터 생성하는 시간 간격
    public int capacity = 2;            // 최대로 유지 가능한 몬스터의 수
    public Vector2 size;                // 스폰 영역의 크기 (transform의 position에서 부터의 크기)

    float elapased = 0.0f;         // 이전 몬스터 생성에서 붙터 경과한 시간
    int count = 0;                 // 현재 생성된 몬스터의 수

    /// <summary>
    /// 스포너가 배치 되어 있는 맵
    /// </summary>
    GridMap gridMap;

    /// <summary>
    /// 스포너가 배치되어 있는 맵을 확인하기 위한 프로퍼티
    /// </summary>
    public GridMap GridMap => gridMap;

    /// <summary>
    /// 스포너가 있는 씬의 몬스터 메니저
    /// </summary>
    SceneMonsterManager manager;

    /// <summary>
    /// 스폰 지역 중에서 벽이 아닌 지역
    /// </summary>
    List<Node> spawnAreaList;

    private void Start()
    {
        manager = GetComponentInParent<SceneMonsterManager>();
        spawnAreaList = manager.CalcSpawnArea(this);                // 스폰 영억 중에서 벽이 아닌 위치들의 모음 가져오기
        //gridMap = manager.GridMap;
    }

    private void Update()
    {
        if (count < capacity)
        {
            elapased += Time.deltaTime;
            if (elapased > delay)
            {
                Spawn();
                elapased = 0.0f;
            } 
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 pos = new Vector3(Mathf.Floor(transform.position.x), Mathf.Floor(transform.position.y));
        Vector3 p0 = pos;
        Vector3 p1 = pos + new Vector3(size.x, 0);
        Vector3 p2 = pos + new Vector3(size.x, size.y);
        Vector3 p3 = pos + new Vector3(0, size.y);

        Handles.color = Color.yellow;
        Handles.DrawLine(p0, p1, 5);
        Handles.DrawLine(p1, p2, 5);
        Handles.DrawLine(p2, p3, 5);
        Handles.DrawLine(p3, p0, 5);
    }
#endif

    public Slime Spawn()
    {
        Slime slime = null;
        if (count < capacity)
        {
            slime = SlimeFactory.Inst.GetSlime();
            if (slime != null)
            {
                count++;
                slime.onDie -= DecressCount;        // DecressCount가 누적되지 않게하기 위한 조치
                slime.onDie += DecressCount;
            
                slime.Initialize(manager.GridMap, GetSpawnPosition());          // 그리드맵 전달 + 스폰될 위치 전달
            }
        }

        return slime;
    }

    void DecressCount()
    {
        count--;
    }

    /// <summary>
    /// spawnAreaList에서 현재 몬스터가 없는 위치를 랜덤으로 찾는 함수
    /// </summary>
    /// <returns>몬스터가 없는 노드의 월드 좌표</returns>
    Vector3 GetSpawnPosition()
    {
        List<Node> spawns = new List<Node>();
        foreach(var node in spawnAreaList)                  // 미리 찾아 놓은 sapwnAreaList 뒤지기
        {
            if(node.gridType == Node.GridType.Plain)        // 평지일 경우 
            {
                spawns.Add(node);                           // spawns에 저장
            }
        }

        int index = Random.Range(0, spawns.Count);
        Node target = spawns[index];                        // spawns 중에서 랜덤으로 하나 선택
        Vector2Int gridPos = new Vector2Int(target.x, target.y);
        return manager.GridMap.GridToWorld(gridPos);        // 선택한 그리드 좌표를 월드 좌표로 변경해서 리턴
    }
}
