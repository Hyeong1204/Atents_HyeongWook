using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //public GameObject monsterPrefab;    // 생성할 몬스터의 프리팹
    public float delay = 1.0f;     // 몬스터 생성하는 시간 간격
    public int capacity = 2;            // 최대로 유지 가능한 몬스터의 수
    public Vector2 size;                // 스폰 영역의 크기 (transform의 position에서 부터의 크기)

    float elapased = 0.0f;         // 이전 몬스터 생성에서 붙터 경과한 시간
    int count = 0;                 // 현재 생성된 몬스터의 수

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
            }

            Vector3 pos = transform.position + new Vector3(Random.Range(0, size.x), Random.Range(0, size.y), 0.0f);
            slime.transform.position = pos;
        }

        return slime;
    }

    void DecressCount()
    {
        count--;
    }
}
