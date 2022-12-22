using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 플레이어 이동 속도
    /// </summary>
    public float speed = 3.0f;
    public float attackCoolTime = 1.0f;

    /// <summary>
    /// 플레이어의 현재 남아 있는 쿨타임
    /// </summary>
    float currentAttackCoolTime = 0.0f;

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

    /// <summary>
    /// 공겨 영역의 중심(축)
    /// </summary>
    Transform attackAreaCenter;

    /// <summary>
    /// 플레이어가 공격하면 죽을 슬라임들
    /// </summary>
    List<Slime> attackTarget;

    /// <summary>
    /// 현재 이동 가능 여부
    /// </summary>
    bool isMove = false;

    /// <summary>
    /// 공격 유효기간 표시. true면 슬라임을 죽일 수 있다. false면 못 죽이든 상황
    /// </summary>
    bool isAttackValid = false;

    private void Awake()
    {
        // 컴포넌트 및 객체 찾기
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        inputActions = new PlayerInputAction();


        attackTarget = new List<Slime>(2);          // 공격 대상 저장용 리슽트 생성
        attackAreaCenter = transform.GetChild(0);   // 공격 지점의 중심점 찾기
        AttackArea attackArea = attackAreaCenter.GetComponentInChildren<AttackArea>();      // 공격 중심점 찾기
        attackArea.onTarget += (slime) =>
        {
            // slime이 공격 지점안에 들어왔을 때의 처리
            attackTarget.Add(slime);        // 리스트에 추가
            slime.ShowOutLine(true);        // 아웃라인 표시
        };
        attackArea.onTargetRelease += (slime) =>
        {
            // slime이 공격 지점 박으로 나갔을 때의 처리
            attackTarget.Remove(slime);     // 리스트에서 제거
            slime.ShowOutLine(false);       // 아웃라인 끄기
        };
    }

    private void Update()
    {
        currentAttackCoolTime -= Time.deltaTime;        // 아무 조건 없이 계속 감소

        if (isAttackValid && attackTarget.Count > 0)
        {
            foreach (var target in attackTarget)
            {
                target.Die();
            }
        }
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
        oldInputDir = dir;                      // 이후 복원을 위해 입력 방향 저장
        anim.SetFloat("InputX", dir.x);         // 애니메이터 파라메터 변경
        anim.SetFloat("InputY", dir.y);
        isMove = true;                          // 이동 한다고 표시하고
        anim.SetBool("IsMove", isMove);         // 이동 애니메이션 재생

        // 입력 방향에 따라 공격지점 위치 변경(중심축 회전으로 처리)
        float attackAngle = 0.0f;
        if (dir.y > 0)
        {
            // 위로 갈 때(위, 아래가 우선 순위가 높음)
            attackAngle = 180.0f;
        }
        else if (dir.y < 0)
        {
            // 아래로 갈 때
            attackAngle = 0.0f;
        }
        else if (dir.x > 0)
        {
            // 오른쪽으로 갈 때
            attackAngle = 90.0f;
        }
        else if (dir.x < 0)
        {
            // 왼쪽으로 갈 때
            attackAngle = 270.0f;
        }
        else
        {
            // 있을 수 없음. 하지만 혹시 일어날 수도 있기 때문에 추가
            attackAngle = 0.0f;
        }

        attackAreaCenter.rotation = Quaternion.Euler(0, 0, attackAngle);
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        dir = Vector2.zero;                     // 입력방향 제거
        isMove = false;
        anim.SetBool("IsMove", isMove);          // 이동 끝 표시
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        if (currentAttackCoolTime < 0)
        {
            oldInputDir = dir;                      // 이후 복원을 위해 입력 방향 저장
            dir = Vector2.zero;                     // 입력 이동 방향 초기화
            anim.SetTrigger("Attack");              // 공격 애니메이션 실행 

            currentAttackCoolTime = attackCoolTime;     // 쿨타임 리셋
        }
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

    /// <summary>
    /// 공격이 효과적으로 보일 때 실행되는 함수
    /// </summary>
    public void AttackValid()
    {
        isAttackValid = true;
    }

    /// <summary>
    /// 공격이 효과적으로 보이는 기간이 끝날 때 싱핼될 함수
    /// </summary>
    public void AttackNotvalid()
    {
        isAttackValid = false;
    }
}
