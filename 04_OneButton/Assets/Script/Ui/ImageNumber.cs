using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageNumber : MonoBehaviour
{
    public Sprite[] numberImages;

    Image[] disgits;

    private void Awake()
    {
        disgits = new Image[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            disgits[i] = transform.GetChild(i).GetComponent<Image>();
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
            disgits[transform.childCount - 1].sprite = numberImages[mod];
            disgits[transform.childCount - 2].sprite = numberImages[mod2];
            disgits[transform.childCount - 3].sprite = numberImages[mod3];
        }
    }
}
