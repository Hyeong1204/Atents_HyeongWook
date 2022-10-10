using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusSpanwer : MonoBehaviour
{
    public GameObject[] cactusPerfabs;         // 장애물 프리팹
    WaitForSeconds delay;

    Vector3 spawnDir;                       // 생성 위치

    float spawnPointX = 11.0f;              // x 좌표 생성 위치
    float spawnPointY = -2.2f;               // y 좌표 생성 위치
    public float spawnDelay = 3.0f;                // 스폰 딜레이

    private void Start()
    {
        delay = new WaitForSeconds(spawnDelay);
        spawnDir = new Vector3(spawnPointX, spawnPointY, 0.0f);
        StartCoroutine(SpawnStart());
    }

    IEnumerator SpawnStart()
    {
        while (true)
        {
            yield return delay;             // delay 시간 만큼 지연

            int rand = Random.Range(0, 3);
            Instantiate(cactusPerfabs[rand], spawnDir, Quaternion.identity);          // 장애물 프리팹 생성
        }
    }

    void StopSpawner()
    {
        StopAllCoroutines();
    }
}
