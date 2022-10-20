using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 무기에 붙어있는 파티클 시스템 컴포넌트
    /// </summary>
    ParticleSystem weaponPs;

    /// <summary>
    /// 무기가 붙어있을 게임오브젝
    /// </summary>
    Transform weapon_r;

    private void Awake()
    {
        weapon_r = GetComponentInChildren<WeaponPosition>().transform;          // 자식중에 WeaponPosition컴포넌트 찾기
        weaponPs = weapon_r.GetComponentInChildren<ParticleSystem>();           // weapon_r에 자식중에 ParticleSystem찾기
    }

    /// <summary>
    /// 무기의 이팩트를 키고 끄는 함수
    /// </summary>
    /// <param name="on">true면 무기 이팩트 켜고, false면 무기 이팩트를 끈다.</param>
    public void WeaponEffectSwitch(bool on)
    {
        if (weaponPs != null)
        {
            if (on)
            {
                weaponPs.Play();        // 파티클 이팩트 재생 시작
            }
            else
            {
                weaponPs.Stop();        // 파이클 이팩트 재생 중지
            }
        }
    }
}
