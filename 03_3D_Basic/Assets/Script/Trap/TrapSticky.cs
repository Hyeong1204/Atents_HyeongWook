using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TrapSticky : TrapBase
{
    // 들어오면 몇 초동안 이동 속도가 10%가 느려진다.

    public float speedDebuff = 0.5f;
    public float duration = 3.0f;

    float originalSpeed = 0.0f;

    Player player = null;

    protected override void TrapActivate(GameObject target)
    {
        if(player == null)
        {
        player = target.GetComponent<Player>();
        originalSpeed = player.moveSpeed;
        player.moveSpeed *= speedDebuff;
        }
        else
        {
            StopAllCoroutines();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                StartCoroutine(ReleaseDebuff());
            }
        }
        
    }




    IEnumerator ReleaseDebuff()
    {
        yield return new WaitForSeconds(duration);
        player.moveSpeed = originalSpeed;
        originalSpeed = 0.0f;
        player = null;
    }

}
