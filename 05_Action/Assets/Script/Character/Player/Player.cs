using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Xml.Serialization;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
    Transform weapon_l;

    /// <summary>
    /// 무기가 데미지를 주는 영역의(리치) 트리거
    /// </summary>
    Collider weaponBlade;
    Animator anim;

    public float attackPower = 10.0f;          // 공격력
    public float defencePower = 3.0f;          // 방어력
    public float maxHP = 100.0f;        // 최댜 HP
    float hp = 100.0f;                  // 현재 HP
    bool isAlive = true;                // 살아 있는지 죽어있는지 표시

    Inventory inven;
    public float itemPickupRange = 2.0f;

    // 프로퍼티 ----------------------------------------------------------------------------------------------------------
    public float AttackPower => attackPower;
    public float DefencePower => defencePower;

    public bool IsAlive { get => isAlive; }

    public float HP 
    {
        get => hp;
        set
        {
            if (isAlive && hp != value )            // 살아있고 hp가 변경되었을 때만 실행
            {
                hp = value;

                if (hp < 0)
                {
                    Die();
                }

                hp = Mathf.Clamp(hp, 0.0f, maxHP);

                onHealthChage?.Invoke(hp/maxHP);
            }
        }
    }

    public float MaxHP => maxHP;
    // -------------------------------------------------------------------------------------------------------------------

    // 델리게이트 ---------------------------------------------------------------------------------------------------------
    public Action<float> onHealthChage { get; set; }
    public Action onDie { get; set; }

    // -------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        weapon_r = GetComponentInChildren<WeaponPosition>().transform;          // 자식중에 WeaponPosition컴포넌트 찾기
        weapon_l = GetComponentInChildren<ShieldPosition>().transform;          // 자식중에 ShieldPosition컴포넌트 찾기
        anim = GetComponent<Animator>();

        // 장비 교체가 일어나면 새로 해줘야함
        weaponPs = weapon_r.GetComponentInChildren<ParticleSystem>();           // weapon_r에 자식중에 ParticleSystem찾기
        weaponBlade = weapon_r.GetComponentInChildren<Collider>();              // 무기의 충돌 영역 가져오기
    }

    private void Start()
    {
        HP = maxHP;
        isAlive = true;
        // 테스트용
        //onHealthChage += Test_HP_Change;
        //onDie += Test_Die;
        inven = new Inventory(this);
        Gamemanager.Inst.InvenUI.InitializeInventoty(inven);
    }

    void Test_HP_Change(float ratino)
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
    /// 무기와 방패를 표시하거나 표시하지 않는 함수
    /// </summary>
    /// <param name="isShow">true면 포시하고 false면 표시하지 않는다.</param>
    public void ShowWeaponAndSheild(bool isShow)
    {
        weapon_r.gameObject.SetActive(isShow);
        weapon_l.gameObject.SetActive(isShow);
    }

    /// <summary>
    /// 방어용 함수
    /// </summary>
    /// <param name="damage">현재 입은 데미지</param>
    public void Defence(float damage)
    {
        // 기본 공식 : 실제 입는 데미지 = 적 공격 데미지 - 방어력

        if (isAlive)                            // 살아있을 때만 데미지 입음.
        {
            anim.SetTrigger("Hit");             // 히트 애니메이션 재생
            HP -= (damage - DefencePower);      // hp 감소
        }
    }

    /// <summary>
    /// 죽었을 때 실행될 함수
    /// </summary>
    public void Die()
    {
        isAlive = false;
        ShowWeaponAndSheild(true);
        anim.SetLayerWeight(1, 0.0f);           // 애니메이션 레이어 가중ㅈ치 제거
        anim.SetBool("IsAlive", isAlive);       // 죽었다고 표시해서 사망 애니메이션 재생
        onDie?.Invoke();
    }

    /// <summary>
    /// 플레이어 주변의 아이템을 획득하는 함수
    /// </summary>
    public void ItemPickup()
    {
        Collider[] items = Physics.OverlapSphere(transform.position, itemPickupRange, LayerMask.GetMask("Item"));

        foreach(var itemCollider in items)
        {
            Item item = itemCollider.gameObject.GetComponent<Item>();
            if (inven.AddItem(item.data))       // 추가가 성공하면
            {
                Destroy(itemCollider.gameObject);   // 아이템 오브젝트 삭제
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.up, itemPickupRange);
    }
#endif
}
