using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public class Player : MonoBehaviour, IFly, IDead
{
    PlayerInputActions inputActions;            // PlayerInputActions 타입이고 inputActions 이름을 가진 변수를 선언
    Rigidbody rigid;

    GroundChecker groundChecker;

    Animator anima;

    public float moveSpeed = 5.0f;
    public float rotateSpeed = 180.0f;
    public float jumpPower = 3.0f;

    float moveDir = 0.0f;
    float rotateDir = 0.0f;

    bool isJumping = false;

    Vector3 dir;
    Vector3 usePosition = Vector3.zero; // 플레이어가 오브젝트 사용을 확인하느 캡슐의 아래 지점 (플레이어 로컬 좌표 기준)
    float useRedius = 0.5f;             // 캡슐의 반지름
    float useHeight = 2.0f;             // 캡슐의 높이

    public Action onObjectUse { get; set; }
    public Action OnDie {get; set;}

    private void Awake()
    {
        inputActions = new PlayerInputActions();    // 인스턴스 생성. 실제 메로리를 할당 받고 사용할 수 있도록 만듬
        rigid = GetComponent<Rigidbody>();
        groundChecker = GetComponentInChildren<GroundChecker>();
        groundChecker.onGrounded += OnGround;
        anima = GetComponent<Animator>();

        usePosition = Vector3.forward;        // 기본적으로 플레이어의 앞(플레이어 로컬 좌표 기준)
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Use.performed += OnUse;
    }

    

    private void OnDisable()
    {
        inputActions.Player.Use.performed -= OnUse;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    

    private void FixedUpdate()
    {
        Move();
        Rotate();

        if (isJumping)
        {
            if(rigid.velocity.y < 0)
            {
                groundChecker.gameObject.SetActive(true);
            }
        }
    }

    void OnDrawGizmos()
    {
        // 플레이어가 오브젝트를 사용하는 범위 표시
        Vector3 newUsePosition = transform.rotation * usePosition;
        Gizmos.DrawWireSphere(this.transform.position + newUsePosition, useRedius);
        Gizmos.DrawWireSphere(this.transform.position + newUsePosition + transform.up * useHeight, useRedius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlatForm"))
        {
            Platform platform = collision.gameObject.GetComponent<Platform>();
            platform.onMove += OnMoveingObject;     // 델리게이트 연결
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlatForm"))
        {
            Platform platform = collision.gameObject.GetComponent<Platform>();
            platform.onMove -= OnMoveingObject;     // 델리게이트 해제
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {

        dir = context.ReadValue<Vector2>();     // 입력된 값을 읽어오기

        moveDir = dir.y;            // w : +1, s : -1   전진인지 후진인지 결정
        rotateDir = dir.x;          // a : -1, d : +1   좌회전인지 우회전인지 결정


        anima.SetBool("IsMove", !context.canceled);     // 이동키를 눌럿으면 true, 아니면 false
    }

    private void OnJump(InputAction.CallbackContext _)
    {
        if (!isJumping)     // 점프 중이 아닐 때만 점프
        {
            JumpStart();
        }
    }

    void Move()
    {
        rigid.MovePosition(rigid.position + moveSpeed * Time.fixedDeltaTime * moveDir * transform.forward);  //forward 정방향
    }

    void Rotate()
    {
        // 리지드바디로 회전 설정
        rigid.MoveRotation(rigid.rotation * Quaternion.AngleAxis(rotateDir * rotateSpeed * Time.fixedDeltaTime, transform.up));
        
        // Quaternion.Euler(0, rotateDir * rotateSpeed * Time.fixedDeltaTime, 0) // x, y 축은 회전  없고 y 기준으로 회전    // world 기준
        // Quaternion.AngleAxis(rotateDir * rotateSpeed * Time.fixedDeltaTime, transform.up) // 플레이어의 y축 기준으로 회전    // 오브젝트 기준
    }


     void OnGround()
    {
        isJumping = false;
    }

    void JumpStart()
    {
        // 플레이어의 위쪽 방향(up)으로 jumpPower만큼 즉시 힘을 추가한다.(질량 있음)
        rigid.AddForce(transform.up * jumpPower, ForceMode.Impulse);
        isJumping = true;

        groundChecker.gameObject.SetActive(false);
    }

    private void OnUse(InputAction.CallbackContext _)
    {
        anima.SetTrigger("Use");        // 아이템 사용 애니메이션 재생

        Vector3 newUsePosition = transform.rotation * usePosition;  // usePosition(로컬좌표)에 회전을 곱해서 월드좌표로 변환 됨

        //onObjectUse?.Invoke();
        Collider[] colliders = Physics.OverlapCapsule(      // 캡슐 모양에 겹치는 컬라이더가 있는지 체크
            transform.position + newUsePosition,               // 캡슐의 아래구의 중심선
            transform.position + newUsePosition + transform.up * useHeight, useRedius,     // 캡슐의 위쪽구의 중심점
            LayerMask.GetMask("UseableObjest"));            // 체크할 레이어

        if(colliders.Length > 0)        // 캡슐에 겹쳐진 UseableObjest 컬라이더가 한개 이상이다.
        {
            IUseavleObject useable = colliders[0].GetComponent<IUseavleObject>();   // 여러개가 있어도 하나만 처리
            if(useable != null)     // IUseavleObject를 가진 오브젝트이면
            {
                useable.Use();      // 사용하기
            }
        }

    }


    void OnMoveingObject(Vector3 delta)
    {
        rigid.velocity = Vector3.zero;          // 원래 플레이어의 벨로시티 제거
        rigid.MovePosition(rigid.position + delta);     // 플렛폼이 이동한 만큼 이동시키기
    }


    public void Fly(Vector3 flayVector)
    {
        rigid.velocity = Vector3.zero;
        rigid.AddForce(flayVector, ForceMode.Impulse);
    }

    public void Die()
    {
        inputActions.Player.Disable();      // 플레이어 입력처리 안함

        rigid.constraints = RigidbodyConstraints.None;  // 모든 회전이 가능하도록 고정
        rigid.angularDrag = 0.0f;       // 회전 마찰력 0으로 만들기
        rigid.AddForceAtPosition(-transform.forward, transform.position + transform.up * 2.0f, ForceMode.Impulse);  // 넘어질떄 머리쯤을 밀어서 뒤로 넘어지도록 만들기
        rigid.AddTorque(transform.up * 1.0f, ForceMode.Impulse);    // 넘어질때 돌면서 넘어지기

        anima.SetTrigger("Die");

        OnDie?.Invoke();        // 죽엇을 때 다른 클래스에서 해야할 일들을 실행 시키기
    }
}
