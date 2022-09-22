using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TrapBlade : TrapBladeTrigger
{
    Transform blade;        // blade
    Transform[] wayPoint;
    int index = 0;

    Rigidbody rigid;

    public float bladeSpeed = 3.0f;    // blade 이동 속도
    //float rota = 90.0f;

    private void Start()
    {
        SetUp();
    }

    private void Awake()
    {
        blade = transform.GetChild(0);      // blade 첫번째 자식을 불러옴
        wayPoint = new Transform[transform.childCount-1];       // wayPoint를 transform의 자식의 -1 만큼의 배열을 만듬
        for (int i =0; i < wayPoint.Length; i++)
        {
            wayPoint[i] = transform.GetChild(i+1);      // wayPoint[i]의 i+1번째의 자식을 불러옴
        }
        rigid = transform.GetChild(0).GetComponent<Rigidbody>();    // rigid 컴포먼트를 불러옴
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(blade.position + bladeSpeed * Time.fixedDeltaTime * blade.forward);      // blade 자신의 앞(z) 방향을 이동
        //rigid.MoveRotation(blade.rotation * Quaternion.AngleAxis(rota * Time.fixedDeltaTime, blade.right));
    }



    void Move()
    {
        if(index < wayPoint.Length)     // index가 < 3 작을 때
        {
            index++;        // 증감식
            index %= wayPoint.Length;       // index % 3 (0~2 숫자만 나옴)
            blade.LookAt(wayPoint[index]);  // wayPoint[index]쪽으로 봐라  LookAt(그대상을 봄)
        }
    }


    void SetUp()
    {
        blade.transform.position = wayPoint[index].transform.position;      // blade가 wayPoint[0]쪽으로 즉시 이동 (처음 시작할때 나오는 함수로 무조건 0번째 지점이다.)

        StartCoroutine(OnMove());       // OnMove() 코루틴 시작
    }

    IEnumerator OnMove()
    {
        Move();
        while (true)
        {
            // Vector3.Distance(두 지점의 사이 길이) 현재 blade 위치와 wayPoint[index]위치의 거리가 0.02f * bladeSpeed(0.06) 보다 작으면
            if (Vector3.Distance(blade.position, wayPoint[index].position) < 0.02f * bladeSpeed)
            {
                Move();         //Move 함수 사용
            }
            yield return null;      // 무한 루프 (비활성화 하려면 StopCoroutine 필요)
        }
    }

   
}
