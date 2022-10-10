using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    Transform[] background;

    public float scrollingSpeed = 3.0f;
    public float widthSize = 3.0f;          // 배경의 가로 사이즈 (땅 : 3.0 , 하늘 : 6.0)

    private void Awake()
    {
        background = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            background[i] = transform.GetChild(i);
        }
    }

    private void Update()
    {
        foreach(var slot in background)
        {
            slot.transform.position += Time.deltaTime * scrollingSpeed * -transform.right;       // scrollingSpeed의 속도로 왼쪽으로 이동
            if (slot.transform.position.x < -14)        // x축 기준 -14보다 작아지면
            {
                slot.transform.Translate(widthSize * transform.childCount * transform.right);
            }
        }
    }

}
