using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 이동속도
    /// </summary>
    public float moveSpeed = 3.5f;

    /// <summary>
    /// 회전 속도
    /// </summary>
    public float rotateSpeed = 3.5f;

    /// <summary>
    /// 이번 프레임에 움직여야 할 이동량
    /// </summary>
    Vector3 movedelta;

    /// <summary>
    /// 이번 프레임에 회전해야 할 회전량
    /// </summary>
    float rotateDelta;

    // 컴포넌트 -----------------------------------------------------------------------------------
    PlayerInputAction playerInput;
    CharacterController controller;

    private void Awake()
    {
        playerInput = new PlayerInputAction();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += OnMoveInput;
        playerInput.Player.Move.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
        playerInput.Player.Move.canceled -= OnMoveInput;
        playerInput.Player.Move.performed -= OnMoveInput;
        playerInput.Player.Disable();
    }

    private void Update()
    {
        controller.Move(moveSpeed * Time.deltaTime * movedelta);
        transform.Rotate(0, rotateDelta * Time.deltaTime, 0, Space.World);
    }

    /// <summary>
    /// 입력을 하거나 취소했을 때 실행될 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        //movedelta = moveInput.y * moveSpeed * transform.forward;    // 회전 입력 저정하기
        movedelta.x = moveInput.x;
        movedelta.y = 0.0f;
        movedelta.z = moveInput.y;

        rotateDelta = moveInput.x * rotateSpeed;                    // 회전 입력 저장하기
    }
}
