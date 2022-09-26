using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    float fireAngle = 10.0f;

    public float turnSpeed = 2.0f;
    public float sightRadius = 5.0f;

    Transform fireTransforem;


    Transform target = null;
    Transform barrelBody;
    RaycastHit hit;
    // float maxDistance = 5.0f;


    float currentAngle = 0.0f;
    float TargetAngle = 0.0f;
    Vector3 initialForward;
    Vector3 dir;


     bool targetin = false;
     bool targetOn = false;

    IEnumerator fireCoroutine;

    private void Awake()
    {
        barrelBody = transform.GetChild(2);
        fireTransforem = barrelBody.GetChild(1);

        fireCoroutine = PeriodFire();
    }

    private void Start()
    {
        initialForward = transform.forward;
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.radius = sightRadius;

        StartCoroutine(fireCoroutine);
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
        LookTarget();

        

        //if (targetin)
        //{
        //    if (Physics.Raycast(barrelBody.position, barrelBody.forward, out hit, maxDistance))
        //    {
        //        if (!targetOn)
        //        {
        //            StartCoroutine(BulletFire());
        //            targetOn = true;
        //        }
        //    }
        //    else
        //    {
        //        if (targetOn)
        //        {
        //            StopAllCoroutines();
        //            targetOn = false;
        //        }
        //    }
        //}
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
            targetin = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
            targetin = false;
        }
    }

    private void LookTarget()
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
            dir = (target.position - barrelBody.position);
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

    void Fire()
    {
        // 총알을 발사한다.
        // 총알 프리팹. 총알이 발사될 방향과 위치. 총알이 발사되는 주기.
        Instantiate(bulletPrefab, fireTransforem.position, fireTransforem.rotation);
    }

    bool IsInFireAngle()
    {

        bool result = false;

        return result;
    }

    IEnumerator PeriodFire()
    {
        while (true)
        {
            Fire();

            yield return new WaitForSeconds(1.0f);
        }
    }


    IEnumerator BulletFire()
    {
        while (true)
        {
            GameObject obj = Instantiate(bulletPrefab, barrelBody.position, Quaternion.LookRotation(dir));
            obj.transform.Translate(0, 0, 1.5f);        // barrelBody의 z축으로 1.5만큼 이동후 생성

            yield return new WaitForSeconds(1.0f);
        }
    }

}
