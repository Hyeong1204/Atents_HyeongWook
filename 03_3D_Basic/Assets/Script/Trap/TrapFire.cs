using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFire : TrapBase
{
    ParticleSystem ps;
    private void Awake()
    {
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    protected override void TrapActivate(GameObject target)
    {
        ps.Play();
        StartCoroutine(EffectStop());

        IDead deadTarget = target.GetComponent<IDead>();
        if (deadTarget != null)
        {
            deadTarget.Die();
        }
    }


    IEnumerator EffectStop()
    {
        yield return new WaitForSeconds(1.0f);
        ps.Stop();
    }
}
