using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeTiimeText : MonoBehaviour
{
    TextMeshProUGUI lifeText;

    private void Awake()
    {
        lifeText = GetComponent<TextMeshProUGUI>();        
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onLifeTimeChange += RefreshText;
        lifeText.text = $"{player.maxLifeTime:f2}";         // 남은 시간을 소수 두 번째 자리 까지 표현
    }

    private void RefreshText(float time, float _)
    {
        lifeText.text = $"{time:0.00} Sec";
    }
}
