using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLine;
    TMP_InputField inputField;

    private void Awake()
    {
        rankLine = GetComponentsInChildren<RankLine>();
        inputField = GetComponentInChildren<TMP_InputField>();
        inputField.gameObject.SetActive(false);
    }

    private void Start()
    {
        GameManager.Inst.onRankRefresh += RankDataRefresh;      // 화면 갱신
        GameManager.Inst.onRankUpdate += EnableNameInput;       // 이름을 입력 받을 수 있게 하기
        RankDataRefresh();
    }


    private void OnDisable()
    {
        GameManager temp = GameManager.Inst;
        if (temp != null)
        {
            temp.onRankRefresh -= RankDataRefresh;
        }
    }

    public void RankDataRefresh()
    {
        for(int i =0; i < rankLine.Length; i++)
        {
            rankLine[i].SetDate(GameManager.Inst.HighScores[i], GameManager.Inst.HighScorerName[i]);
        }
    }
    private void EnableNameInput(int index)
    {
        transform.position = new Vector3(transform.position.x, rankLine[index].transform.position.y, transform.position.z);
        inputField.gameObject.SetActive(true);
    } 
}
