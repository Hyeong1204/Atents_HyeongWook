using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]          // 직렬화 가능
public class SaveData
{
    public int[] highScore;                 // 최고 득점. 0번재가 1등, 4번째가 5등
    public string[] highScorerName;         // 최고 득점자 이름. 0번재가 1등, 4번째가 5등
}
