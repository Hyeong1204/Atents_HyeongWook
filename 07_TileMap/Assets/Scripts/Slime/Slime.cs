using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public float phaseDuration = 0.5f;
    public float dissolveDuration = 1.0f;
    const float Outline_Thickness = 0.005f;

    Material mainMaterial;

    Action onPhaseEnd;
    Action onDie;

    private void Awake()
    {
        Renderer renderer = GetComponent<SpriteRenderer>();
        mainMaterial = renderer.material;
    }

    private void OnEnable()
    {
        StartCoroutine(StartPhase());
    }

    private void Start()
    {
        ShowOutLine(false);
        mainMaterial.SetFloat("_Dissolve_Fade", 1.0f);
    }

    public void Die()
    {
        StartCoroutine(StartDissolve());
        onDie?.Invoke();
    }

    IEnumerator StartPhase()
    {
        mainMaterial.SetFloat("_Phase_Thickness", 0.1f);
        mainMaterial.SetFloat("_Phase_Split", 0.0f);

        float timeElipsed = 0.0f;
        float phaseDuationNoramlize = 1 / phaseDuration;

        while (timeElipsed < phaseDuration)
        {
            timeElipsed += Time.deltaTime;

            mainMaterial.SetFloat("_Phase_Split", timeElipsed * phaseDuationNoramlize);
            yield return null;
        }

        mainMaterial.SetFloat("_Phase_Thickness", 0.0f);
        onPhaseEnd?.Invoke();
    }

    IEnumerator StartDissolve()
    {
        mainMaterial.SetFloat("_Dissolve_Fade", 1.0f);

        float timeElipsed = 0.0f;
        float phaseDuationNoramlize = 1 / dissolveDuration;

        while (timeElipsed < dissolveDuration)
        {
            timeElipsed += Time.deltaTime;

            mainMaterial.SetFloat("_Dissolve_Fade", 1 - timeElipsed * phaseDuationNoramlize);
            yield return null;
        }

        Destroy(this.gameObject);
    }

    public void ShowOutLine(bool isShow)
    {
        if(isShow)
        {
            mainMaterial.SetFloat("_OutLine_Thickness", Outline_Thickness);
        }
        else
        {
            mainMaterial.SetFloat("_OutLine_Thickness", 0.0f);
        }
    }
}
