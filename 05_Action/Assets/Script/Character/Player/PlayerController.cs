using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputAction playerInput;

    public float moveSpeed = 5.0f;
    public float rotateSpeed = 180.0f;

    Vector3 moveDir = Vector3.zero;
    Quaternion targerRotation = Quaternion.identity;

    private void Awake()
    {
        playerInput = new PlayerInputAction();
    }

    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.Move.canceled += OnMove;
    }


    private void OnDisable()
    {
        playerInput.Player.Move.canceled -= OnMove;
        playerInput.Player.Move.performed -= OnMove;
        playerInput.Player.Disable();
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        moveDir.z = dir.y;
        moveDir.y = 0.0f;
        moveDir.x = dir.x;

        Quaternion cameraYRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        moveDir = cameraYRotation * moveDir;
        if (!context.canceled)
        {
            targerRotation = Quaternion.LookRotation(moveDir);
        }
    }


    void Move()
    {
        transform.Translate(Time.deltaTime * moveSpeed * moveDir, Space.World);
    }

    void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targerRotation, Time.deltaTime * rotateSpeed);
    }
}
