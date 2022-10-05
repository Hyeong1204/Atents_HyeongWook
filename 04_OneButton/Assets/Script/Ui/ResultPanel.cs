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

    public void RefreshScore()
    {
        int playerScore = GameManager.Inst.Score;
        score.maxNumber = playerScore;
        
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

}
