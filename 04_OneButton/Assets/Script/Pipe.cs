using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float minHeight;     // 랜덤 최소 높이
    public float maxHeight;     // 랜덤 최대 높이

    Rigidbody2D rigid;


    /// <summary>
    /// 플레이어가 파이프를 통과하면 실행될 델리게이트
    /// </summary>
    public System.Action<int> onScored;
    public int point = 10;

    /// <summary>
    /// 랜덤한 위치를 반환하는 프로퍼티
    /// </summary>
    public float RandomHeight { get => Random.Range(minHeight, maxHeight); }    // get 할때마다 랜덤으로 부여


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();        // 리지드 바디 찾기
    }
    
    private void Start()
    {
        Vector2 pos = Vector2.up * RandomHeight;
        transform.Translate(pos);       // 시작할 때 첫 위치 랜덤으로 지정
    }

    //public void RestRandomHeight()
    //{
    //    // 주의 : 위치를 옮기고 사용하기
    //    Vector2 pos = Vector2.up * RandomHeight;
    //    rigid.MovePosition(rigid.position + pos);
    //}


    /// <summary>
    /// 왼쪽으로 이동시키는 함수
    /// </summary>
    /// <param name="moveDelta">이동시킬 정도</param>
    public void MoveLeft(float moveDelta)
    {
        rigid.MovePosition(rigid.position + moveDelta * Vector2.left);  // 현재 위치에서 moveDelta만큼 왼쪽으로 이동
    }


    /// <summary>
    /// 통과 체크 용도
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))     // 플레이어가 통과 했다면
        {
            Bird bird = collision.GetComponent<Bird>();
            if (bird != null && !bird.IsDead)    // 살아있을 때만 점수 증가
            {
                onScored?.Invoke(point);         // 델리게이트 실행
            }
        }
    }
}
