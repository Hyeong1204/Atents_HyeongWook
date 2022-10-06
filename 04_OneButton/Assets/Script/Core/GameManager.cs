using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System;

public class GameManager : Singleton<GameManager>
{
    ImageNumber socreUI;
    PipeRotator pipeRotator;
    Bird player;
    public Action onNewMark;            // 최고점수가 갱신 될때 실행될 델리게이트
    public Action onChangRank;

    int score = 0;
    int bestScore = 0;

    const int RankCount = 5;
    int[] highScores = new int[RankCount];                   // 0번째 1등 4번째 꼴등
    string[] highScorerName = new string[RankCount];        // 0번째 1등 4번째 꼴등

    public Bird Player => player;

    public int BestScore
    {
        get => bestScore;
        private set
        {
            bestScore = value;
        }
    }

    public int Score
    {
        get => score;
        private set
        {
            score = value;
            socreUI.maxNumber = score;
        }
    }

    public int[] HighScores => highScores;
    public string[] HighScorerName => highScorerName;

    protected override void Initaialize()
    {
        player = FindObjectOfType<Bird>();
        player.onDead += BestScoreUpdate;       // 새가 죽을 때 최고 점수 갱신 시도
        player.onDead += RankUpdate;            // 새가 죽을 때 랭크 갱신

        pipeRotator = FindObjectOfType<PipeRotator>();
        pipeRotator.SetPipeScoreDelegate(AddScore);
        socreUI = GameObject.FindGameObjectWithTag("Score").GetComponent<ImageNumber>();

        LoadGameDate();         // 신 로드와 동시에 데이타 파일 로드하기
    }

    void AddScore(int point)
    {
        Score += point;
    }

    public void TestSetScore(int newScore)
    {
        Score = newScore;
    }

    void SaveGameDate()         // 데이터 저장하기
    {
        SaveData saveDate = new();          // 저장할 클래스 생성
        saveDate.bestScore = BestScore;     // 저장할 데이터 넣기
        saveDate.name = "Test";

        string json = JsonUtility.ToJson(saveDate);             // 클래스를 문자열로 변환
        string path = $"{Application.dataPath}/Save/";          // path에 파일 경로 넣기

        if (!Directory.Exists(path))                            // path경로에 파일이 있는지 판단
        {
            Directory.CreateDirectory(path);                    // 업다면 파일 생성
        }

        string fullPath = $"{path}Test.json";                   // fullPath에 파일경로 + 파일 이름(확장자까지)저장
        File.WriteAllText(fullPath, json);                      // fullPath경로에 있는 파일에 json내용을 저장
    }

    void LoadGameDate()        // 데이터 로드하기
    {
        string path = $"{Application.dataPath}/Save/";          // 파일 경로
        string fullPath = $"{path}Test.json";                   // 파일 경로 + 파일 이름

        BestScore = 0;     // 기본 값 설정
        if (Directory.Exists(path) && File.Exists(fullPath))    // 파일 경로안에 폴더가 있고 그 해당 파일까지 있으면
        {
            string json = File.ReadAllText(fullPath);           // 파일에 있는 내용을 문자열로 받아오기
            SaveData loadDate = JsonUtility.FromJson<SaveData>(json);       // loadDate에 SaveData타입으로 json내용 넣기
            BestScore = loadDate.bestScore;                     // loadDate.bestScore에 있는 데이터를 BestScore에 넣기
        }
    }

    /// <summary>
    /// 최고 점수 저장와 득점자 이름 추가
    /// </summary>
    void BestScoreUpdate()
    {
        if(BestScore < Score)               // Score가 BestScore보다 높으면
        {
            onNewMark?.Invoke();            // 델리게이트 사용 (ResultPanel로 이동)
            BestScore = Score;              
            SaveGameDate();                 // SaveGameDate 함수 사용하여 저장하기
        }
    }

    void RankUpdate()
    {
        for(int i = 0; i < RankCount; i++)  // 한 단계씩 비교해서 Score가 더 크면
        {
            if (highScores[i] < Score)      // 새 Score가 더 크면
            {
                for(int j = RankCount -1;j > i; j--)        // 그 아래 단계는 하나씩 뒤로 밀고
                {
                    highScores[j] = highScores[j - 1];
                    highScorerName[j] = highScorerName[j - 1];
                }
                highScores[i] = Score;      // 새 Score 넣기
                highScorerName[i] = "";

                onChangRank?.Invoke();
                break;
            }
        }
        //highScorerName;
    }
}