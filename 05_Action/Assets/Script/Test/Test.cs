using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        TrailRenderer trail = GetComponent<TrailRenderer>();
        trail.startWidth = 1.0f;
        trail.endWidth = 0.1f;

        ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
        ps.Play();
        ps.Stop();
    }
}
