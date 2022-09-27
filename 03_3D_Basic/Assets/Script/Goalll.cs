using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goalll : MonoBehaviour
{
    ParticleSystem[] ps;
    Transform effect;

    private void Awake()
    {
        effect = transform.GetChild(2);
        ps = new ParticleSystem[effect.childCount];
        for (int i = 0; i < effect.childCount; i++)
        {
            ps[i] = effect.GetChild(i).GetComponent<ParticleSystem>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playGoalInEffect();
        }
    }

    void playGoalInEffect()
    {
        for(int i = 0; i < effect.childCount; i++)
        {
            ps[i].Play();
        }
    }
}
