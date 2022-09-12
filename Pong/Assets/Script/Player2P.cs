using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2P : Player
{
    private void OnEnable()
    {
        input.Player2P.Enable();
        input.Player2P.Move.performed += onMove;
        input.Player2P.Move.canceled += onMove;
    }

    private void OnDisable()
    {
        input.Player2P.Move.canceled -= onMove;
        input.Player2P.Move.performed -= onMove;
        input.Player2P.Disable();
    }

    protected override void onMove(InputAction.CallbackContext context)
    {
        base.onMove(context);
    }
}
