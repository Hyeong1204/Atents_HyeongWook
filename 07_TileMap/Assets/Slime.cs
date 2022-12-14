using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    // 일반 변수 ----------------------------------------------------------------------------------------

    public float bonusLife = 2.0f;
    bool isActivate = false;

    /// <summary>
    /// 이 슬라임의 그리드 좌표를 확인하기 위한 프로퍼티
    /// </summary>
    Vector2Int Position => map.WorldToGrid(transform.position);

    // 길찾기 관련 변수들 --------------------------------------------------------------------------------

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

    public PathLineDraw PathLine => pathLine;

    /// <summary>
    /// 다른 슬라임에 의해 경로가 막혀서 기달린 시간
    /// </summary>
    float pathWaitTime = 0.0f;

    /// <summary>
    /// 경로가 막혓을 때 최대로 기다리는 시간
    /// </summary>
    const float MaxWaitTime = 1.0f;

    /// <summary>
    /// 이 슬라임이 현재 위치하고 있는 노드
    /// </summary>
    Node currentNode;

    //Collider2D bodyCollider;

    Node CurrentNode
    {
        get => currentNode;             // 노드 위치가 바뀔 때
        set
        {
            if (value != currentNode)
            {
                if (currentNode != null)        // 이전에 위치하던 노드가 있으면
                {
                    currentNode.gridType = Node.GridType.Plain;     // 노드 타입을 Monter -> Plain으로 변경 
                }

                currentNode = value;                                // 새로 노드  설정
                currentNode.gridType = Node.GridType.Monster;       // 노드 타임을 Monster로 변경

                spriteRenderer.sortingOrder = -currentNode.y;       // order in layer 변경. 아래쪽에 있는 것이 위에 그려지도록 변경
            }
        }
    }

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

    /// <summary>
    /// order in layer를 변경하기 위한 스프라이트 랜더러
    /// </summary>
    SpriteRenderer spriteRenderer;

    Collider2D bodyCollider;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainMaterial = spriteRenderer.material;           // 머티리얼 미리 찾아 놓기
        pathLine = GetComponentInChildren<PathLineDraw>();
        bodyCollider = GetComponent<Collider2D>();

        path = new List<Vector2Int>();

    }

    private void OnEnable()
    {
        onDie = () => isActivate = false;               // 페이즈가 끝나면 활성화
        onPhaseEnd = () =>
        {
            isActivate = true;                          // 페이즈가 끝나면 활성화
            bodyCollider.enabled = true;                // 컬라이더 켜기(페이즈가 끝나기 전까지 무적)
        };

        pathLine.gameObject.SetActive(isShowPath);      // isShowPath에 따라 경로 활성화/비활성화 설정

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
        onGoalArrive += () =>
        {
            //Vector2Int pos = map.GetRandomMoveable();       // 현재 내 위치를 기록
            //while (pos == Position)                         // pos가 내 위치면 계속 반복 => 내 위치와 다른 위치가 나올 때까지 반복
            //{
            //    pos = map.GetRandomMoveable();              // 맵에서 이동 가능한 위치를 랜덤으로 가져오기
            //}
            Vector2Int pos;
            do
            {
                pos = map.GetRandomMoveable();
            } while (pos == Position);

            SetDestination(pos);                            // 랜덤으로 가져온 위치로 이동하기
        };
    }

    private void Update()
    {
        MoveUpdate();
    }

    /// <summary>
    /// 슬라임 초기화용 함수. 슬라임이 맵에 나타날 때 실행되어야 함
    /// </summary>
    /// <param name="grid">슬라임이 존재하는 맵</param>
    /// <param name="pos">슬라임의 위치</param>
    public void Initialize(GridMap grid, Vector3 pos)
    {
        map = grid;
        transform.position = pos;
        CurrentNode = map.GetNode(pos);         // 이 슬라임이 위치한 노드 가지고 있기
    }

    /// <summary>
    /// 슬라임 이동처리하는 함수 (매 업데이트마다 실행)
    /// </summary>
    private void MoveUpdate()
    {
        if (isActivate)                                             // 활성화 되면 실행
        {
            if (path != null && path.Count > 0 && pathWaitTime < MaxWaitTime)       // 정해진 경로가 있고 && 다음 경로가 있고 && 기다려도 되는 시간이 남아 있을 때
            {
                Vector2Int destGrid = path[0];
                if (!map.IsMoster(destGrid) || CurrentNode == map.GetNode(destGrid))
                {
                    // path[0]에 몬스터가 없거나(다음에 이동할 칸에 다른 몬스터가 없는 경우)
                    // path[0]에 내가 있다.(내가 다음 도착지점 칸에는 있는데 아직 한 가운데까지 못 간 경우)
                    // 이동처리
                    Vector3 dest = map.GridToWorld(destGrid);            // destGrid의 첫번째 위치로 항상 이동
                                                                         // 목적 방향으로 (Time.deltaTime * moveSpeed)만큼 이동하기 
                    Vector3 dir = dest - transform.position;            // 방향 계산

                    transform.Translate(Time.deltaTime * moveSpeed * dir.normalized);                 // 목적지 방향으로  속도에 맞춰서 이동
                    CurrentNode = map.GetNode(transform.position);       // 현재 노드 변경 (그리드 타입 변경)

                    if (dir.sqrMagnitude < 0.001f)                       // 목적지(path의 첫번째 위치)에 도착 했는지 확인
                    {
                        transform.position = dest;                      // 목적지의 정확한 위치에 설정
                        path.RemoveAt(0);                               // 목적지에 도착 했으면 그 노드를 제거
                    }

                    pathWaitTime = 0.0f;
                }
                else
                {
                    // destGrid에 몬스터가 있고, destGrid에 내가 없다.
                    //Vector2Int back = new Vector2Int(currentNode.x, currentNode.y);
                    //path.Insert(0, back);
                    // 기다리기
                    pathWaitTime += Time.deltaTime;
                }
            }
            else
            {
                pathWaitTime = 0.0f;
                onGoalArrive?.Invoke();
            }
        }
    }

    /// <summary>
    /// 이 슬라임을 죽일 때 실행할 함수
    /// </summary>
    public float Die()
    {
        float bonus = 0.0f;

        bodyCollider.enabled = false;               // 더 이상 충돌 못 하게 막기
        if (isActivate)
        {
            // 슬라임을 저게하기 위한 처리를 수행
            ClearData();

            StartCoroutine(StartDissolve());        // 디졸브 실행
            onDie?.Invoke();                        // 죽었다고 신호보내기
            bonus = bonusLife;
        }
        return bonus;
    }

    /// <summary>
    /// 슬라임 없어져야 하는 상황일 때 해야하는 처리를 수행하는 함수
    /// </summary>
    public void ClearData()
    {
        transform.SetParent(SlimeFactory.Inst.gameObject.transform);     // 슬라임을 다시 팩토리의 자식으로

        // 움직이던 경로 삭제
        path.Clear();                           // 재활용 되었을 때 이전 경로를 찾아가던 문제를 수정하기 위해 추가
        pathLine.ClearPath();                   // 재활용 된 직후에 이전 경로가 보이던 것을 수정하기 위해 추가
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

        CurrentNode.gridType = Node.GridType.Plain;             // 슬라임이 죽었으니 다시 지나갈 수 있게 만들기

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
