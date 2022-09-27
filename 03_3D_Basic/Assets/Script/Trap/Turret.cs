using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    float fireAngle = 5.0f;            // 플레이어와 총구에 각도가 10도 미만이면

    public float turnSpeed = 2.0f;      // 총구 회전 속도
    public float sightRadius = 5.0f;    // 콜라이더 반지름

    Transform fireTransforem;


    Transform target = null;        // 플레이어가 사정거리 안에 들어왔다. 없으면 null, 있으면 !null
    Transform barrelBody;
    //RaycastHit hit;
    // float maxDistance = 5.0f;


    float currentAngle = 0.0f;
    //float TargetAngle = 0.0f;       // 플레이어와 터렛의 각도
    Vector3 initialForward;
    Vector3 dir;                    // 터렛과 플레이어의 방향


     //bool targetin = false;
     //bool targetOn = false;
    bool isFiring = false;

    IEnumerator fireCoroutine;      // 코루틴을 담는 변수;

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

        //StartCoroutine(fireCoroutine);
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
            //targetin = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;                  // 범 위에서 나가면 target null로 만들어라
            //targetin = false;
            StopCoroutine(fireCoroutine);   // fireCoroutine 코루틴 종료
        }
    }

    private void LookTarget()
    {
        if (target != null)     // 타겟이 트리거 영역에 들어왔을 때
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
            dir = (target.position - barrelBody.position);      // 터렛과 플레이어의 방향 구하기
            dir.y = 0.0f;       // y값만 0으로 만들기 (안하면 플레이어의 아래쪽을 보기 때문)

            float betweenAngle = Vector3.SignedAngle(barrelBody.forward, dir, barrelBody.up);   // 정방향일 때 0 ~ 180도. 역방향일 때 0 ~ -180도

            Vector3 resultDir;

            if(Mathf.Abs(betweenAngle) > 0.1f)  // 사이각이 일정 각도 이하인지 체크
            {
                // 사이각이 충분한 경우

                float rotateDirection = 1.0f;   // 일단 + 방향(시계방향)으로 결정
                if (betweenAngle < 0)
                {
                    rotateDirection = -1.0f;    // betweenAngle이 -면 rotateDirection도 -1로
                }

                // 초당 trunSpeed 만큼 회전하는데 rotateDirection로 시계방향으로 회전할지 반시계 방향으로 회전할지 결정
                currentAngle += rotateDirection * turnSpeed * Time.deltaTime;   

                resultDir = Quaternion.Euler(0, currentAngle, 0) * initialForward;
            }
            else        // 사이각이 거의 0인 경우
            {
                resultDir = dir;
            }
            barrelBody.rotation = Quaternion.LookRotation(resultDir);
            

            if (!isFiring && IsInFireAngle())
            {
                FireStart();
            }
            if(isFiring && !IsInFireAngle())
            {
                FireStop();
            }
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
        Vector3 targetDir = target.position - barrelBody.forward;
        targetDir.y = 0.0f;
        return Vector3.Angle(barrelBody.forward, targetDir) < fireAngle;
    }

    void FireStart()
    {
        isFiring = true;
        StartCoroutine(fireCoroutine);
    }

    void FireStop()
    {
        isFiring = false;
        StopCoroutine(fireCoroutine);
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
