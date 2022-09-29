using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bird : MonoBehaviour
{
    BirdInputAction input;
    Rigidbody2D rigid;
    [Range(1.0f, 10.0f)]
    public float jumpPower = 5.0f;

    float pitchMaxAngle = 45.0f;

    private void Awake()
    {
        input = new BirdInputAction();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()     // 오브젝트가 활성화 될 때
    {
        input.Bird.Enable();
        input.Bird.Space.started += OnJump;
        input.Bird.MouseLeft.started += OnJump;
    }


    private void OnDisable()    // 오브젝트가 비활성화 될 때
    {
        input.Bird.MouseLeft.started -= OnJump;
        input.Bird.Space.started -= OnJump;
        input.Bird.Disable();
    }
    private void OnJump(InputAction.CallbackContext _)
    {
        rigid.velocity = Vector2.up * jumpPower;
    }

    private void FixedUpdate()      // 물리 업데이트 주기 마다(고정된 신간)
    {
        float veloictiyY = Mathf.Clamp(rigid.velocity.y, -jumpPower, jumpPower);
        float angle = (veloictiyY / jumpPower) * pitchMaxAngle;

        rigid.MoveRotation(angle);
    }


}
