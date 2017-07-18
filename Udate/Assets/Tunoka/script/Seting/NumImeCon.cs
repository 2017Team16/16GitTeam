using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class NumImeCon : MonoBehaviour {

    [SerializeField]
    Image[] images = new Image[4];
    [SerializeField]
    Sprite[] numberSprites = new Sprite[10];

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        SetNumbers(int.Parse(transform.GetComponent<Text>().text));
    }
    void SetNumbers(int num)
    {

        int str = num % 10;
        images[0].sprite = numberSprites[str];
        for (int i = 1; i < images.Length ; i++)
        {
            num = num / 10;
            str = num % 10;
            images[i].sprite = numberSprites[str];
        }


        for (int i = images.Length - 1; i > 0; i--)
        {
            if (images[i].sprite.name == "result-score-nb_0")
            {
                images[i].sprite = numberSprites[10];
            }
            else
            {
                return;
            }
        }

    }
}
