using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public float turnSpeed = 2.0f;
    public float sightRadius = 5.0f;

    Transform target = null;
    Transform barrelBody;

    float currentAngle = 0.0f;
    float TargetAngle = 0.0f;
    Vector3 initialForward;
    

    private void Awake()
    {
        barrelBody = transform.GetChild(2);
    }

    private void Start()
    {
        initialForward = transform.forward;
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.radius = sightRadius;
    }

    private void OnValidate()       // 인스펙터 창에서 값이 성공적으로 변경 되었을 때 호출되는 함수
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider != null)
        {
            collider.radius = sightRadius;
        }
    }




    private void Update()
    {
        if (target != null)
        {
            // 보간을 사용한 경우
            //Vector3 dir = (target.position - barrelBody.position);        // 총구를 플레이어쪽으로 돌리기 위해 총구에서 플레이어 위치로 가는 방향 벡터를 계산
            //dir.y = 0;  // 총구가 바닥을 향하기 때문에 y축의 영향을 제거 (x, z축의 방향만 남음)

            //// Time.deltaTime 1초에 걸처 도착점에 도달한다.
            //dir = Vector3.Lerp(barrelBody.forward, dir, turnSpeed * Time.deltaTime);        // Lerp(보간) 시작점, 도착점, 걸리는 신간

            //barrelBody.rotation = Quaternion.LookRotation(dir);     // 최종적인 방향을 바라보는 회전을 만듦
            ////barrelBody.Rotate(turnSpeed * Time.deltaTime * dir);

            ////barrelBody.LookAt(target);

            // 각도를 사용하느 경우 (등속도로 회전)
            Vector3 dir = (target.position - barrelBody.position);
            dir.y = 0.0f;

            TargetAngle = Vector3.SignedAngle(initialForward, dir, barrelBody.up);

            if (currentAngle < TargetAngle)
            {
                currentAngle += turnSpeed * Time.deltaTime;
                currentAngle = Mathf.Min(currentAngle, TargetAngle);
            }
            else if (currentAngle > TargetAngle)
            {
                currentAngle -= turnSpeed * Time.deltaTime;
                currentAngle = Mathf.Max(currentAngle, TargetAngle);
            }

            Vector3 targetDir = Quaternion.Euler(0, currentAngle, 0) * initialForward;

            barrelBody.rotation = Quaternion.LookRotation(targetDir);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }



}
