using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bird : MonoBehaviour
{
    /// <summary>
    /// InputSystem용 에셋 함수
    /// </summary>
    BirdInputAction input;
    Rigidbody2D rigid;
    /// <summary>
    /// 점프력 
    /// </summary>
    [Range(1.0f, 10.0f)]
    public float jumpPower = 5.0f;

    /// <summary>
    /// 위아래 회전 최대값
    /// </summary>
    float pitchMaxAngle = 45.0f;        // 최대 각도(Bird에 최대 각도)

    /// <summary>
    /// 새가 죽었는지 살았는지 표시하는 값
    /// </summary>
    bool isDead = false;

    /// <summary>
    /// 새가 죽었는지 살았는지 확인하는 프로퍼티
    /// </summary>
    public bool IsDead
    {
        get => isDead;
    }

    /// <summary>
    /// 죽었을 때 실행될 델리게이트
    /// </summary>
    public Action onDead;

    // 유니티 이벤트 함수 ---------------------------------------------------------------------------

    private void Awake()
    {
        input = new BirdInputAction();          // inputAction 객체 생성
        rigid = GetComponent<Rigidbody2D>();    // 리지드바디 컴포넌트 찾기
    }
    private void Start()
    {
        isDead = false;         // 살아있다고 표시
    }

    private void OnEnable()     // 오브젝트가 활성화 될 때
    {
        input.Bird.Enable();                    // 인풋 액션 활성화
        input.Bird.Space.started += OnJump;     // 점프 액션과 OnJump 함수 연결
        input.Bird.MouseLeft.started += OnJump;
    }


    private void OnDisable()    // 오브젝트가 비활성화 될 때
    {
        input.Bird.MouseLeft.started -= OnJump;
        input.Bird.Space.started -= OnJump;     // 점프 액션과 OnJump 함수 연결 해제
        input.Bird.Disable();                   // 인풋 액션 비활성화
    }
    

    private void FixedUpdate()      // 물리 업데이트 주기 마다(고정된 신간)
    {
        if (!isDead)        // 살아있을 때만 아래 코드 실행
        {
            float veloictiyY = Mathf.Clamp(rigid.velocity.y, -jumpPower, jumpPower);    // jumpPower만큼 최대/최소값 지정
            float angle = (veloictiyY / jumpPower) * pitchMaxAngle;     // 올라가거나 내려가는 정도에 따라 각도 설정

            rigid.MoveRotation(angle);      // 설정된 각도로 회전
        }
    }

    // 내부 함수 ------------------------------------------------------------------------------------

    private void OnCollisionEnter2D(Collision2D collision)      // 다른 컬라이더와 충돌했을 때 실행
    {
        Die(collision.GetContact(0));   // 충돌한 지점에 대한 정보 전달
    }


    void Die(ContactPoint2D contact)
    {

        Vector2 dir = (contact.point - (Vector2)transform.position).normalized;     // 새가 충돌 지점으로 가는 방향
        Vector2 reflect = Vector2.Reflect(dir, contact.normal);     // dir이 반사된 백터
        rigid.velocity = reflect * 10.0f + Vector2.left * 5.0f;     // 반사되는 방향에 왼쪽으로 가는 힘 추가
        rigid.angularVelocity = 1000.0f;        // 1초의 1000도 도는 회전 추가

        if (!isDead)    // 처음 죽었을 때만 처리하는 코드
        {
            // 아직 살아있다고 표시될 때만 실행
            onDead?.Invoke();       // 사망 알림용 델리게이트 실행
            input.Bird.Disable();   // 죽었을 때 한번만 입력 막기
            isDead = true;          // 죽었다고 표시
        }
    }

    /// <summary>
    /// 점프 입력이 들어왔을 때 실행되는 함수
    /// </summary>
    /// <param name="_">사용 안함</param>
    private void OnJump(InputAction.CallbackContext _)
    {
        rigid.velocity = Vector2.up * jumpPower;    // 위쪽 방향으로 점프력만큼 velocity 변경
    }
}
