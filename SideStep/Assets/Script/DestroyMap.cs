using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMap : MonoBehaviour
{

    Vector2 limitMax = new Vector2(9.0f, 10.0f);
    Vector2 limitMin = new Vector2(-9.0f, -4.0f);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > limitMax.x || transform.position.x < limitMin.x ||
            transform.position.y > limitMax.y || transform.position.y < limitMin.y)
        {
            Destroy(gameObject);
        }
    }
}
