using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    protected PlayerInput input;
    private Rigidbody2D rigid;
    private Vector3 dir;

    public float speed = 1.0f;

    private void Awake()
    {
        input = new PlayerInput();
        rigid = GetComponent<Rigidbody2D>();
    }


    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Move.performed += onMove;
        input.Player.Move.canceled += onMove;
    }


    private void OnDisable()
    {
        input.Player.Move.canceled -= onMove;
        input.Player.Move.performed -= onMove;
        input.Player.Disable();
    }
    protected virtual void onMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(transform.position += speed * Time.fixedDeltaTime * dir);
    }
}
