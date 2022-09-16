using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;            // PlayerInputActions 타입이고 inputActions 이름을 가진 변수를 선언
    Rigidbody rigid;

    GroundChecker groundChecker;

    public float moveSpeed = 5.0f;
    public float rotateSpeed = 180.0f;
    public float jumpPower = 3.0f;

    float moveDir = 0.0f;
    float rotateDir = 0.0f;

    bool isJumping = false;

    Vector3 dir;

    private void Awake()
    {
        inputActions = new PlayerInputActions();    // 인스턴스 생성. 실제 메로리를 할당 받고 사용할 수 있도록 만듬
        rigid = GetComponent<Rigidbody>();
        groundChecker = GetComponentInChildren<GroundChecker>();
        groundChecker.onGrounded += OnGround;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
    }


    private void OnDisable()
    {
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

    private void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();

        moveDir = dir.y;            // w : +1, s : -1   전진인지 후진인지 결정
        rotateDir = dir.x;          // a : -1, d : +1   좌회전인지 우회전인지 결정
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
}
