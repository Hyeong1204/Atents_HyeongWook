using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    // 길찾기 관련 변수들 --------------------------------------------------------------------------------
    public Test_TlimemapAStarSlime test;        // 이 후에 반드시 삭제할 코드.

    /// <summary>
    /// 슬라임의 이동속도
    /// </summary>
    public float moveSpeed = 2.0f;

    /// <summary>
    /// 경로 표시 여부. true면 보이고 false면 안보인다. (테스트 편의를 위한 것)
    /// </summary>
    public bool isShowPath = true;

    /// <summary>
    /// 길 찾기를 수행할 그리드맵
    /// </summary>
    GridMap map;

    /// <summary>
    /// 슬라임이 이동 해야할 경로
    /// </summary>
    List<Vector2Int> path;

    /// <summary>
    /// 이 슬라임의 경로를 그리기 위한 변수
    /// </summary>
    PathLineDraw pathLine;

    /// <summary>
    /// 이 슬라임의 그리드 좌표를 확인하기 위한 프로퍼티
    /// </summary>
    Vector2Int Position => map.WorldToGrid(transform.position);

    // 쉐이더 관련 변수들 --------------------------------------------------------------------------------
    /// <summary>
    /// 페이즈 진행 되는 시간(스폰 후 딜레이)
    /// </summary>
    public float phaseDuration = 0.5f;

    /// <summary>
    /// 디졸브가 진행되는 시간(사망 이후에 실제로 사라질 때가지의 시간)
    /// </summary>
    public float dissolveDuration = 1.0f;

    /// <summary>
    /// 아웃라인의 두깨
    /// </summary>
    const float Outline_Thickness = 0.005f;

    /// <summary>
    /// 쉐이더의 프로퍼티에 접근을 하기 위한 머티리얼
    /// </summary>
    Material mainMaterial;

    // 델리게이트 ----------------------------------------------------------------------------------------
    /// <summary>
    /// 페이즈가 끝났을 때 실행될 델리게이트
    /// </summary>
    public Action onPhaseEnd;

    /// <summary>
    /// 슬라임이 죽었을 때 실행될 델리게이트
    /// </summary>
    public Action onDie;

    /// <summary>
    /// 이 오브젝트가 비활성화 될 때(디졸브가 다 끝난 이 후에) 실행될 델리게이트
    /// </summary>
    public Action onDisable;

    /// <summary>
    /// 슬라임이 목적지에 도착 했을 때 실행되는 델리게이트
    /// </summary>
    public Action onGoalArrive;

    // 함수들 -----------------------------------------------------------------------------------------------------
    private void Awake()
    {
        Renderer renderer = GetComponent<SpriteRenderer>();
        mainMaterial = renderer.material;           // 머티리얼 미리 찾아 놓기
        pathLine = GetComponentInChildren<PathLineDraw>();

        path = new List<Vector2Int>();
    }

    private void OnEnable()
    {
        // 쉐이더 프로퍼티 값들 초기화
        ShowOutLine(false);                              // 아웃라인 끄기
        mainMaterial.SetFloat("_Dissolve_Fade", 1.0f);   // 디졸브 안된 상태로 만들기
        StartCoroutine(StartPhase());                    // 페이즈 시작
    }

    private void OnDisable()
    {
        onDisable?.Invoke();                              // 비활성화 되었다고 알림(레디 큐에 다시 돌려주라는 신호를 보내는 것을 주 용도)
    }

    private void Start()
    {
        map = test.Map;                                    // 맵 받아오기(수정 되어야할 코드)
        onGoalArrive += () =>
        {
            Vector2Int pos = map.GetRandomMoveable();
            SetDestination(pos);
        };
        pathLine.gameObject.SetActive(isShowPath);         // isShowPath에 따라 경로 활성화/비활성화 설정
        pathLine.transform.SetParent(pathLine.transform.parent.parent);                  // 부모를 슬라임의 부모로
    }

    private void Update()
    {
        if (path.Count > 0)                                     // path에 위치가 기록 되어 있으면 진행
        {
            Vector3 dest = map.GridToWorld(path[0]);            // path의 첫번째 위치로 항상 이동
            // 목적 방향으로 (Time.deltaTime * moveSpeed)만큼 이동하기 
            Vector3 dir = dest - transform.position;          // 방향 계산
            transform.Translate(Time.deltaTime * moveSpeed * dir.normalized);       // 계산한 방향으로 1초에 moveSpeed만큼 이동

            if (dir.sqrMagnitude < 0.001f)                       // 목적지(path의 첫번째 위치)에 도착 했는지 확인
            {
                path.RemoveAt(0);                               // 목적지에 도착 했으면 그 노드를 제거
            }
        }
        else
        {
            onGoalArrive?.Invoke();
        }
    }

    /// <summary>
    /// 이 슬라임을 죽일 때 실행할 함수
    /// </summary>
    public void Die()
    {
        StartCoroutine(StartDissolve());        // 디졸브 실행
        onDie?.Invoke();                        // 죽었다고 신호보내기
    }

    /// <summary>
    /// 페이즈를 실행하기 위한 델리게이트
    /// </summary>
    /// <returns></returns>
    IEnumerator StartPhase()
    {
        mainMaterial.SetFloat("_Phase_Thickness", 0.1f);            // 페이즈 진행 시 나올 중간 선의 두께 지정(보이게 만들기);
        mainMaterial.SetFloat("_Phase_Split", 0.0f);                // 페이즈 선의 위치 초기화

        float timeElipsed = 0.0f;                                   // 시간 초기화
        float phaseDuationNoramlize = 1 / phaseDuration;            // split의 범위는 0~1이기 때문에 시간을 0~1 사이의 범위로 정규화 시키기 위한 용도

        while (timeElipsed < phaseDuration)                         // 측정한 시간이 목표시간에 도달할 때 까지 반복
        {
            timeElipsed += Time.deltaTime;                          // 측정 시간을 프레임별로 누적시키기

            mainMaterial.SetFloat("_Phase_Split", timeElipsed * phaseDuationNoramlize);     // 측정 시간을 정규화 시겨 split 값으로 설정
            yield return null;      // 다음 프레임까지 기달리기
        }

        mainMaterial.SetFloat("_Phase_Split", 1.0f);                // slite을 최대치로 변경
        mainMaterial.SetFloat("_Phase_Thickness", 0.0f);            // 페이즈 진행 시 나올 중간 선의 두께 지정(안 보이게 만들기)
        onPhaseEnd?.Invoke();                                       // 페이즈가 끝났다고 알림
    }

    /// <summary>
    /// 디졸브를 실행하기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDissolve()
    {
        mainMaterial.SetFloat("_Dissolve_Fade", 1.0f);          // 디졸브 진행 정도 초기화(디졸브가 전혀 진행되지 않게 설정)

        float timeElipsed = 0.0f;                               // 측정 시간 초기화
        float phaseDuationNoramlize = 1 / dissolveDuration;     // Fade의 범위는 0~1이기 때문에 시간을 0~1 사이의 범위로 정규화 시키기 위한 용도

        while (timeElipsed < dissolveDuration)                  // 측정  시간이 목표시간에 도달할 때 까지 반복
        {
            timeElipsed += Time.deltaTime;                      // 측정 시간을 프레임별로 누적 시키기

            mainMaterial.SetFloat("_Dissolve_Fade", 1 - timeElipsed * phaseDuationNoramlize);       // 측정 시간을 정규화시켜 (1 - 정규화 
            yield return null;                                  // 다음 프레임까지 대기
        }

        this.gameObject.SetActive(false);                       // 게임 오브젝트 비활성화(오브젝트 풀로 되돌리기)
    }

    /// <summary>
    /// 아웃라인 표시용 함수
    /// </summary>
    /// <param name="isShow">true면 아웃라인 표시, false면 아웃라인 끄기</param>
    public void ShowOutLine(bool isShow)
    {
        if (isShow)
        {
            mainMaterial.SetFloat("_OutLine_Thickness", Outline_Thickness);     // 아웃라인 두께 지정해서 보이게 만들기
        }
        else
        {
            mainMaterial.SetFloat("_OutLine_Thickness", 0.0f);                  // 아웃라인의 두께를 제거해서 안보이게 만들기
        }
    }

    /// <summary>
    /// 슬라임이 이동할 목적지 설정
    /// </summary>
    /// <param name="goal">목적지의 그리드 좌표</param>
    public void SetDestination(Vector2Int goal)
    {
        path = AStar.PathFind(map, Position, goal);     // 경로 계산해서 받아옴
        pathLine.DrawPath(map, path);                   // 경로 그리기
    }
}