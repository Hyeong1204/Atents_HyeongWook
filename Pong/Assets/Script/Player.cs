using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInput input;
    private Vector3 dir;

    public float speed = 1.0f;

    private void Awake()
    {
        input = new PlayerInput();
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
    private void onMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * dir);
    }

}
