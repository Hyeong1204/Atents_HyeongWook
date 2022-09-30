using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public Transform[] bgSlots;

    /// <summary>
    /// 스크롤링 스피드
    /// </summary>
    public float scrollingSpeed = 5.0f;
    /// <summary>
    /// 배경 이미지의 가로 크기
    /// </summary>
    public float width = 7.2f;  // 하늘은 7.2, 바닥은 8.4

    /// <summary>
    /// 이미지가 반대쪽으로 넘어갈 위치(충분히 왼쪽으로 이동할 위치)
    /// </summary>
    float endPoint;

    private void Awake()
    {

        bgSlots = new Transform[transform.childCount];      // 자식 갯수만큼 bgSlots 배열 크기 잡기
        for (int i = 0; i < transform.childCount; i++)
        {
            bgSlots[i] = transform.GetChild(i);     // bgSlots에 들어갈(이미지가 있는 자식 오브젝트) 드랜스폼들 넣기
        }

    }

    private void Start()
    {
        endPoint = transform.position.x - width * 2;
    }

    private void Update()
    {
        foreach(var slot in bgSlots)    // bgSlots안에 있는 오브젝트들 순차적으로 처리
        {
            slot.Translate(scrollingSpeed * Time.deltaTime * -transform.right); // 초당 scrollingSpeed만큼의 속도로 왼쪽으로 이동

            if (slot.position.x < endPoint)     // 이동후에 endPoint 보다 왼쪽에 있다면
            {
                slot.Translate(width * bgSlots.Length * transform.right);       // 오른쪽 끝으로 보내기
            }
        }
    }
}
