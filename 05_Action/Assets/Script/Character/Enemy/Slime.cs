using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;      // UNITY_EDITOR라는 전처리기가 설정되어있을 때만 실행버전에 넣어라
#endif

[RequireComponent(typeof(Rigidbody))]       // 필수적으로 필요한 컴포넌트가 있을 때 자동으로 넣어주는 유니티 속성
[RequireComponent(typeof(Animator))]
public class Slime : MonoBehaviour
{
    /// <summary>
    /// 적이 순찰할 웨이포인트
    /// </summary>
    public WayPoints waypoints;
    public float moveSpeed = 3.0f;      // 적의 이동 속도

    Transform wayPointTarget;               // 지금 적이 이동할 목표 지점

    EnemyState state = EnemyState.Patrol;                   // 현재 적의 상태(대기 상태 or 순찰 상태)
    public float waitTime = 1.0f;       // 목적지에 도착했을 때 기달리는 시간
    float waitTimer;                    // 남아있는 기다려야 하는 시간

    // 추적 관련 변슈 ------------------------------------------------------
    public float sightRange = 10.0f;                // 시야 범위
    public float sightHalfAngle = 50.0f;            // 시야각의 절반
    Transform chaseTarget;                          // 추적할 플레이어의 트랜스폼
    // --------------------------------------------------------------------
    Animator anima;
    NavMeshAgent agent;

    /// <summary>
    /// 적의 상태를 나타내기 위한 enum
    /// </summary>
    protected enum EnemyState
    {
        Wait = 0,       // 대기 상태
        Patrol,         // 순찰 상태
        Chase           // 추적 상태
    }

    /// <summary>
    /// 상태별 업데이트 함수를 가질 델리게이트
    /// </summary>
    Action StateUpdate;

    /// <summary>
    /// 이동할 목적지(웨이 포인트)를 나타내는 프로퍼티
    /// </summary>
    protected Transform WayPointTarget
    {
        get => wayPointTarget;
        set
        {
            wayPointTarget = value;
        }
    }

    /// <summary>
    /// 적의 상태를 나타내는 프로퍼티
    /// </summary>
    protected EnemyState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                //switch (state)      // 이전 상태(상태를 나가면서 해야할 일 처리
                //{
                //    case EnemyState.Wait:
                //        break;
                //    case EnemyState.Patrol:
                //        break;
                //    default:
                //        break;
                //}

