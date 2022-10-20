using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("몬스터 공격");
            IBattle target = other.GetComponent<IBattle>();
            if (target != null)
            {
                player.Attact(target);
            }
        }
    }
}
