using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Test_Panel : MonoBehaviour
{
    TMP_InputField inputField;
    Button dieButton;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        dieButton = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        inputField.onValueChanged.AddListener(OnInputChanged);
        dieButton.onClick.AddListener(OnDieButtonClick);
    }

    public void OnInputChanged(string Text)
    {
        int score = 0;
        if (Text != "")
        {
            score = int.Parse(Text);
        }
        else
        {
            score = 0;
        }
        GameManager.Inst.TestSetScore(score);
    }
    private void OnDieButtonClick()
    {
        GameManager.Inst.Player.TestDie();
    }

}
