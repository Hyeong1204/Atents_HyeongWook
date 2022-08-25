using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //public delegate void DelegateName();        // 이런 종류의 델리게이트가 있다. (리턴없고 파라메터도 없는 함수를 저장하는 델리게이트)

    //public DelegateName del;    // DelegateName 타입으로 del이라는 이름의 델리게이트를 만듬
    //Action del2;                // 리턴타입이 void, 파라메터도 없는 델리게이트 del2를 만듬
    //Action<int> del3;           // 리턴타입이 void, 파라메터는 int 하나인 델리게이트 del3를 만듬
    //Func<int, float> del4;      // 리턴타입이 int고 파라메터는 float 하나인 델리게이트 del4를 만듬

    PlayerinputAction inputActions;
    Vector3 dir;                // 이동 방향(입력에 따라 변경됨)
    public float Speed = 1.0f;  // 플레이어의 이동 속도(초당 이동 속도)
    float boost = 1.0f;
    // Awake > OnEnble > Start : 대체적으로 이 순서
    Rigidbody2D rigid;

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 생성된 직후에 호출
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerinputAction();
        rigid = GetComponent<Rigidbody2D>();        // 한번만 찾고 저장해서 계속 쓰기(메모리 더 쓰고 성능 아끼기)
    }

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 활성화 되었을때 호출
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Enable();   // 오브젝트가 생성되면 입력을 받도록 활성화
        inputActions.Player.Move.performed += OnMove;   // performed일 때 Onmove 함수 실행하도록 연결
        inputActions.Player.Move.canceled += OnMove;    // canceled일 때 Onmove 함수 실행하도록 연결
        inputActions.Player.Fire.performed += OnFire;
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
        inputActions.Player.Move.performed -= OnMove;   // 연결해 놓은 함수 해제(안전을 위해)
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Fire.performed -= OnFire;
        inputActions.Player.Booster.performed -= OnBooster;
        inputActions.Player.Booster.canceled -= OffBooster;
        inputActions.Player.Disable();  // 오브젝트가 사라질때 더 이상 입력을 받지 않도록 비활성화
    }

    private void Start()
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
        //transform.position += (Speed * Time.fixedDeltaTime * dir);
        // 이 스크립트 파일이 들어 있는 게임 오브젝트에서 Rigiboody2D 컴포넌트를 찾아 리턴.(없으면 null)
        // 그런데 GetComponent는 무거운 함수 => (Update나 FixedUpdate처럼 주기적 또는 자주 호촐되는 함수 안에서는 안쓰는 것이 좋다
        //rigid = GetComponent<Rigidbody2D>();

        // rigid.AddForce(Speed * Time.fixedDeltaTime * dir); // 관성이 있는 움직임을 할 때 유용
        rigid.MovePosition(transform.position + boost * Speed * Time.fixedDeltaTime * dir);     // 관성없는 움직임을 처리할 때 유용
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        // Exception : 예외 상황( 무엇을 해야 할지 지정이 안되어있는 예외 일때 )
        //throw new NotImplementedException();    // NotImplementedException 을 실행해라. => 코드 구현을 알려주기 위해 강제로 죽이는 코드
        Vector2 inputdir = context.ReadValue<Vector2>();
        dir = inputdir;
        //Debug.Log("이동 입력");
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("발사");
    }


}
