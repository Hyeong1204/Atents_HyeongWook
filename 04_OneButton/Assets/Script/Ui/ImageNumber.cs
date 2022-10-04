using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public GameObject digitPrefab;

    int expectedLength = 6;
    public Sprite[] numberImages;
    Transform[] numberTransform;

    
    public int MaxScore = 0;

    List<Image> digits;        // 0번째가 1의 자리 , 1번째가 10의 자리

    private void Awake()
    {
        digits = new List<Image>(transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            digits.Add(transform.GetChild(i).GetComponent<Image>());
        }

        numberTransform = new Transform[transform.childCount];
        for(int i = 0; i<transform.childCount; i++)
        {
            numberTransform[i] = transform.GetChild(i);
        }
    }

    public int number;

    public int Number
    {
        get => number;
        set
        {
            if (number != value)        // 값이 변경되었을 때만 실행
            {
                int teampNum = value;

                List<int> remainders = new List<int>(expectedLength);        // 자료구조를 만들 때는 기본 크기를 잡아주는 편이 좋다

                // 자리수별로 숫자 분리하기
                while(teampNum != 0)                // teampNum가 123일때
                {
                    remainders.Add(teampNum % 10);  // 3 -> 2 -> 1
                    teampNum /= 10;                 // 12 -> 1 -> 0
                }


                // 자리수에 맞게 digits 증가 또는 감소
                int diff = remainders.Count - digits.Count;
                if(diff > 0)
                {
                    // digits 증가, 나머지들의 자리수가 더 길음
                    for (int i = 0; i < diff; i++)
                    {
                        GameObject obj = Instantiate(digitPrefab, transform);
                        obj.name = $"Digit{Mathf.Pow(10, digits.Count)}";
                        digits.Add(obj.GetComponent<Image>());
                    }
                }
                else if (diff < 0)
                {
                    // digits 감소. 나머지들의 자리수가 더 작음
                    for(int i = digits.Count + diff; i< digits.Count; i++)
                    {
                        digits[i].gameObject.SetActive(false);
                    }
                }


                for(int i = 0; i < remainders.Count; i++)
                {
                    digits[i].sprite = numberImages[remainders[i]];
                    digits[i].gameObject.SetActive(true);
                }

                number = value;
            }
        }
    }

    private void Start()
    {
        
    }

    //private void Update()
    //{
    //    if (currentScore < MaxScore)
    //    {
    //        currentScore += Time.deltaTime * scoreTime;
    //        currentScore = Mathf.Min(currentScore, MaxScore);
    //        Number = (int)currentScore;
    //    }

    //}
}
