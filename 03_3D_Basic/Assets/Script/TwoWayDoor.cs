using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// 양쪽으로 열리고 닫히는 문. Door 상속 받음
/// </summary>
public class TwoWayDoor : Door
{

    bool doorOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            // 플레이어가 문앞에 있는 상황에서
            doorOpen = IsInFront(other.transform.position);     // 플레이어의 위치가 문 앞인지 뒤인지 확인하고


            Open(); // 문을 연다
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetTrigger("Close");
        }
    }

    /// <summary>
    /// 문이 여는 함수
    /// </summary>
    void Open()
    {
        if (doorOpen)
        {
            anim.SetTrigger("OpeninFront");
        }
        else
        {
            anim.SetTrigger("OpeninBack");
        }
    }

    private bool IsInFront(Vector3 PlayerPostion)
    {
        Vector3 playerToDoor = transform.position - PlayerPostion;          // 플레이어 위치에서 문의 위치로 가는 방향 벡터 계산
        return (Vector3.Angle(transform.forward, playerToDoor) > 90.0f);    // 방향 벡터와 문의 front 방향 벡터의 사이각을 통해 앞 뒤 판다.
       
    }

}
