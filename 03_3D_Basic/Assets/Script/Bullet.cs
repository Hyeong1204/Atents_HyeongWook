using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    float moveSpeed = 20.0f;    // 총알 속도
    float killTime = 2.0f;      // 디스폰 시간

    Rigidbody rigid;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rigid.velocity = transform.forward * moveSpeed;     // 리지드 바디에 velocity를 앞 방향으로 moveSpeed속도로 발사;
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
                deadTarget.Die();       // 플레이어 죽이기
            }
        }
        Destroy(this.gameObject, killTime);     // 벽이나 플레이어에 충돌하면 2초 후에 디스폰
    }

}
