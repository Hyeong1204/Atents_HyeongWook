using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Dino dino;

    int hiScore;            // 최고 점수
    int score;              // 현재 점수
    public int HiScore => hiScore;
    public int Score
    {
        get => score;
        set
        {
            score = value;
        }
    }

    public Dino Dino { get => dino; }

    protected override void Initaialize()
    {
        dino = FindObjectOfType<Dino>();
        dino.isDie += BestScoreUpdate;
        LoadDateUpdate();
    }

    void SaveDateUpdate()           // 세이브
    {
        SaveData saveData = new();
        saveData.hiScore = hiScore;

        string json = JsonUtility.ToJson(saveData);
        string path = $"{Application.dataPath}/Save/";
        string fullPath = $"{path}bestScore.json";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);        // 해당 위치에 폴더 만들기
        }

        File.WriteAllText(fullPath, json);          // 해당 경로에 json형식으로 저장
    }

    void LoadDateUpdate()           // 세이브 파일 로드
    {
        string path = $"{Application.dataPath}/Save/";
        string fullPath = $"{path}bestScore.json";

        if(Directory.Exists(path) && File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);

            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            hiScore = saveData.hiScore;
        }
        else
        {
            hiScore = 0;        // 파일이 없으면 0으로
        }
    }

    void BestScoreUpdate()          // 최고점수 갱신
    {
        if(hiScore < score)
        {
            hiScore = score;

            SaveDateUpdate();       // 세이브 하기
        }
    }
}
