using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BsllSpawner : MonoBehaviour
{
    public GameObject prefub;

    IEnumerator SpawnTime;


    private void Awake()
    {
        SpawnTime = Spawn();
    }

    private void Start()
    {
        StartCoroutine(SpawnTime);
    }

   


   IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);

            GameObject obj = Instantiate(prefub, transform.position, Quaternion.identity);
            obj.transform.Translate(0, Random.Range(-5.0f, 5.0f), 0);
        }
    }

}
