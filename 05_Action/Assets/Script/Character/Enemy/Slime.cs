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

    Transform moveTarget;               // 지금 적이 이동할 목표 지점

    EnemyState state;                   // 현재 적의 상태(대기 상태 or 순찰 상태)
    public float waitTime = 1.0f;       // 목적지에 도착했을 때 기달리는 시간
    float waitTimer;                    // 남아있는 기다려야 하는 시간

    public float sightRange = 10.0f;

    Animator anima;
    NavMeshAgent agent;

    /// <summary>
    /// 적의 상태를 나타내기 위한 enum
    /// </summary>
    protected enum EnemyState
    {
        Wait = 0,       // 대기 상태
        Patrol          // 순찰 상태
    }

    /// <summary>
    /// 상태별 업데이트 함수를 가질 델리게이트
    /// </summary>
    Action StateUpdate;

    /// <summary>
    /// 이동할 목적지를 나타내는 프로퍼티
    /// </summary>
    protected Transform MoveTarget
    {
        get => moveTarget;
        set
        {
            moveTarget = value;
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
                    agent.SetDestination(moveTarget.position);
                    anima.SetTrigger("Move");       // Move 애니메이션 재생
                    StateUpdate = Update_Patrol;    // FixedUpdate에서 실행될 델리게이트 변경
                    break;
                default:
                    break;
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
            MoveTarget = waypoints.Current;
        }
        else
        {
            MoveTarget = transform;
        }

        // 값 초기화 작업
        State = EnemyState.Wait;      // 기본 상태 설정(wait)
        anima.ResetTrigger("Stop");     // 트리거가 쌓이는걸 방지
    }

    private void FixedUpdate()
    {
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
            MoveTarget = waypoints.MoveNext();
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

    bool SearchPlayer()
    {
        bool result = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, LayerMask.GetMask("Player"));
        if(colliders.Length > 0)
        {
            result = true;
        }
        else
        {
            result = false;
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
        Handles.color = Color.red;
        Handles.DrawLine(transform.position, transform.position + transform.forward * sightRange);

        Handles.color = Color.green;
        if (SearchPlayer())
        {
            Handles.color = Color.red;
        }
        Handles.DrawWireDisc(transform.position, transform.up, sightRange);
#endif
    }
}
