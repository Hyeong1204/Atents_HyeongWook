using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ImageNumber : TestBase
{
    [Range(-99, 999)]
    public int testNumber = 0;
    ImageNumber imageNum;

    private void OnValidate()
    {
        if(imageNum != null)
        {
            imageNum.Number = testNumber;
        }
    }

    private void Start()
    {
        imageNum = FindObjectOfType<ImageNumber>();
    }

    protected override void Test1(InputAction.CallbackContext _)
    {
        imageNum.Number++;
    }

    protected override void Test2(InputAction.CallbackContext _)
    {
        imageNum.Number--;
    }

    protected override void Test3(InputAction.CallbackContext _)
    {
        imageNum.Number = 999;
    }

    protected override void Test4(InputAction.CallbackContext _)
    {
        imageNum.Number = -99;
    }
}
