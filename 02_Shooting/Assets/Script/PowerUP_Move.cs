using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUP_Move : MonoBehaviour
{
    float speed = 3.0f;
    float coolTime = 5.0f;
    Vector2 move;

    Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        SetRandomDir();
        StartCoroutine(DirChange());
        Destroy(this.gameObject, 10.0f);            // 10초 뒤에 소멸
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * move);
    }


   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            move = -Vector2.Reflect(move, collision.contacts[0].normal);
        }
    }

    IEnumerator DirChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(coolTime);
            SetRandomDir();
        }
    }

    void SetRandomDir(bool allRandom = true)            // 디폴트 파라메터. 값을 지정하지 않으면 디폴트 값이 대신 들어간다.
    {
        if (allRandom)
        {
        move = Random.insideUnitCircle;             // 원 중심점 기준으로 아무곳을 랜덤하게 지정
        move = move.normalized;
        }
        else
        {
            Vector2 playerToPowerUp = transform.position - player.transform.position;
        }
    }

}
