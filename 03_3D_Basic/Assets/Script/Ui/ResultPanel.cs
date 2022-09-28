using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    Button button;
    TextMeshProUGUI resultText;
    

    float clearTime = 0.0f;

    public float ClearTime 
    {
        get => clearTime;
        set
        {
            clearTime = value;
            resultText.text = $"클리어하는데 {clearTime:f2}초 걸렸습니다.";
        }
    }

    public string nextScenName;

    private void Awake()
    {
        resultText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        button = GetComponentInChildren<Button>();
        
        
    }

    private void Start()
    {
        Goalll goal = FindObjectOfType<Goalll>();
        button.onClick.AddListener(goal.GoNextScen);     //  버튼에 함수 연결
    }

    private void ButtonTest2()
    {
        SceneManager.LoadScene(nextScenName);
    }

    public void ButtonTest1()
    {
        Debug.Log("버튼 클릭");
    }

    
}
