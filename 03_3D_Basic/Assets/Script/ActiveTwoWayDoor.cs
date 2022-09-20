using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveTwoWayDoor : TwoWayDoor, IUseavleObject
{
    bool DoorOpen = true;
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
            if(DoorOpen)
            if (Vector3.Angle(transform.forward, playerToDoor) > 90.0f)
            {
                anim.SetTrigger("OpeninFront");
            }
            else
            {
                anim.SetTrigger("OpeninBack");
            }
            DoorOpen = !DoorOpen;
            if (DoorOpen)
            {
                anim.SetTrigger("Close");
            }
        }
    }
}
