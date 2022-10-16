using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public Transform[] wayPoint;
    public float moveSpeed = 3.0f;
    public float rotateSpeed = 10.0f;

    int currentWayPoint = 0;
    bool isMove = true;

    Vector3 dir = Vector3.zero;            // 다음 wayPoint 방향
    Quaternion targetRotation = Quaternion.identity;        // 바라볼 방향

    Animator anima;
    Rigidbody rigid;

    private void Awake()
    {
        anima = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.MovePosition(wayPoint[currentWayPoint].position);           // 시작위치
        StartCoroutine(NextMove());
    }

    private void FixedUpdate()
    {
        if (isMove)
        {
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * dir);
            rigid.MoveRotation(Quaternion.Slerp(rigid.rotation, targetRotation, Time.fixedDeltaTime * rotateSpeed));
        }
    }

    IEnumerator NextMove()      // 다음 지점을 결정해주는 코루틴
    {
        while (true)
        {
            if (currentWayPoint < wayPoint.Length)
            {
                if(Vector3.Distance(rigid.position, wayPoint[currentWayPoint].position) < 0.02 * moveSpeed)         // 다음 wayPoint까지의 거리가 0.02 * moveSpeed 보다 작으면
                {
                    rigid.position = wayPoint[currentWayPoint].position;            // 현재 포지션을 wayPoint으로 바꾸기
                    StartCoroutine(MvoeStop());                     // 잠시 멈췄다가 움직이는 코루틴 시작
                    currentWayPoint++;
                     
                    if(currentWayPoint >= wayPoint.Length)          // 다음 웨이 포인트가 없으면 시작점으로
                    {
                        currentWayPoint = 0;                        // 처음 지점으로
                    }

                    dir = (wayPoint[currentWayPoint].position - transform.position).normalized;     // 다음 진행 방향
                    targetRotation = Quaternion.LookRotation(dir);              // 다음지점 바라보기
                    
                }
            }
                yield return null;
        }
    }

    IEnumerator MvoeStop()          // 멈췄다가 n초 후에 다시 움직이기
    {
        StopMove();
        yield return new WaitForSeconds(2.0f);
        StartMove();
    }

    void StopMove()                 // 이동 시작
    {
        isMove = false;
        anima.SetFloat("Speed", 0.0f);
    }

    void StartMove()                // 이동 중지
    {
        anima.SetFloat("Speed", 1.0f);
        isMove = true;
    }
}
