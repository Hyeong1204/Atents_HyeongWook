using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PipeRotator : MonoBehaviour
{
    /// <summary>
    /// 움직일 파이프
    /// </summary>
    public Pipe[] pipes;

    /// <summary>
    /// 파이프 끝 위치
    /// </summary>
    Transform endPoiunt;

    /// <summary>
    /// 파이프 시작 위치
    /// </summary>
    Transform startPoiunt;

    /// <summary>
    /// 파이프가 움직이는 속도
    /// </summary>
    public float pipeMoveSpeed = 5.0f;
    float currentPipeMoveSpeed = 0.0f;          // 일단 파이프가 안움직이도록 속도를 0으로 함

    private void Awake()
    {
        endPoiunt = transform.GetChild(transform.childCount - 2);       // endPoiunt 구하기
        startPoiunt = transform.GetChild(transform.childCount - 1);     // startPoiunt 구하기
        pipes = GetComponentsInChildren<Pipe>();       // 자식으로 있는 Pipe 모두 찾기
    }

    private void FixedUpdate()
    {
        //foreach(var slot in bgslot)
        //{
        //    slot.Translate(rotatorSpeed * Time.deltaTime * -transform.right);

        //    if (endPoiunt.position.x > slot.position.x)
        //    {
        //        slot.position = Vector3.zero;
        //        slot.position = startPoiunt.position;
        //        Pipe pipe = slot.GetComponent<Pipe>();
        //        pipe.RestRandomHeight();
        //    }
        //}

        for (int i = 0; i < pipes.Length; i++)     //bgslot에 있는 모든 bgslot를 하나씩 처리하기
        {
            pipes[i].MoveLeft(currentPipeMoveSpeed * Time.fixedDeltaTime);    // 파이프를 계속 왼쪽으로 이동 시키기

            if(endPoiunt.position.x > pipes[i].transform.position.x)   // 파이프 위치가 bgslot[i].transform.position.x보다 왼쪽인지 체크
            {
                    //bgslot[i].RestRandomHeight();
                    pipes[i].transform.position = new Vector2(startPoiunt.position.x, pipes[i].RandomHeight);
            }
        }
    }

    /// <summary>
    /// 파이프 별로 득점했을 때 점수를 올려주는 함수등록
    /// </summary>
    /// <param name="del">하나의 파이프가 득점 상황에서 실행될 함수 또는 델리게이트</param>
    public void AddPipeScoreDelegate(Action<int> del)
    {
        foreach(Pipe pipe in pipes)
        {
            pipe.onScored += del;
        }
    }


    /// <summary>
    /// 게임이 시작될 때 실행될 함수
    /// </summary>
    public void OnGameStart()
    {
        currentPipeMoveSpeed = pipeMoveSpeed;       // 실제 움직여야 할 속도로 설정
    }
}
