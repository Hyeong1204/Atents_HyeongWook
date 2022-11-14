using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMana
{
    float MP { get; set; }  // HP를 확인하고 설저할 수 있따.
    float MaxMP { get; }    // 최대HP를 확인할 수있다.

    /// <summary>
    /// HP가 변경될 때 실행될 델리게이트용 프로퍼티
    /// 파라메터는 현재 / 최대 비율
    /// </summary>
    Action<float> onManaChage { get; set; }

    /// <summary>
    /// Update 함수에서 지속적으로 마나 회복하는 함수
    /// </summary>
    void ManaRegenerate();
}
