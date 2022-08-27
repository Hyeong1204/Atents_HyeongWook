
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInput playerinput;
    Rigidbody2D rigid;
    Animator anim;

    Vector3 dir;

    public float speed = 1.0f;

    private void Awake()
    {
        playerinput = new PlayerInput();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerinput.Player.Enable();
        playerinput.Player.Move.performed += OnMove;
        playerinput.Player.Move.canceled += OnMove;
    }


    private void OnDisable()
    {
        playerinput.Player.Move.performed -= OnMove;
        playerinput.Player.Move.canceled -= OnMove;
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(transform.position + speed * Time.fixedDeltaTime * dir);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
        
        anim.SetFloat("InputX", dir.x);
    }



}
