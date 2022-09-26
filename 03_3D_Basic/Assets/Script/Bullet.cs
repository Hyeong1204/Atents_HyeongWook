using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    float moveSpeed = 20.0f;
    float killTime = 2.0f;

    Rigidbody rigid;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, killTime);
        rigid.velocity = transform.forward * moveSpeed;
    }

    private void FixedUpdate()
    {
        //rigid.MovePosition(transform.position + moveSpeed * Time.fixedDeltaTime * transform.forward);
    }
    
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDead deadTarget = collision.gameObject.GetComponent<IDead>();
            if (deadTarget != null)
            {
                deadTarget.Die();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDead deadTarget = other.gameObject.GetComponent<IDead>();
            if (deadTarget != null)
            {
                deadTarget.Die();
            }
        }
    }
}
