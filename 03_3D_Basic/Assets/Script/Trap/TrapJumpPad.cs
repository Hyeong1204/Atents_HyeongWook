using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapJumpPad : TrapBase
{
    public float power = 5.0f;


    protected override void TrapActivate(GameObject target)
    {
        Rigidbody rigid = target.GetComponent<Rigidbody>();
        rigid.velocity = Vector3.zero;
        rigid.AddForce(transform.up * power, ForceMode.Impulse);
    }
}
