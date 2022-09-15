using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;            // PlayerInputActions 타입이고 inputActions 이름을 가진 변수를 선언

    public float moveSpeed = 5.0f;
    public float rotateSpeed = 180.0f;

    Vector3 dir;

    private void Awake()
    {
        inputActions = new PlayerInputActions();    // 인스턴스 생성. 실제 메로리를 할당 받고 사용할 수 있도록 만듬
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }


    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * dir);
        transform.Rotate(rotateSpeed * Time.deltaTime * dir);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }
}
