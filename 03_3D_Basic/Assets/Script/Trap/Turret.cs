using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{

    public float turnSpeed = 360.0f;
    public float sightRadius = 5.0f;

    Transform target = null;
    Transform barrelBody;
    

    private void Awake()
    {
        barrelBody = transform.GetChild(2);
    }

    private void Start()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.radius = sightRadius;
    }

    private void OnValidate()       // 인스펙터 창에서 값이 성공적으로 변경 되었을 때 호출되는 함수
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.radius = sightRadius;
    }




    private void Update()
    {
        if (target != null)
        {
            Vector3 dir = (target.position - barrelBody.position).normalized;        // 총구를 플레이어쪽으로 돌리기 위해 총구에서 플레이어 위치로 가는 방향 벡터를 계산
            dir.y = 0;  // 총구가 바닥을 향하기 때문에 y축의 영향을 제거 (x, z축의 방향만 남음)
            //barrelBody.rotation = Quaternion.LookRotation(dir);     // 최종적인 방향을 바라보는 회전을 만듦
            barrelBody.Rotate(turnSpeed * Time.deltaTime * dir);

            //barrelBody.LookAt(target);
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
