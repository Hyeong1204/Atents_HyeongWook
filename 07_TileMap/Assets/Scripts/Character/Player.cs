using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 3.0f;

    Animator anim;
    Vector2 dir;

    PlayerInputAction inputActions;
    Rigidbody2D rigid;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        inputActions = new PlayerInputAction();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + speed * Time.fixedDeltaTime * dir);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
        inputActions.Player.Attack.performed += OnAttack;
    }


    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Move.canceled -= OnStop;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
        anim.SetFloat("InputX", dir.x);
        anim.SetFloat("InputY", dir.y);
        anim.SetBool("IsMove", true);
    }

    private void OnStop(InputAction.CallbackContext context)
    {
        dir = Vector2.zero;
        anim.SetBool("IsMove", false);
    }

    private void OnAttack(InputAction.CallbackContext _)
    {

    }
}
