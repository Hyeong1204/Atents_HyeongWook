using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 플레이어 이동 속도
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// 애니메이터 컴포넌트
    /// </summary>
    Animator anim;

    /// <summary>
    /// 현재 입력된 이동 방향
    /// </summary>
    Vector2 dir;

    /// <summary>
    /// 공격 이후에 복구 용도로 임시 저장해 놓은 이전 입력 방향
    /// </summary>
    Vector2 oldInputDir;

    /// <summary>
    /// 인풋 시스템용 인풋 액션맵 클래스 객체
    /// </summary>
    PlayerInputAction inputActions;

    /// <summary>
    /// 리지드바디 컴포넌트
    /// </summary>
    Rigidbody2D rigid;

    bool isMove = false;

    private void Awake()
    {
        // 컴포넌트 및 객체 찾기
        anim = GetComponent<Animator>();
        inputActions = new PlayerInputAction();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + speed * Time.fixedDeltaTime * dir);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
        inputActions.Player.Attack.performed += OnAttack;
    }


    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Move.canceled -= OnStop;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // 이동 입력이 들어왔을 때
        dir = context.ReadValue<Vector2>();     // 방향 저장
        anim.SetFloat("InputX", dir.x);         // 애니메이터 파라메터 변경
        anim.SetFloat("InputY", dir.y);
        isMove = true;
        anim.SetBool("IsMove", isMove);
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        dir = Vector2.zero;                     // 입력방향 제거
        isMove = false;
        anim.SetBool("IsMove", isMove);          // 이동 끝 표시
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        oldInputDir = dir;                      // 이후 복원을 위해 입력 방향 저장
        dir = Vector2.zero;                     // 입력 이동 방향 초기화
        anim.SetTrigger("Attack");              // 공격 애니메이션 실행
    }

    /// <summary>
    /// 공격 애니메이션 state가 끝날 때 실행되는 함수
    /// </summary>
    public void RestoreInputDir()
    {
        if (isMove == true)
        {
            dir = oldInputDir;                      // 입력 이동방향 복원
        }
    }
}
