using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Base : MonoBehaviour
{
    protected virtual void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            
        }
    }

    
}
