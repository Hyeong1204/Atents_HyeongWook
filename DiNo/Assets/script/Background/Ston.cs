using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ston : MonoBehaviour
{
    SpriteRenderer[] sprite;
    Transform[] ston;

    public float moveSpeed = 4.0f;
    float endPointX = -14;

    private void Awake()
    {
        ston = new Transform[transform.childCount];
        sprite = new SpriteRenderer[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)
        {
            ston[i] = transform.GetChild(i);
            sprite[i] = ston[i].GetComponent<SpriteRenderer>();
        }
    }


    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            ston[i].position += Time.deltaTime * moveSpeed * -transform.right;
            if (ston[i].position.x < endPointX)
            {
                ston[i].Translate(-(endPointX * 2) * transform.right);          // 오른쪽으로 -endPointX * 2 만큼 이동
                int rand = Random.Range(0, 2);
                sprite[i].flipX = (rand & 0b_01) != 0;              // 랜덤으로 플립 바꾸기
            }
        }
    }
}
