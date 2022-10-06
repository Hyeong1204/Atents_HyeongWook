using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    public Sprite[] medalSprites;

    ImageNumber score;
    ImageNumber bestScore;
    Image newMark;
    Image medalImage;

    private void Awake()
    {
        score = transform.GetChild(0).GetComponent<ImageNumber>();
        bestScore = transform.GetChild(1).GetComponent<ImageNumber>();
        newMark = transform.GetChild(2).GetComponent<Image>();
        medalImage = transform.GetChild(3).GetComponent<Image>();
    }

    private void OnDisable()
    {
        // OnDestroy에서 하면 게임 종료시 GameManager는 이미 삭제되었는데
        // OnDestroy에서 GameManager.Inst를 접근하여 새롭게 GameManager를 만드는 일이 발생 가능
        // 그것을 방지하기 위해 OnDisable에서 실행

        // GameManager가 삭제되기 전에 연결 해제
        GameManager temp = GameManager.Inst;
        if (temp != null)
        {
            temp.onNewMark -= OnMark;       // 이 패널이 닫힐 때(게임이 끝날 때) 델리게이트에 연결된 함수 해제
        }
    }

    private void Start()
    {
        GameManager.Inst.onNewMark += OnMark;       // 델리게이트와 OnMark를 연결
        newMark.color = Color.clear;        //시작 할때 newMark를 안보이게 만듦
    }

    public void RefreshScore()
    {
        int playerScore = GameManager.Inst.Score;               // 
        score.maxNumber = playerScore;                          // 현재 점수 설정
        
        bestScore.maxNumber = GameManager.Inst.BestScore;       // 최고 점수 설정 (새가 죽을 때 최고점수는 자동으로 갱신 됨)

        // 100점 이상이면 브론즈 메달
        // 200점 이상이면 실버 메달
        // 300점 이상이면 골드 메달
        // 400점 이상이면 플레티넘 메달

        if(playerScore >= 100)
        {
            medalImage.color = Color.white;
        }

        if(playerScore >= 400)
        {
            medalImage.sprite = medalSprites[3];
        }
        else if(playerScore >= 300)
        {
            medalImage.sprite = medalSprites[2];
        }
        else if(playerScore >= 200)
        {
            medalImage.sprite = medalSprites[1];
        }
        else if(playerScore >= 100)
        {
            medalImage.sprite = medalSprites[0];
        }
        else
        {
            medalImage.color = Color.clear;
        }
    }

    void OnMark()
    {
        newMark.color = Color.white;        // newMark 나오게 하기
    }
}
