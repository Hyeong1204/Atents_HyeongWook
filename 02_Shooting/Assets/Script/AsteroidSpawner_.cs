
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AsteroidSpawner_ : EnemySpawner
{

    Transform destination;


    private void Awake()
    {
        // 오브젝트가 생성된 직후
    }



    // Start is called before the first frame update
    void Start()
    {
        // 첫번째 업데이트 실행 직전 호출
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override IEnumerator EnemySpawn()
    {
        while (true)
        {
            GameObject obj = Instantiate(SpawnObject, transform.position, Quaternion.identity);   // 생성하고 부모를 이 오브젝트로 설정
            obj.transform.Translate(0, Random.Range(minY, maxY), 0);        // 스폰 생성 범위 안에서 랜덤으로 높이 정하기

            yield return new WaitForSeconds(SpawnTime);     // SpawnTime 만큼 대기
        }
    }


    
    protected override void OnDrawGizmos()         // 개발용 정보를 항상 그리는 함수
    {
        //Gizmos.color = new Color(1, 0, 0);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new(1, Mathf.Abs(minY) + Mathf.Abs(maxY) + 2, 1));
    }

    //private void OnDrawGizmosSelected()     // 개발자 영역에서만 보이는 영역
    //{
        
    //}
    
}
