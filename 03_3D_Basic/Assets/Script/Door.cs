using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 수동문. IUseavleObject(사용할 수 있는 오브젝ㅈ트라는 특성을 가짐) 상속 받음.
/// </summary>
public class Door : MonoBehaviour, IUseavleObject
{
    protected Animator anim;    // 애니메이터 컴포넌트

    bool playerIn = false;      // 플레이어가 문을 열수 있는 위치에 있는지 판단하는 변수. 
    bool ondoor = true;         // 현재 문이 열려있는지를 기록하는 변수

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("문 열림");
            playerIn = true;    // 플레이어가 문앞에 오면 playerIn를 true로 만든다.


        }
    }


    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("문 닫힘");
            playerIn = false;   // 플레이어가 문이랑 멀어지면 playerIn를 false로 만든다.


        }

    }


    /// <summary>
    /// 오브젝트가 사용될 때 사용될 함수
    /// </summary>
    public virtual void Use()
    {
        if (playerIn)       // 플레이어가 문앞에 있는지 확인
        {
            anim.SetBool("IsOpen", ondoor);
            ondoor = !ondoor;       // 문 열린 상태 표시용 스위치 변경하기
        }
    }

    

}
