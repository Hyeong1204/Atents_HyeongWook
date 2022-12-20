using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Shader : TestBase
{
    public GameObject phaseSlime;
    public float phaseDuration = 2.0f;

    protected override void Test1(InputAction.CallbackContext _)
    {
        StartCoroutine(StartPhase());
    }

    IEnumerator StartPhase()
    {
        Renderer renderer = phaseSlime.GetComponent<SpriteRenderer>();
        Material material = renderer.material;
        material.SetFloat("_thickness", 0.1f);
        material.SetFloat("_Split", 0.0f);

        float timeElipsed = 0.0f;
        float phaseDuationNoramlize = 1 / phaseDuration;

        while (timeElipsed < phaseDuration)
        {
            timeElipsed += Time.deltaTime;

            material.SetFloat("_Split", timeElipsed * phaseDuationNoramlize);
            yield return null;
        }

        material.SetFloat("_thickness", 0.0f);
    }
}
