using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    RankLine[] rankLine;

    private void Awake()
    {
        rankLine = GetComponentsInChildren<RankLine>();
    }

    private void Start()
    {
        GameManager.Inst.onChangRank += RankDateUpdate;
    }

    private void OnDisable()
    {
        GameManager temp = GameManager.Inst;
        if (temp != null)
        {
            temp.onChangRank -= RankDateUpdate;
        }
    }

    public void RankDateUpdate()
    {
        for(int i =0; i < rankLine.Length; i++)
        {
            rankLine[i].SetDate(GameManager.Inst.HighScores[i], GameManager.Inst.HighScorerName[i]);
        }
    }
}
