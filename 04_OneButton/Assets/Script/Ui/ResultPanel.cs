using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
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
        score.maxNumber = GameManager.Inst.Score;
    }

}
