using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IBattle, IHealth
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

    public float attackPower = 10.0f;          // 공격력
    public float defencePower = 3.0f;          // 방어력
    public float maxHP = 100.0f;        // 최댜 HP
    float hp = 100.0f;                  // 현재 HP

    // 프로퍼티 ----------------------------------------------------------------------------------------------------------
    public float AttackPower => attackPower;
    public float DefencePower => defencePower;

    public float HP 
    {
        get => hp;
        set
        {
            if (hp != value)
            {
                hp = value;
                onHealthChage?.Invoke();

                if (hp < 0)
                {
                    Die();
                }
            }
        }
    }

    public float MaxHP => maxHP;
    // -------------------------------------------------------------------------------------------------------------------

    // 델리게이트 ---------------------------------------------------------------------------------------------------------
    public Action onHealthChage { get; set; }
    public Action onDie { get; set; }

    // -------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        weapon_r = GetComponentInChildren<WeaponPosition>().transform;          // 자식중에 WeaponPosition컴포넌트 찾기

        // 장비 교체가 일어나면 새로 해줘야함
        weaponPs = weapon_r.GetComponentInChildren<ParticleSystem>();           // weapon_r에 자식중에 ParticleSystem찾기
        weaponBlade = weapon_r.GetComponentInChildren<Collider>();              // 무기의 충돌 영역 가져오기
    }

    private void Start()
    {
        hp = maxHP;

        // 테스트용
        onHealthChage += Test_HP_Change;
        onDie += Test_Die;
    }

    void Test_HP_Change()
    {
        Debug.Log($"{gameObject.name}이 피해를 받았습니다. 현재 HP : {hp}");
    }

    void Test_Die()
    {
        Debug.Log($"{gameObject.name}이 죽었습니다. ");
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

    /// <summary>
    /// 공격용 함수
    /// </summary>
    /// <param name="target">공격할 대상</param>
    public void Attact(IBattle target)
    {
        target?.Defence(AttackPower);
    }

    /// <summary>
    /// 방어용 함수
    /// </summary>
    /// <param name="damage">현재 입은 데미지</param>
    public void Defence(float damage)
    {
        // 기본 공식 : 실제 입는 데미지 = 적 공격 데미지 - 방어력
        HP -= (damage - DefencePower);
    }

    /// <summary>
    /// 죽었을 때 실행될 함수
    /// </summary>
    public void Die()
    {
        onDie?.Invoke();
    }
}