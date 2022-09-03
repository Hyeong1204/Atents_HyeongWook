using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUP_Move : MonoBehaviour
{
    float speed = 4.0f;
    float coolTime = 0.0f;
    Vector3 move;


    private void Start()
    {
        move = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
    }

    // Update is called once per frame
    void Update()
    {
        coolTime += Time.deltaTime;
        if(coolTime >= 1.0f)
        {
           move = new Vector3(Random.Range(-1,1), Random.Range(-1,1), 0);
            coolTime = 0.0f;
        }


        transform.Translate(speed * Time.deltaTime * move);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Border"))
        {
            move.y = -move.y;
            move.x = -move.x;
        }
    }


}
