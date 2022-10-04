using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public int expectedLength = 9;
    public GameObject digitPrefab;
    public Sprite[] numberImages = new Sprite[10];

    List<Image> digits; // 0번째가 1자리, 1번째가 10자리
    List<int> remainders;

    float numberChangeSpeed = 200.0f;       // 숫자 이미지가 변하는 속도
    float currentNumber = 0.0f;             // 현재 보여질 값

    public int maxNumber = 0;               // 도달할 목표 값
    int number = 0;
    public int Number
    {
        get => number;
        set
        {
            if (number != value)    // 값이 변경되었을 때만 실행하라.
            {
                int tempNum = value;
               

                // 자리수별로 숫자 분리하기
                do
                {
                    remainders.Add(tempNum % 10);   // 3 -> 2 -> 1
                    tempNum /= 10;                  // 12 -> 1 -> 0
                } while (tempNum != 0);                 //123으로 시작했을 때


                    // 자리수에 맞게 digits 증가 또는 감소
                    int diff = remainders.Count - digits.Count;
                if (diff > 0)
                {
                    // digits 증가. 나머지들의 자리수가 더 기니까
                    for (int i = 0; i < diff; i++)
                    {
                        GameObject obj = Instantiate(digitPrefab, transform);
                        obj.name = $"Digit{Mathf.Pow(10, digits.Count)}";   // Mathf.Pow는 제곱근 함수(10의 digits.Count제곱만큼 계산)
                        digits.Add(obj.GetComponent<Image>());              // digits가 추가 되기 때문에 for문 한 바퀴돌면 digits가 1증가한다.
                    }
                }
                else if (diff < 0)
                {
                    // digits 감소. 나머지들의 자리수가 더 작으니까
                    for (int i = digits.Count + diff; i < digits.Count; i++)
                    {
                        digits[i].gameObject.SetActive(false);
                    }
                }

                // 자리수별로 숫자 설정
                for (int i = 0; i < remainders.Count; i++)          // remainders 갯수만큼 반복
                {
                    digits[i].sprite = numberImages[remainders[i]];
                    digits[i].gameObject.SetActive(true);
                }

                number = value;
            }
        }
    }

    private void Awake()
    {
        digits = new List<Image>(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            digits.Add(transform.GetChild(i).GetComponent<Image>());
        }
        remainders = new List<int>(expectedLength); // 자료구조를 만들 때는 기본 크기를 잡아주는 편이 좋다.
    }

    private void Update()
    {
        if(Number != maxNumber)     // Number가 maxNumber와 같아졌는지 확인, 다를 때만 실행
        {
            float dir = (currentNumber > maxNumber) ? -1 : 1;       // 삼항 연산자. 조건식이 참일때 -1, 거짓일때 1. 변화하는 방향 구하기

            currentNumber += dir * Time.deltaTime * numberChangeSpeed;      // 방향에 따라 초당 numberChangeSpeed만큼 currentNumber 변화
            if (dir > 0)
            {
                currentNumber = Mathf.Min(currentNumber, maxNumber);        // 방향이 증가일 때 목표인 maxNumber 넘친 경우 maxNumber로 설정
            }
            else
            {
                currentNumber = Mathf.Max(currentNumber, maxNumber);        // 방향이 감소일 때 목표인 maxNumber 밑으로 내려간 경우 maxNumber로 설정
            }

            remainders.Clear();             // 리스트 비우기

            Number = (int)currentNumber;    // Number에 (int)currentNumber(소수점 제거) 넣기
        }
    }

}
