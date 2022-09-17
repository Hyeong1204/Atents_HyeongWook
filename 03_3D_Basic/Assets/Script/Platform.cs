using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Transform target;
    

    public float speed = 3.0f;      // 엘레베이터의 속도
    private bool moveStart = false;

    private Vector3 dir;        // 방향
    private Vector3 golaTrans;      // 도착지점 위치

    private void Awake()
    {
        target = transform.GetChild(0);

    }

    private void Start()
    {
        dir = target.transform.position - transform.position;   //  방향 구하기
        dir = dir.normalized;    //dir를 정규화   
        golaTrans = target.transform.position;  // 시작과 동시에 golaTrans의 값을 target의 위치로 만듬

        target.transform.parent = null;         // 자식 지우기
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))  //플레이어가 들어가면
        {
            if (transform.position.y < target.transform.position.y)     // transform.position.y가 target.transform.position.y 작으면
            {
                moveStart = true;       // moveStart를 true로 바꾸기
            }
        }
    }

    private void Update()
    {
        if (moveStart)  // moveStart가 true면
        {
            transform.Translate(speed * Time.deltaTime * dir); // dir의 방향으로 이동

            if(transform.position.y >= golaTrans.y)     // 자신의 y위치가 golaTrans y위치보다 크면
            {
                moveStart = false;      // moveStart를 false로 바꾸기
            }
            
        }
    }

}