                state = value;
                switch (state)      // 새로운 상태(새로운 상태로 들어가면서 해야할 일 처리
                {
                    case EnemyState.Wait:
                        agent.isStopped = true;
                        waitTimer = waitTime;           // 타이머 초기화
                        anima.SetTrigger("Stop");       // Idle 애니메이션 재생
                        StateUpdate = Update_Wait;      // FixedUpdate에서 실행될 델리게이트 변경
                        break;
                    case EnemyState.Patrol:
                        agent.isStopped = false;
                        agent.SetDestination(wayPointTarget.position);
                        anima.SetTrigger("Move");       // Move 애니메이션 재생
                        StateUpdate = Update_Patrol;    // FixedUpdate에서 실행될 델리게이트 변경
                        break;
                    case EnemyState.Chase:
                        agent.isStopped = false;
                        anima.SetTrigger("Move");       // Move 애니메이션 재생
                        StateUpdate = Update_Chase;     // FixedUpdate에서 실행될 델리게이트 변경
                        break;
                    default:
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 남은 대기 시간을 나타내는 프로퍼티
    /// </summary>
    protected float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if (waitTimer < 0)      // 남은 시간이 다 되면
            {
                State = EnemyState.Patrol;      // Patrol 상태로 전환
            }
        }
    }

    private void Awake()
    {
        anima = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.speed = moveSpeed;
        if (waypoints != null)      // waypoints 가 없을 때를 대비한 코드
        {
            WayPointTarget = waypoints.Current;
        }
        else
        {
            WayPointTarget = transform;
        }

        // 값 초기화 작업
        State = EnemyState.Wait;      // 기본 상태 설정(wait)
        anima.ResetTrigger("Stop");     // 트리거가 쌓이는걸 방지
    }

    private void FixedUpdate()
    {
        if (SearchPlayer())     // 매번 추적대상을 찾기
        {
            State = EnemyState.Chase;       // 추적 대상이 있으면 추적 상태로 변경
        }

        StateUpdate();
    }

    /// <summary>
    /// Patrol 상태일 떄 실행될 업데이트 함수
    /// </summary>
    void Update_Patrol()
    {
        // 도착 확인
        // agent.pathPending : 경로 계산이 진행중인지 확인. true면 아직 경로 계산중
        // agent.remainingDistance : 도착지점까지 남아있는 거리
        // agent.remainingDistance : 도착지점에 도착했다고 인정되는 거리
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)    // 경로 계산이 완료 됐고
        {
            WayPointTarget = waypoints.MoveNext();
            State = EnemyState.Wait;
        }
    }

    /// <summary>
    ///  Wiat 상태일 때 실행될 업데이트 함수
    /// </summary>
    void Update_Wait()
    {
        WaitTimer -= Time.fixedDeltaTime;       // 시간 지속적으로 감소
    }

    /// <summary>
    /// Chase 상태일 때 실행될 업데이트 함수
    /// </summary>
    void Update_Chase()
    {
        if (chaseTarget != null)        // 추적 대상이 있는지 확인
        {
            agent.SetDestination(chaseTarget.position);     // 추적 대상이 있으면 추적 대상의 위치로 이동
        }
        else
        {
            State = EnemyState.Wait;            // 추적 대상이 없으면 잠시 대기
        }
    }

    /// <summary>
    /// 플레이어를 감지하는 함수
    /// </summary>
    /// <returns>적이 플레이어를 감지하면 true, 아니면 false</returns>
    bool SearchPlayer()
    {
        bool result = false;
        chaseTarget = null;

        // 특정 범위안에 존재하는지 확인
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, LayerMask.GetMask("Player"));
        if(colliders.Length > 0)
        {
            // player가 몬스터 주변에 있다.
            Vector3 playerPos = colliders[0].transform.position;            // 플레이어 위치
            Vector3 toPlayerDir = playerPos - transform.position;           // 플레이어로 가는 방향
           
            if (IsInSightAngle(toPlayerDir))
            {
                // player가 시야각 안에 들어왔다.
                if (!IsSightBlocked(toPlayerDir))
                {
                    // 시야가 다른 물체로 인해 막히이 않았다.
                    chaseTarget = colliders[0].transform;   // 추적할 플레이어 저장
                    result = true;
                }                
            }
        }
        return result;
    }

    /// <summary>
    /// 대상이 시야각안에 들어와 있는지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDir">대상으로 가는 방향 벡터</param>
    /// <returns>대상이 있다면 true</returns>
    bool IsInSightAngle(Vector3 toTargetDir)
    {
        float angle = Vector3.Angle(transform.forward, toTargetDir);    // forward 벡터와 플레이어로 가는 방향 벡터의 시야각 구하기
        return sightHalfAngle > angle;
    }

    /// <summary>
    /// 플레이어를 바라보는 시야가 막혔는지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDir">대상으로 가는 방향 벡터</param>
    /// <returns>방해하는 물건이 없다면 false</returns>
    bool IsSightBlocked(Vector3 toTargetDir)
    {
        bool result = true;
        // 레이 만들기 : 시점점 = 적의 위치 + 적의 눈높이, 방항 = 적에서 플레이어로 가는 방향
        Ray ray = new(transform.position + transform.up * 0.5f, toTargetDir);
        if (Physics.Raycast(ray, out RaycastHit hit, sightRange))
        {
            // 레이에 부딪친 컬라이더가 있다.
            if (hit.collider.CompareTag("Player"))
            {
                // 컬라이더가 player면
                result = false;
            }
        }
        return result;
    }

    public void Test()
    {
        SearchPlayer();
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        //Gizmos.DrawWireSphere(transform.position, sightRange);
        Vector3 forward = transform.forward * sightRange;

        Handles.DrawLine(transform.position, transform.position + forward);         // 몬스터의 앞 방향

        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);         // 시야 반경만큼 원 그리기

        if (SearchPlayer())
        {
            Handles.color = Color.red;
        }

        Quaternion q1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up);        // up벡터를 축으로 반시계방향으로 회전
        Quaternion q2 = Quaternion.AngleAxis(sightHalfAngle, transform.up);         // up벡터를 축으로 시계방향으로 회전

        Handles.DrawLine(transform.position, transform.position + q1 * forward, 5.0f);    // 중심선을 반시계방향으로 회전
        Handles.DrawLine(transform.position, transform.position + q2 * forward, 5.0f);    // 중심선을 시계방향으로 회전

        Handles.DrawWireArc(transform.position, transform.up, q1 * forward, sightHalfAngle * 2, sightRange, 5.0f);
#endif
    }
}
