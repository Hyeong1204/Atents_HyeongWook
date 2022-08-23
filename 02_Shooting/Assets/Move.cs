using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 1.0f;

    // 유니티 이벤트 함수 : 유니티가 특정 타이밍에 실행 시키는 함수

    /// <summary>
    /// Start 함수. 게임이 시작될 때(첫번째 update 함수가 호출되는 직전에 호출되는 함수) 호출되는 함수
    /// </summary>
    private void Start()
    {
        Debug.Log("Hollo unity");
    }
     

    /// <summary>
    /// Update 함수. 매 프레임마다 호쵤되는 함수. 지속으로 변경되는 것이 있을 때 사용하는 함수.
    /// </summary>
    private void Update()
    {
        // Vector3 : 백터를 표현하기 위한 구조체. 위치를 표현할 때도 많이 사용한다.
        // 백터 : 힘의 방향과 크기를 나타내는 단위

        //transform.position += (Vector3.left * speed);   // 아래쪽 방향으로 speed 만큼 움직여라(매 프레임마다)
        //transform.position += new Vector3(1, 0, 0); 
        //new Vector3(0, 1, 0);   // 위쪽
        //new Vector3(0, -1, 0);   // 아래쪽

        //  Time.deltaTime : 이전 프레임에서 지금 프레임까지 걸린 시간
        transform.position += (speed * Time.deltaTime * Vector3.left);  // 아래쪽 방향으로 speed 만큼 움직여라(매 초마다)
    }
}
