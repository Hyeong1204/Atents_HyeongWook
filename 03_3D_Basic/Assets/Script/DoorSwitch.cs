using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 연결된 문을 열고 단느 스위치. IUseavleObject 상속받음
/// </summary>
public class DoorSwitch : MonoBehaviour, IUseavleObject
{
    Animator anim;
    bool switchOn = false;      // 스위치 사용 여부 true면 사용, false면 취소

    public ActiveTwoWayDoor targetDoor; // 스위치로 열고 닫을 문

    private void Awake()
    {
        anim = GetComponent<Animator>();
        
    }


    /// <summary>
    /// 이 오브젝트가 사용되면 실행될 함수
    /// </summary>
    public void Use()
    {
        switchOn = !switchOn;       // 스위치 on/off 서로 전환
        anim.SetBool("SwitchOn", switchOn); // shitchOn에 맞게 애니메이션 재생
        if (switchOn)
        {
            targetDoor.Open();  // 스위치를 켰으면 문을 연다
        }
        else
        {
            targetDoor.Close(); // 스위치를 끄면 문을 닫는다.
        }
    }

}
