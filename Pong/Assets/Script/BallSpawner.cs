using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject prefub;
    private bool ballDead = true;


    IEnumerator SpawnTime;


    private void Awake()
    {
        SpawnTime = Spawn();
    }

    private void Start()
    {
        StartCoroutine(SpawnTime);

        Killzone kill = GameObject.FindObjectOfType<Killzone>();
        kill.onballDead += Dead;
    }

   


   IEnumerator Spawn()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(5.0f);


            if (ballDead)
            {
                GameObject obj = Instantiate(prefub, transform.position, Quaternion.identity);
                obj.transform.Translate(0, Random.Range(-5.0f, 5.0f), 0);
                ballDead = false;
            }
        }
    }


    void Dead()
    {
        ballDead = true;
    }

}
