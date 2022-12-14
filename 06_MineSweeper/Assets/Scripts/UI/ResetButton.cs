using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    enum ButtonState
    {
        Normal = 0,
        Surpise,
        GameClear,
        GameOver
    }

    public Sprite[] buttonSprites;

    ButtonState state = ButtonState.Normal;

    ButtonState State
    {
        get => state;
        set
        {
            if(state != value)
            {
                state = value;
                image.sprite = buttonSprites[(int)state];
            }
        }
    }

    Image image;
    Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Inst;
        // 각 상황별 스프라이트 변경
        gameManager.onGameClear += () => State = ButtonState.GameClear;
        gameManager.onGameOver += () => State = ButtonState.GameOver;

        gameManager.Board.onBoardPress += () =>
        {
            if (gameManager.IsPlaying)
                State = ButtonState.Surpise;
        };
        gameManager.Board.onBoardRelease += () =>
        {
            if (gameManager.IsPlaying)
            {
                State = ButtonState.Normal;
            }
        };
        button.onClick.AddListener(() =>
        {
            gameManager.GameReset();
            State = ButtonState.Normal;
        });
    }
}
