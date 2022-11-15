using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyPanelUI : MonoBehaviour
{
    TextMeshProUGUI moneyText;
    Player player;
    private void Awake()
    {
        moneyText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        player = Gamemanager.Inst.Player;
        player.onMoneyChange += OnMoneyChange;
        moneyText.text = player.Money.ToString("##,0");
    }

    private void OnMoneyChange(int obj)
    {
        moneyText.text = player.Money.ToString("##,0");
    }
}
