using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    Transform[] background;

    public float moveSpeed = 3.0f;

    float endPositionX = -14;
    float startPositionX = 17;

    private void Awake()
    {
        background = new Transform[transform.childCount];
        for(int i = 0; i< transform.childCount; i++)
        {
            background[i] = transform.GetChild(i);
        }
    }


    private void Update()
    {
        foreach(var slot in background)
        {
            slot.position += Time.deltaTime * moveSpeed * -transform.right;
            if(slot.transform.position.x < endPositionX)
            {
                slot.Translate(-(endPositionX - startPositionX) * transform.right);     // 31 만큼 이동
            }
        }
    }

    
}
