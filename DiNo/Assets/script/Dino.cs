using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dino : MonoBehaviour
{
    Dinoaction input;
    Rigidbody2D rigid;

    Animator anima;

    public float jumpPower = 1.0f;          // 점프력
    bool jumpTime = true;                   // 점프 중인지 아닌지 판단 ( false면 점프중)

    public Action isDie;

    private void Awake()
    {
        input = new Dinoaction();
        rigid = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        input.Dino.Enable();
        input.Dino.Move.started += Jump;
    }

    private void OnDisable()
    {
        input.Dino.Move.started -= Jump;
        input.Dino.Disable();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpTime = true;        // 땅에 닿으면 true로 바꾸기
            anima.SetBool("Jump", false);       // 점프 애니메이션 끄기
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }


    private void Jump(InputAction.CallbackContext _)
    {
        if (jumpTime)               // 점프 중이 아니면
        {
            jumpTime = false;       // 점프중으로 바꾸기
            rigid.velocity = Vector2.up * jumpPower;    // 점프하기
            anima.SetBool("Jump", true);        // 점프 애니메이션 키기
        }
    }

    private void Die()
    {
        input.Disable();            // 죽으면 조작막기
        anima.SetTrigger("Die");    // Die 애니메이션 키기
        rigid.AddForce(jumpPower * Vector2.left, ForceMode2D.Impulse);          // jumpPower만큼 뒤로 날라가기
        isDie?.Invoke();
    }

}

