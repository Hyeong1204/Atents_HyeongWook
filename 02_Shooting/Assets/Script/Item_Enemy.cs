using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Enemy : MonoBehaviour
{
    public GameObject Item;         // item_Enemy에 붙어 있는 파워업 아이템

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 블릿에 맞으면 실행
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Instantiate(Item, transform.position, Quaternion.Euler(0,0,90.0f));
        }
    }
}
