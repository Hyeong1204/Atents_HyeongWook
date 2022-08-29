using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    public float SpawnTime = 1.0f;
    float minY = -4.0f;
    float maxY = 4.0f;


    IEnumerator enemySpawn;



    // Start is called before the first frame update
    void Start()
    {
        enemySpawn = EnemySpawn();
        StartCoroutine(enemySpawn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnemySpawn()
    {
        while (true)
        {            
            GameObject obj = Instantiate(Enemy, transform.position, Quaternion.identity);   // 생성하고 부모를 이 오브젝트로 설정
            obj.transform.Translate(0, Random.Range(minY, maxY), 0);        // 스폰 생성 범위 안에서 랜덤으로 높이 정하기
            yield return new WaitForSeconds(SpawnTime);     // SpawnTime 만큼 대기
        }
    }


    
    private void OnDrawGizmos()         // 개발용 정보를 항상 그리는 함수
    {
        //Gizmos.color = new Color(1, 0, 0);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new(1, Mathf.Abs(minY) + Mathf.Abs(maxY) + 2, 1));
    }

    //private void OnDrawGizmosSelected()     // 개발자 영역에서만 보이는 영역
    //{
        
    //}

}
