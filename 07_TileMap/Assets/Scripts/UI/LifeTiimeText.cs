using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeTiimeText : MonoBehaviour
{
    TextMeshProUGUI lifeText;

    private void Start()
    {
        lifeText = GetComponent<TextMeshProUGUI>();
        Player player = GameManager.Inst.Player;
        player.onLifeTimeChange += RefreshText;
    }

    private void RefreshText(float time, float _)
    {
        lifeText.text = $"{time:0.00} Sec";
    }
}
