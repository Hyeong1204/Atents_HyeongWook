using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Goalll : MonoBehaviour
{
    public Action onGoalIn;

    ParticleSystem[] ps;
    Transform effect;

    private void Awake()
    {
        effect = transform.GetChild(2);
        ps = effect.GetComponentsInChildren<ParticleSystem>();      // 골인할 때 터트릴 파티클 시스템 찾기
        //ps = new ParticleSystem[effect.childCount];
        //for (int i = 0; i < effect.childCount; i++)
        //{
        //    ps[i] = effect.GetChild(i).GetComponent<ParticleSystem>();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playGoalInEffect();
            onGoalIn?.Invoke();
        }
    }

    void playGoalInEffect()
    {
        foreach(var Effect in ps)       // 찾아놓은 골인 파티클 시스템을 전부 실행하기
        {
            Effect.Play();
        }
        //for(int i = 0; i < effect.childCount; i++)
        //{
        //    ps[i].Play();
        //}
    }
}
