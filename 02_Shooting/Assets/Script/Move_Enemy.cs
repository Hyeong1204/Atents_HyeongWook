using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Move_Enemy : MonoBehaviour
{
    float speed = 7.0f;
    Vector3 dis = new Vector3(-10, 0, 0);
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector3.left);
        if(transform.position.x < dis.x)
        {
            Destroy(gameObject);
        }
    }
}