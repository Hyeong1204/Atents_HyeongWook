using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Pipe : MonoBehaviour
{
    
    public ImageNumber imageNumber;
    public TMP_InputField inputField;


    private void Start()
    {
        inputField.onValueChanged.AddListener(OnInputChanged);
    }

    private void OnInputChanged(string Text)
    {
        if (Text != "")
        {
            imageNumber.maxNumber = int.Parse(Text);
        }
        else
        {
            imageNumber.maxNumber = 0;
        }
    }

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            //imageNumber.number = 123465;
        }
    }

    
}
