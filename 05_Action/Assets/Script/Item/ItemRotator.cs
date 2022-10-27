using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트를가진 오브젝트는 y축을 기준으로 계속 회전(시계 방향)하면서 위아래로 올라갔다 내려갔다 한다(삼각함수 활용).
public class ItemRotator : MonoBehaviour
{
    public float rotateSpeed;       // 오브젝트의 회전 속도
    public float minHeight;         // 오브젝트의 가장 낮은 높이
    public float maxHeight;         // 오브젝트의 가장 높은 높이
    //public float moveSpeed;         // y축 기준으로 위래 왕복운동 속도

    float runningTime;              // 플레이 시작시간
    float halfDiff;

    Vector3 newPosition;

    private void Start()
    {
        newPosition = transform.position;
        newPosition.y = minHeight;
        transform.position = newPosition;
        runningTime = 0.0f;
        halfDiff = 0.5f * (maxHeight - minHeight);
    }

    private void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotateSpeed * 1.0f, 0);        // 시계방향으로 회전

        runningTime += Time.deltaTime;
        newPosition.y = minHeight + (1 - Mathf.Cos(runningTime)) * halfDiff;
        transform.position = newPosition;
        //runningTime += Time.deltaTime * moveSpeed;                          // 1초에 1 x moveSpeed 만큼 증가
        //yPos = Mathf.Clamp(Mathf.Sin(runningTime), minHeight, maxHeight);   // 사인 함수를 이용하여 -1 ~ 1 사이 값이 나오고 Clamp 함수를 이용하여 최소 높이와 최대높이를 만든다.
        //transform.Translate(Time.deltaTime * yPos * Vector3.up);            // y축 기준으로 yPos값만큼 이동
    }
}