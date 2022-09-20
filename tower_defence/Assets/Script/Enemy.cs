using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int wayPointCount;              // 이동 경로 개수
    Transform[] wayPoints;          // 이동 경로 정보
    int currentIndex = 0;           // 현재 목표지점 인덱스
    Movement movement;              // 오브젝트 이동 제어


    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    public void Setup(Transform[] wayPoints)
    {
        // 적 이동 경로 wayPoints 정보 설정
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        transform.position = wayPoints[currentIndex].position;      // 적의 위치를 첫번째 wayPoints 위치로 설정(즉 시작 지점으로 이동)

        StartCoroutine(OnMove());
    }




    IEnumerator OnMove()
    {
        NextMoveTo();

        while (true)
        {
            transform.Rotate(Vector3.forward * 10);     // 적 오브젝트 회전

            // 적의 현재위치와 목표위치의 거리가 0.02 * movement.moveSpeed보다 작을 때 if 조건문 실행
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement.moveSpeed)    // Vector3.Distance (가장 가까운 오브젝트 찾기)
            {
                NextMoveTo();   // 다음 이동 방향 설정
            }

            yield return null;
        }

    }

    private void NextMoveTo()
    {
        if(currentIndex < wayPointCount - 1)        // 아직 이동할 wayPointCount가 남아 있다면
        {
            transform.position = wayPoints[currentIndex].position;  // 다음 이동 경로로 바꿈
            currentIndex++;

            Vector3 dir = (wayPoints[currentIndex].position - transform.position).normalized;   // 다음 목적지의 방향(dir)를 구함
            movement.MoveTo(dir);
        }
        else
        {
            Destroy(this.gameObject);   // 다음 목적지가 없다면 삭제
        }
    }
}
