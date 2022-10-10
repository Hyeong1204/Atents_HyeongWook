using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusMove : MonoBehaviour
{
    Rigidbody2D rigid;

    public float moveSpeed = 4.0f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveSpeed * Time.fixedDeltaTime * Vector2.left);

        if(transform.position.x < -14)
        {
            Destroy(this.gameObject);
        }
    }
}
