using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goalll : MonoBehaviour
{
    public Action onGoalIn;
    

    public string nextSceanName;

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
            playGoalInEffect();             // 파티클 실행
            StartCoroutine(Wait1Second());  // 1초 이후에 결과창 열기

            onGoalIn?.Invoke();             // 골인 했을 때 실행될 팜수들 실행
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

    IEnumerator Wait1Second()
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.Inst.ShowResultPanel();     // 결과창 열기
    }

   public void GoNextScen()
    {
        SceneManager.LoadScene(nextSceanName);      // 지정된 씬으로 변경
    }


}
