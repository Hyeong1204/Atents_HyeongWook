using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // 수명 관련 변수들 ------------------------------------------------------------------

    /// <summary>
    /// 플레이어의 최대 수명
    /// </summary>
    public float maxLifeTime = 10.0f;

    /// <summary>
    /// 플레이어의 현재 수명
    /// </summary>
    float lifeTime;

    /// <summary>
    /// 전체 플레이 시간
    /// </summary>
    float totalPlayTime;

    /// <summary>
    /// 플레이어의 생존 여부 표시용
    /// </summary>
    bool isDead = false;

    /// <summary>
    /// 플레이어의 수명을 확인하거나 설정할 때의 여러 처리를 하는 프로퍼티
    /// </summary>
    public float LifeTime
    {
        get => lifeTime;
        set
        {
            lifeTime = value;                   // 우선 값 변경
            if (lifeTime < 0.0f && !isDead)      // 아직 안죽었는데 수명이 0 이하면
            {
                // 사망처리 용
                Die();                          // 사망 처리
                onLifeTimeChange?.Invoke(lifeTime, maxLifeTime);     // 수명이 변경 되었다고 알림(죽은 직후에 한번만 실행)
            }
            else
            {
                lifeTime = Math.Clamp(value, 0.0f, maxLifeTime);     // 아니면 수명을 (0 ~ 최대값)으로 설정
                onLifeTimeChange?.Invoke(lifeTime, maxLifeTime);     // 수명이 변경 되었다고 알림
            }

        }
    }

    /// <summary>
    /// 플레이어의 수명이 변경 되었을 때 실행될 델리게이트
    /// 비네트, 슬라이더, 남은 시간 표시용으로 사용
    /// </summary>
    public Action<float, float> onLifeTimeChange;         // 비네트, 슬라이더, 남은 시간

    /// <summary>
    /// 플레이어가 죽었을 때 실행될 델리게이트
    /// </summary>
    public Action onDie;

    // 이동 관련 변수들 ------------------------------------------------------------------

    /// <summary>
    /// 플레이어 이동 속도
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// 현재 입력된 이동 방향
    /// </summary>
    Vector2 dir;

    /// <summary>
    /// 공격 이후에 복구 용도로 임시 저장해 놓은 이전 입력 방향
    /// </summary>
    Vector2 oldInputDir;

    /// <summary>
    /// 현재 이동 가능 여부
    /// </summary>
    bool isMove = false;

    // 공격 관련 변수들 ------------------------------------------------------------------

    public float attackCoolTime = 1.0f;

    /// <summary>
    /// 플레이어의 현재 남아 있는 쿨타임
    /// </summary>
    float currentAttackCoolTime = 0.0f;

    /// <summary>
    /// 플레이어가 공격하면 죽을 슬라임들
    /// </summary>
    List<Slime> attackTarget;

    /// <summary>
    /// 공격 유효기간 표시. true면 슬라임을 죽일 수 있다. false면 못 죽이든 상황
    /// </summary>
    bool isAttackValid = false;

    // 기타 변수들 -------------------------------------------------------------------------

    /// <summary>
    /// 애니메이터 컴포넌트
    /// </summary>
    Animator anim;

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

    MapManager mapManager;

    /// <summary>
    /// 현재 위치하고 있는 맵(의 그리드 좌표)
    /// </summary>
    Vector2Int currentMap;

    /// <summary>
    /// 현재 위치하고 있는 맵을 확인하고 변겨할 수 있는 프로퍼티
    /// </summary>
    Vector2Int CurrentMap
    {
        get => currentMap;
        set
        {
            if (value != currentMap)                    // 맵이 변경이 될 때만
            {
                currentMap = value;                    // 실제로 변경
                onMapMoved?.Invoke(currentMap);        // 델리게이트로 변경 되었음을 알림
            }
        }
    }

    /// <summary>
    /// 맵 변경시 실행될 델리게이트 (파라메터는 변경된 맵의 그리드 좌표)
    /// </summary>
    public Action<Vector2Int> onMapMoved;

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

    private void Start()
    {
        mapManager = GameManager.Inst.MapManager;       // 맵매니저 가져오기
        lifeTime = maxLifeTime;
    }

    private void Update()
    {
        if (!isDead)
        {
            totalPlayTime += Time.deltaTime;
            LifeTime -= Time.deltaTime;
        }

        currentAttackCoolTime -= Time.deltaTime;        // 아무 조건 없이 계속 감소 && 공격 쿨타임 감소

        if (isAttackValid)
        {
            while (attackTarget.Count > 0)              // 공격 대상이 있으면 다 죽이기
            {
                attackTarget[0].Die();                  // Die가 실행되면 컬라이더가 비활성와 되면서 attackTarget에서 자동으로 제거됨
            }
        }
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + speed * Time.fixedDeltaTime * dir);     // 이동처리
        if (mapManager != null)
            CurrentMap = mapManager.WorldToGrid(transform.position);               // 이동후에 어떤 맵에 있는지 표시
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

    /// <summary>
    /// 타임 오버로 사망시 실행될 함수
    /// </summary>
    void Die()
    {
        LifeTime = 0.0f;            // 비네트, 슬라이더, 남은시간을 깔끔하게 표시하기 위해 0으로 설정
        isDead = true;              // 여러번호출되지 않도록하기 하기 위치 설정
        onDie?.Invoke();            // 죽었다고 알리기
    }
}
