using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    


    float speed = 12.0f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.right, Space.Self);        // Space.Self : 자기 기준, Space.World : 씬 기준
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"on : {collision.gameObject.name}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"ontrigger : {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Enemy"))
        {
        //Destroy(collision.gameObject);
        Destroy(this.gameObject);
        }
    }

}
