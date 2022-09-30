using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public Transform[] bgSlots;

    public float scrollingSpeed = 5.0f;
    public float width = 7.2f;  // 하늘은 7.2, 바닥은 8.4

    float edgePostion;

    private void Awake()
    {
        bgSlots = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            bgSlots[i] = transform.GetChild(i);
        }

    }

    private void Start()
    {
        edgePostion = transform.position.x - width * 2;
    }

    private void Update()
    {
        foreach(var slot in bgSlots)
        {
            slot.Translate(scrollingSpeed * Time.deltaTime * -transform.right);

            if(slot.position.x < edgePostion)
            {
                slot.Translate(width * bgSlots.Length * transform.right);
            }
        }
    }
}
