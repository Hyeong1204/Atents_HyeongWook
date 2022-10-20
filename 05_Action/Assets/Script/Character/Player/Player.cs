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

    /// <summary>
    /// 무기가 데미지를 주는 영역의(리치) 트리거
    /// </summary>
    Collider weaponBlade;

    private void Awake()
    {
        weapon_r = GetComponentInChildren<WeaponPosition>().transform;          // 자식중에 WeaponPosition컴포넌트 찾기

        // 장비 교체가 일어나면 새로 해줘야함
        weaponPs = weapon_r.GetComponentInChildren<ParticleSystem>();           // weapon_r에 자식중에 ParticleSystem찾기
        weaponBlade = weapon_r.GetComponentInChildren<Collider>();              // 무기의 충돌 영역 가져오기
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

    /// <summary>
    /// 무기가 공격 행동을 할 때 무기의 트리거 켜는 함수
    /// </summary>
    public void WeaponBladeEnable()
    {
        if(weaponBlade != null)
        {
            weaponBlade.enabled = true;
        }
    }

    /// <summary>
    /// 무기가 공격 행동이 끝날 때 무기의 트리거를 끄는 함수
    /// </summary>
    public void weaponBladeDisable()
    {
        if(weaponBlade != null)
        {
            weaponBlade.enabled = false;
        }
    }

}
