using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 양방향 수동문. TwoWayDoor, IUseavleObject 상속받음
/// </summary>
public class ActiveTwoWayDoor : TwoWayDoor, IUseavleObject
{
    bool DoorOpen = true;       // 현재 문이 열렸는지 닫혔는지 표시용
    bool PlayerIn = false;
    Vector3 playerToDoor;

    private void OnTriggerEnter(Collider other)
    {
        PlayerIn = true;
        playerToDoor = transform.position - other.transform.position;       
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerIn = false;
    }




    public override void Use()
    {
        if (PlayerIn)
        {
            if (DoorOpen)   // 문이 닫혀있으면 문을 열고
            {
                if (Vector3.Angle(transform.forward, playerToDoor) > 90.0f) // 플레이어가 문 앞에 있는지 뒤에 있는지 판단
                {
                    anim.SetTrigger("OpeninFront");
                }
                else
                {
                    anim.SetTrigger("OpeninBack");
                }
            }
            DoorOpen = !DoorOpen;
            if (DoorOpen)   // 문이 열려있으면 닫는다.
            {
                anim.SetTrigger("Close");
            }
        }
    }

    public void Open()
    {
        anim.SetTrigger("OpeninBack");
    }

    public void Close()
    {
        anim.SetTrigger("Close");
    }
}
