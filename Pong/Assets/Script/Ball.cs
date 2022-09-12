using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 dir;

    public float speed = 1.0f;

    private void Start()
    {
        dir = Random.insideUnitCircle;          // 반지름이 1인 원을 그려 무작위로 방향을 정함
        dir = dir.normalized;
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * dir);
    }


}
