using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleTap : MonoBehaviour
{

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)        // 키보드 나 마우스 왼쪽 버튼을 누르면
        {
            GameManager.Inst.GameStart();       // 게임 메니저에게 시작 신호를 보냄
            Destroy(this.gameObject);           // 자기 자신을 삭제
        }
    }
}
