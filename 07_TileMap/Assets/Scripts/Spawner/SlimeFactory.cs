using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 슬랑림 생성 전용 클래스. 팩토리 패턴과 싱글톤 패턴 적용
/// </summary>
public class SlimeFactory : Singleton<SlimeFactory>
{
    /// <summary>
    /// 생성할 슬라임 프리펩
    /// </summary>
    public GameObject slimePrefab;

    /// <summary>
    /// 처음에 생성할 슬라임의 갯수. 한번에 나올 수 있는 슬라임의 최대 수보다 크게 하는 것이 좋다.
    /// </summary>
    public int poolSize = 128;

    /// <summary>
    /// 생성한 슬라임을 저장할 배열
    /// </summary>
    Slime[] pool;

    /// <summary>
    /// 지금 사용할 수 있는 슬라임들의 큐
    /// </summary>
    Queue<Slime> readyQueue;

    /// <summary>
    /// 모든 슬라임의 pahtLine의 부모가 될 게임 오브젝트의 트랜스폼
    /// </summary>
    Transform linesParent;

    /// <summary>
    /// 씬 로드 이후에 호출 되는 함수
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();      // 디버그 출력용(없이도 상관 없음)

        linesParent = transform.GetChild(0);            // 처음에는 자식이 1개만 있기 때문에 그대로 가져옴

        pool = new Slime[poolSize];                     // 풀 배열 크기 잡기(poolSize만큼)
        readyQueue = new Queue<Slime>(poolSize);        // 레디큐 생성(poolSize 만큼 capaticy 확보)

        //for (int i = 0; i < poolSize; i++)              // poolSize만큼 반복
        //{
        //    GameObject obj = Instantiate(slimePrefab, this.transform);      // 슬라임 생성하고 팩토리의 자삭으로 설정
        //    obj.name = $"Slime_{i}";                                        // 이름 재설정
        //    Slime slime = obj.GetComponent<Slime>();                        // Slime 컴포너트 찾아서
        //    slime.onDisable += () =>
        //    {
        //        readyQueue.Enqueue(slime);                                  // 슬라임 게임오브젝트가 disable 될 때 레디큐로 되돌리기
        //    };
        //    PathLineDraw pathLine = obj.GetComponentInChildren<PathLineDraw>(); // 슬라임의 자식인 PathLineDraw 찾기
        //    pathLine.gameObject.name = $"PathLine{i}";                      // 이름 재설정
        //    pathLine.transform.SetParent(linesParent);                      // 부로를 linesParent로 변경
        //    pathLine.gameObject.SetActive(false);                           // pathLine 비활성화

        //    pool[i] = slime;                                                // 풀에 슬라임 저장해 놓기
        //    obj.SetActive(false);                                           // 시작하자마자 비활성화
        //}

        GenerateSlimes(0, poolSize, pool);            // 풀에 슬라임 채워 넣기
    }

    /// <summary>
    /// 슬라임을 Instantiate하고 기본적으로 필요한 처리들 수행하는 함수
    /// </summary>
    /// <param name="atrt">생성한 슬라임이 들어가기 시작할 인덱스</param>
    /// <param name="end">생성한 슬라임이 마지막으로 들어가는 풀의 인덱스 한 칸 앞</param>
    /// <param name="array">생성한 슬라임이 들어갈 풀</param>
    void GenerateSlimes(int atrt, int end, Slime[] array)
    {
        for (int i = atrt; i < end; i++)                                    // end - start 반복
        {
            GameObject obj = Instantiate(slimePrefab, this.transform);      // 슬라임 생성하고 팩토리의 자삭으로 설정
            obj.name = $"Slime_{i}";                                        // 이름 재설정
            Slime slime = obj.GetComponent<Slime>();                        // Slime 컴포너트 찾아서
            slime.onDisable += () =>
            {
                readyQueue.Enqueue(slime);                                  // 슬라임 게임오브젝트가 disable 될 때 레디큐로 되돌리기
            };
            PathLineDraw pathLine = slime.PathLine; // 슬라임의 자식인 PathLineDraw 찾기
            pathLine.gameObject.name = $"PathLine{i}";                      // 이름 재설정
            pathLine.transform.SetParent(linesParent);                      // 부로를 linesParent로 변경
            pathLine.gameObject.SetActive(false);                           // pathLine 비활성화

            array[i] = slime;                                                // 풀에 슬라임 저장해 놓기
            obj.SetActive(false);                                           // 시작하자마자 비활성화
        }
    }

    /// <summary>
    /// 풀에서 슬라임 하나를 꺼내서 주는 함수
    /// </summary>
    /// <returns></returns>
    public Slime GetSlime()
    {
        // 풀에서 사용할 수 있는 슬라임이 있는지 확인
        if (readyQueue.Count > 0)
        {
            // 지금 사용할 수 있는 슬라임이 있는 상태
            Slime slime = readyQueue.Dequeue();     // 큐에서 하나 꺼내고
            slime.gameObject.SetActive(true);       // 활성화 시키고

            return slime;                           // 활성화 시킨 슬라임을 리턴
        }
        else
        {
            // 지금 사용할 수 있는 슬라임이 없는 상태 => 풀의 크기를 2배로 늘리고 슬라임임도 추가
            int newSize = poolSize * 2;             // 새크기를 원래 크기의 2배로 설정
            Slime[] newPool = new Slime[newSize];   // 새풀도 원래 풀의  2배로 설정
            for (int i = 0; i < poolSize; i++)
            {
                newPool[i] = pool[i];               // 새풀에 기존 풀에 있는 슬라임 전부 복사
            }

            //for (int i = poolSize; i < newSize; i++)    // 새풀의 빈곳에 새 슬라임 만들어서 추가(Initialize와 거의 동일)
            //{
            //    GameObject obj = Instantiate(slimePrefab, this.transform);
            //    obj.name = $"Slime_{i}";
            //    Slime slime = obj.GetComponent<Slime>();
            //    slime.onDisable += () =>
            //    {
            //        readyQueue.Enqueue(slime);
            //    };
            //    newPool[i] = slime;                     // 새 풀의 뒤쪽에 추가한 다는 것만 다름
            //    obj.SetActive(false);
            //}

            GenerateSlimes(poolSize, newSize, newPool); // 풀의 확장된 부분에 슬라임 채워 넣기
            pool = newPool;                             // 새 풀을 풀로 설정
            poolSize = newSize;                         // 풀 크기를 새 풀 크기로 설정

            return GetSlime();                          // 새 것 하나 꺼내서 리턴하기
        }
    }
}
