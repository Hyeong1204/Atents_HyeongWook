using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : Singleton<Gamemanager>
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// player 일기 전용 프로퍼티
    /// </summary>
    public Player Player => player;

    /// <summary>
    /// 씬이 로드 되었을 때 실행될 초기화 함수
    /// </summary>
    protected override void Initaialize()
    {
        player = FindObjectOfType<Player>();        // 플리어 찾기
    }
}
