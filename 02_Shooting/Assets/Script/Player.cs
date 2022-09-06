
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //public delegate void DelegateName();        // 이런 종류의 델리게이트가 있다. (리턴없고 파라메터도 없는 함수를 저장하는 델리게이트)

    //public DelegateName del;    // DelegateName 타입으로 del이라는 이름의 델리게이트를 만듬
    //Action del2;                // 리턴타입이 void, 파라메터도 없는 델리게이트 del2를 만듬
    //Action<int> del3;           // 리턴타입이 void, 파라메터는 int 하나인 델리게이트 del3를 만듬
    //Func<int, float> del4;      // 리턴타입이 int고 파라메터는 float 하나인 델리게이트 del4를 만듬

    // Awake > OnEnble > Start : 대체적으로 이 순서
    PlayerinputAction inputActions;
    Rigidbody2D rigid;
    Animator anim;
    public GameObject Bullet;
    GameObject flash;

    public GameObject explosionPrefab;

    Transform firePositionRoot;   // 트랜스폼을 여러개 가지는 배열
    //Vector3[] fireRot;

    Vector3 dir;                // 이동 방향(입력에 따라 변경됨)
    IEnumerator fireCoroutine;

    bool isDead = false;

    float fireAngle = 30.0f;
    int power = 0;
    int Power
    {
        get => power;
        set
        {
            power = value;          // 들어온 값으로 파워 설정
            if (power > 3)          // 파워가 3을 벗어나면 3을 제한
                power = 3;


            // 기존에 있는 파이어 포지션 제거
            while (firePositionRoot.childCount > 0)                  // 자식이 0보다 많으면....
            {
                Transform temp = firePositionRoot.GetChild(0);      // temp에 firePositionRoot 첫번째 자식을 넣어라
                temp.parent = null;                                 // temp에 부모을 버려라
                Destroy(temp.gameObject);                           // 자기 자신을 지워라
            }

            // 파워 등급에 맞게 새로 배치
            for (int i=0; i < power; i++)
            {
                GameObject firePos = new GameObject();          // 빈 오브젝트 생성하기
                firePos.name = $"FirePosition_{i}";             // firePos에 이름을 바꿈
                firePos.transform.parent = firePositionRoot;    // fiePos를 firePositionRoot에 자식으로 만듦
                firePos.transform.position = firePositionRoot.transform.position;       // firePos의 위치를 firePositionRoot위치로 바꿈

                // power가 1일때 : 0도
                // power가 2일때 : -15도, +15도
                // power가 3일때 : -30도, 0도, +30도
                firePos.transform.rotation = Quaternion.Euler(0, 0, (power - 1) * (fireAngle * 0.5f) + i * -fireAngle);
                firePos.transform.Translate(1.0f, 0, 0);            // 자기 자신을 x축으로 1.0f만큼 이동해라
            }
        }
    }

    public float Speed = 1.0f;  // 플레이어의 이동 속도(초당 이동 속도)
    float boost = 1.0f;
    //bool isFire = false;
    public float fireInterval = 0.5f;
    //float fireTimeCount = 0.0f;

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 생성된 직후에 호출
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerinputAction();
        rigid = GetComponent<Rigidbody2D>();        // 한번만 찾고 저장해서 계속 쓰기(메모리 더 쓰고 성능 아끼기)
        anim = GetComponent<Animator>();
        fireCoroutine = Fire();

        firePositionRoot = transform.GetChild(0);
        flash = transform.GetChild(1).gameObject;
        flash.SetActive(false);
        //fireRot = new Vector3[3];
        //fireRot[0] = new Vector3(0, 0, 0);
        //fireRot[1] = new Vector3(0, 0, 30);
        //fireRot[2] = new Vector3(0, 0, -30);

    }

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 활성화 되었을때 호출
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Enable();   // 오브젝트가 생성되면 입력을 받도록 활성화
        inputActions.Player.Move.performed += OnMove;   // performed일 때 Onmove 함수 실행하도록 연결
        inputActions.Player.Move.canceled += OnMove;    // canceled일 때 Onmove 함수 실행하도록 연결
        inputActions.Player.Fire.performed += OnFireStart;
        inputActions.Player.Fire.canceled += OnFireStop;
        inputActions.Player.Booster.performed += OnBooster;
        inputActions.Player.Booster.canceled += OffBooster;
    }

    

    private void OffBooster(InputAction.CallbackContext context)
    {
        boost = 1.0f;
    }

    private void OnBooster(InputAction.CallbackContext context)
    {
        boost *= 2.0f;
    }



    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 비활성화 되었을 때 호출
    /// </summary>
    private void OnDisable()
    {
        InputDisable();
    }

    void InputDisable()
    {
        inputActions.Player.Move.performed -= OnMove;   // 연결해 놓은 함수 해제(안전을 위해)
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Fire.performed -= OnFireStart;
        inputActions.Player.Booster.performed -= OnBooster;
        inputActions.Player.Booster.canceled -= OffBooster;
        inputActions.Player.Fire.canceled -= OnFireStop;
        inputActions.Player.Disable();  // 오브젝트가 사라질때 더 이상 입력을 받지 않도록 비활성화
    }


    private void Start()
    {
        Power = 1;
    }

    private void Update()
    {
        
    }


    //private void Update()
    //{
    //    //transform.position += (Speed * Time.deltaTime * dir);
    //}

    /// <summary>
    /// 일정 시간 간격(물리 업데이트 시간 간격)으로 호출
    /// </summary>
    private void FixedUpdate()
    {
        if (!isDead)
        {
        //transform.position += (Speed * Time.fixedDeltaTime * dir);
        // 이 스크립트 파일이 들어 있는 게임 오브젝트에서 Rigiboody2D 컴포넌트를 찾아 리턴.(없으면 null)
        // 그런데 GetComponent는 무거운 함수 => (Update나 FixedUpdate처럼 주기적 또는 자주 호촐되는 함수 안에서는 안쓰는 것이 좋다
        //rigid = GetComponent<Rigidbody2D>();

        // rigid.AddForce(Speed * Time.fixedDeltaTime * dir); // 관성이 있는 움직임을 할 때 유용
        rigid.MovePosition(transform.position + boost * Speed * Time.fixedDeltaTime * dir);     // 관성없는 움직임을 처리할 때 유용
        //fireTimeCount += Time.fixedDeltaTime;
        //if(isFire && fireTimeCount > fireInterval)
        //{
        //    Instantiate(Bullet, transform.position, Quaternion.identity);
        //    fireTimeCount = 0.0f;
        //}
        }
        else
        {
            rigid.AddForce(Vector2.left * 0.1f, ForceMode2D.Impulse);       // 죽었을 때 뒤로 돌면서 튕겨나가기
            rigid.AddTorque(10.0f);
        }
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        // Exception : 예외 상황( 무엇을 해야 할지 지정이 안되어있는 예외 일때 )
        //throw new NotImplementedException();    // NotImplementedException 을 실행해라. => 코드 구현을 알려주기 위해 강제로 죽이는 코드
        Vector2 inputdir = context.ReadValue<Vector2>();
        dir = inputdir;
        //Debug.Log("이동 입력");

        //dir.y > 0     // W를 눌렀다
        //dir.y == 0 // w,s 중 아무것도 안눌렀다,
        //dir.y < 0 // s를 눌렀다,
        anim.SetFloat("InputY", dir.y);
    }

    private void OnFireStart(InputAction.CallbackContext context)
    {
        //Debug.Log("발사");
        //float value = Random.Range(0.0f, 10.0f);      // value에는 0.0 ~ 10.0 의 랜덤값이 들어간다.
        //Instantiate(Bullet, transform.position, Quaternion.identity);
        //isFire = true;
        StartCoroutine(fireCoroutine);
    }

    private void OnFireStop(InputAction.CallbackContext context)
    {
        //isFire = false;
        //StopAllCoroutines();
        StopCoroutine(fireCoroutine);
    }

    IEnumerator Fire()
    {
        //yield return null;      // 다음 프레임에 이어서 시작해라

        //yield return new WaitForSeconds(1.0f);      // 1초 후에 이어서 시작해라

        while (true)
        {
            for (int i = 0; i < firePositionRoot.childCount; i++)
            {
                // Bullet이라는 프리팹을 firePosition[i]의 위치에 (0,0,0) 회전으로 만들어라
                GameObject obj = Instantiate(Bullet, firePositionRoot.GetChild(i).position, firePositionRoot.GetChild(i).rotation);

                // Instantiate(생성할 프리팹);        // 프리팹이 (0,0,0) 위치에 (0,0,0) 회전에 (1,1,1) 스케일로 만드러짐
                // Instantiate(생성할 프리팹, 생성할 위치, 생성될 때의 회전);

                //obj.transform.Rotate(fireRot[i]);
                //obj.transform.rotation = firePosition[i].rotation;  // 총알의 회전 값으로 firePosition[i]의 회전값을 그래도 사용한다.

                //Vector3 angle = firePosition[i].rotation.eulerAngles; // 현재 회전 값을 x,y,z축별로 몇도씩 회전 했는지 확인 가능
            }
            flash.SetActive(true);
            StartCoroutine(Flashoff());
            yield return new WaitForSeconds(fireInterval);
        }
    }


    IEnumerator Flashoff()
    {
        yield return new WaitForSeconds(0.1f);
        flash.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("OnCollisionEnter2D");        // Collider와 부딪쳤을 때 실행
        if (collision.gameObject.CompareTag("PowerUp"))
        {

            // 파워업 아이템을 먹었으면
            Power++;                                // 파워 증가
            Destroy(collision.gameObject);          // 파워업 오브젝트 삭제
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            //if(isDead == false)
            Dead();     // 적이랑 부딪치면 죽이기
        }
    }

    void Dead()
    {
        isDead = true;          // 죽었다고 표시
        GetComponent<Collider2D>().enabled = false;             // 콜라이더를 비활성화
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);      //폭팔 이팩트 생성
        InputDisable();                     // 입력 막기
        rigid.gravityScale = 1.0f;          // 충력으로 떨어지게 만들기
        rigid.freezeRotation = false;       // 회전 막아놓은 것 풀기
        StopCoroutine(fireCoroutine);       // 총을 쏘던 중이면 더이상 쏘지 않게 처리
    }
}
