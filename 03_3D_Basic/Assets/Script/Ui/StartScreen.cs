using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class StartScreen : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Close();
        GameManager.Inst.GameStart();
    }

    private void Update()
    {
        // 현재 사용중인 키보드에서 어떤 키든 이 프레임에 늘러졌을 때 true다.
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Close();
            GameManager.Inst.GameStart();
        }
    }

    void Close()
    {
        this.gameObject.SetActive(false);
    }
}
