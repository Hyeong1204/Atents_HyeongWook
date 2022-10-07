using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLine;
    TMP_InputField inputField;
    CanvasGroup canvasGroup;

    int rank;

    private void Awake()
    {
        rankLine = GetComponentsInChildren<RankLine>();
        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.gameObject.SetActive(false);
        inputField.onEndEdit.AddListener(OnNameInputEnd);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        GameManager.Inst.onRankRefresh += RankDataRefresh;      // 랭킹 화면 갱신
        GameManager.Inst.onRankUpdate += EnableNameInput;       // 이름을 입력 받을 수 있게 하기
        Close();
        RankDataRefresh();
    }

    private void OnDisable()
    {
        GameManager temp = GameManager.Inst;
        if (temp != null)
        {
            temp.onRankRefresh -= RankDataRefresh;
            GameManager.Inst.onRankUpdate -= EnableNameInput;
        }
    }

    public void RankDataRefresh()   // 랭킹 화면 갱신
    {
        for(int i =0; i < rankLine.Length; i++)
        {
            rankLine[i].SetDate(GameManager.Inst.HighScores[i], GameManager.Inst.HighScorerName[i]);
        }
    }

    public void Open()          // 패널 보이게 하기
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()         // 패널 안보이게 하기
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void EnableNameInput(int index)     // 랭킹의 점수에 맞게 이름 TMP_InputField창이 그 위치에 가게 하는 함수
    {
        Open();
        rank = index;           // 순위 저장
        inputField.transform.position = new Vector3(inputField.transform.position.x, rankLine[rank].transform.position.y, inputField.transform.position.z);     // 순위 위치에 생성
        inputField.gameObject.SetActive(true);
    } 

    void OnNameInputEnd(string text)            // TMP_InputField의 값이 입력되면 실행됨
    {
        GameManager temp = GameManager.Inst;
        if (temp != null)
        {
            temp.SetHighScorerName(rank, text);     // 그 순위에 맞게 이름을 저장
        }
        inputField.gameObject.SetActive(false);
    }
}
