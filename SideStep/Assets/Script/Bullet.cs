using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public GameObject Obstacles;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(Random.Range(-8.5f, 8.5f), 9.0f, 0.0f);

        if(Random.Range(0.0f,1.0f) < 0.01f)
        {
            Instantiate(Obstacles, transform.position = new Vector3(Random.Range(-8.5f, 8.5f), 9.0f, 0.0f), Quaternion.identity);
        }
    }
}
