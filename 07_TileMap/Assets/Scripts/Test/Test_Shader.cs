using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Shader : TestBase
{
    public GameObject phaseSlime;
    public GameObject dessolveSlime;
    public GameObject allSlime;
    public float allDuration = 2.0f;

    protected override void Test1(InputAction.CallbackContext _)
    {
        StartCoroutine(StartPhase());
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        StartCoroutine(StartDessolve());
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        StartCoroutine(StartAllTest(allSlime));
    }

    IEnumerator StartPhase()
    {
        Renderer renderer = phaseSlime.GetComponent<SpriteRenderer>();
        Material material = renderer.material;
        material.SetFloat("_thickness", 0.1f);
        material.SetFloat("_Split", 0.0f);

        float timeElipsed = 0.0f;
        float phaseDuationNoramlize = 1 / allDuration;

        while (timeElipsed < allDuration)
        {
            timeElipsed += Time.deltaTime;

            material.SetFloat("_Split", timeElipsed * phaseDuationNoramlize);
            yield return null;
        }

        material.SetFloat("_thickness", 0.0f);
    }

    IEnumerator StartDessolve()
    {
        Renderer renderer = dessolveSlime.GetComponent<SpriteRenderer>();
        Material material = renderer.material;
        material.SetFloat("_Fode", 1.0f);

        float timeElipsed = allDuration;
        float phaseDuationNoramlize = 1 / allDuration;

        while (timeElipsed > 0)
        {
            timeElipsed -= Time.deltaTime;

            material.SetFloat("_Fode", timeElipsed * phaseDuationNoramlize);
            yield return null;
        }

        material.SetFloat("_Fode", 0.0f);
    }


    IEnumerator StartAllTest(GameObject target)
    {
        Renderer renderer = target.GetComponent<SpriteRenderer>();
        Material material = renderer.material;

        material.SetFloat("_Phase_Split", 0.0f);
        material.SetFloat("_Dissolve_Fade", 1.0f);

        float timeElipsed = 0;
        float allDuationNoramlize = 1 / allDuration;

        while (timeElipsed < allDuration)
        {
            timeElipsed += Time.deltaTime;

            material.SetFloat("_Phase_Split", timeElipsed * allDuationNoramlize);
            material.SetFloat("_Dissolve_Fade", 1 - timeElipsed * allDuationNoramlize);
            yield return null;
        }

        material.SetFloat("_Phase_Tickness", 0.0f);
    }
}
