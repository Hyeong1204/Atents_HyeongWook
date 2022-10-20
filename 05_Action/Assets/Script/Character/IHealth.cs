using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float HP { get; set; }  // HP를 확인하고 설저할 수 있따.
    float MaxHP { get; }    // 최대HP를 확인할 수있다.

    /// <summary>
    /// HP가 변경될 때 실행될 델리게이트용 프로퍼티
    /// 파라메터는 현재 / 최대 비율
    /// </summary>
    Action<float> onHealthChage { get; set; }

    void Die();     // 죽었을 때 실행될 함수

    Action onDie { get; set; }              // 죽었을 때 실행될 델리게이트용 프로퍼티
}
