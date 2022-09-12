using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{

    public Action onballDead;
    public Action<int> onPlayerScore;
    public Action<int> onPlayerScore2p;

    private int playerScore = 0;
    private int playerScore2P = 0;

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

        if(collision.gameObject.CompareTag("Ball") && collision.transform.position.x > 0.0f)
        {
            playerScore++;
            onPlayerScore?.Invoke(playerScore);
        }


        if(collision.gameObject.CompareTag("Ball") && collision.transform.position.x < 0.0f)
        {
            playerScore2P++;
            onPlayerScore2p?.Invoke(playerScore2P);
        }
    }
}
