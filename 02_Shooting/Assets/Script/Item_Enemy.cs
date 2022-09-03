using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Enemy : MonoBehaviour
{
    public GameObject Item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Instantiate(Item, transform.position, Quaternion.Euler(0,0,90.0f));
        }
    }
}
