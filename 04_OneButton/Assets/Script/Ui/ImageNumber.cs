using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public Sprite[] numberImages;
    Transform[] numberTransform;

    float currentScore = 0.0f;
    float scoreTime = 70.0f;
    public int MaxScore = 0;

    Image[] disgits;

    private void Awake()
    {
        disgits = new Image[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            disgits[i] = transform.GetChild(i).GetComponent<Image>();
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
            number = value;

            int mod = number % 10;
            int mod2 = (number / 10) % 10;
            int mod3 = number / 100;

            if(number >= 10)
            {
                numberTransform[1].gameObject.SetActive(true);
            }
            if(number >= 100)
            {
                numberTransform[0].gameObject.SetActive(true);
            }


            disgits[transform.childCount - 1].sprite = numberImages[mod];
            disgits[transform.childCount - 2].sprite = numberImages[mod2];
            disgits[transform.childCount - 3].sprite = numberImages[mod3];
        }
    }

    private void Start()
    {
        currentScore = 0.0f;
    }

    private void Update()
    {
        if (currentScore < MaxScore)
        {
            currentScore += Time.deltaTime * scoreTime;
            currentScore = Mathf.Min(currentScore, MaxScore);
            Number = (int)currentScore;
        }

    }
}
