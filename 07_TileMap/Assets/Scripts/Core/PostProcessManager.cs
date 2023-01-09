using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    Volume postProcessVolume;
    Vignette vighette;

    private void Awake()
    {
        postProcessVolume = GetComponent<Volume>();
        postProcessVolume.profile.TryGet<Vignette>(out vighette);
    }

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        player.onLifeTimeChange += OnLifeTimeChange;

        vighette.intensity.value = 0;
    }

    private void OnLifeTimeChange(float time, float maxTime)
    {
        vighette.intensity.value = 1 - time / maxTime;
    }
}
