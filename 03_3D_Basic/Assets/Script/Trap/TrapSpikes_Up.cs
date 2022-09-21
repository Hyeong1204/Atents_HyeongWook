using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikes_Up : TrapBase
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }



    protected override void TrapActivate(GameObject target)
    {
        anim.SetTrigger("OnTrap");
    }
}
