using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_GameOver : Test_Base
{
    protected override void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            //// Serializable로 되어 있는 클래스 만들기
            //SaveData saveDate = new();              // 해당 클래스의 인스턴스 만들기
            //saveDate.bestScore = 10;                // 인스턴스에 데이터 기록
            //saveDate.name = "TestPlayer";           // 인스턴스에 데이터 기록

            //string json = JsonUtility.ToJson(saveDate);         // 해덩 클래스를 json형식의 문자열로 변경

            //string path = $"{Application.dataPath}/Save/";      // 해당 폴더 경로, 파일을 지정할 폴더를 지정
            //string fullPath = $"{path}Save.json";               // 해당 폴더 경로 + 파일 이름(확장자까지)
            //if (!Directory.Exists(path))            // path 해당 폴더가 없으면
            //{
            //    Directory.CreateDirectory(path);    // 그 폴더를 만들어라 
            //}

            //File.WriteAllText(fullPath, json);      // fullPath경로에 json내용을 저장한다.

            //Debug.Log("세이브 완료");
        }


        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            ////string test = File.ReadAllText(@"d:\tset.txt");
            ////Debug.Log(test);
            //string path = $"{Application.dataPath}/Save/";          // 경로 확인용
            //string fullPath = $"{path}Save.json";                   // 전체 경로 확인용

            //if(Directory.Exists(path) && File.Exists(fullPath))     // 해당 폴더 및 파일이 있으면 실행
            //{
            //    string json = File.ReadAllText(fullPath);                   // json형식의 데이터 일기
            //    SaveData loadDate = JsonUtility.FromJson<SaveData>(json);   // SaveDate 타입에 맞게 파싱 (파싱 : 문자열을 쪼개는 작업)
            //    Debug.Log(loadDate.name);
            //}
        }
    }


}
