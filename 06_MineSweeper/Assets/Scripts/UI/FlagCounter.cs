using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCounter : MonoBehaviour
{
    ImageNumber imageNumber;

    private void Awake()
    {
        imageNumber= GetComponent<ImageNumber>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        gameManager.onFlagCountChange += Refresh;
    }

    private void Refresh(int flagCount)
    {
        imageNumber.Number = flagCount;
    }
}
