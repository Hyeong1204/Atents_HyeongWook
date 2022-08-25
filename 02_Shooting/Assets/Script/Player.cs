using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerinputAction inputActions;
    Vector3 dir;
    public float Speed = 1.0f;
    // Awake > OnEnble > Start : 대체적으로 이 순서

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 생성된 직후에 호출
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerinputAction();
    }

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 활성화 되었을때 호출
    /// </summary>
    private void OnEnable()
    {
        inputActions.Player.Enable();   // 오브젝트가 생성되면 입력을 받도록 활성화
        inputActions.Player.Move.performed += OnMove;   // performed일 때 Onmove 함수 실행하도록 연결
        inputActions.Player.Move.canceled += OnMove;    // canceled일 때 Onmove 함수 실행하도록 연결
        inputActions.Player.Fire.started += OnFire;
    }

    

    /// <summary>
    /// 이 스크립트가 들어있는 게임 오브젝트가 비활성화 되었을 때 호출
    /// </summary>
    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;   // 연결해 놓은 함수 해제(안전을 위해)
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Fire.started -= OnFire;
        inputActions.Player.Disable();  // 오브젝트가 사라질때 더 이상 입력을 받지 않도록 비활성화
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.position += (Speed * Time.deltaTime * dir);
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        // Exception : 예외 상황( 무엇을 해야 할지 지정이 안되어있는 예외 일때 )
        //throw new NotImplementedException();    // NotImplementedException 을 실행해라. => 코드 구현을 알려주기 위해 강제로 죽이는 코드
        Vector2 inputdir = context.ReadValue<Vector2>();
        dir = inputdir;
        Debug.Log("이동 입력");
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("발사");
    }


}
