using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{

    public Action onballDead;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);

        if (collision.gameObject.CompareTag("Ball"))
        {
            onballDead?.Invoke();
        }
    }
}
