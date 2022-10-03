using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Pipe : MonoBehaviour
{
    
    public ImageNumber imageNumber;


    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            imageNumber.MaxScore += 10;
        }
    }

    
}
