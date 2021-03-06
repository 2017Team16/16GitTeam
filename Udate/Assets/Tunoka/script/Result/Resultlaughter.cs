﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Resultlaughter : MonoBehaviour {


    [SerializeField]
    Image[] images = new Image[4];
    [SerializeField]
    Sprite[] numberSprites = new Sprite[10];
    private string _Normal;

    // Use this for initialization
    void Start () {
        _Normal = "";
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_Normal == transform.GetComponent<Text>().text) return;
        _Normal = transform.GetComponent<Text>().text;
        SetNumbers(int.Parse(transform.GetComponent<Text>().text));
    }
    void SetNumbers(int num)
    {
        int str = num % 10;
        images[0].sprite = numberSprites[str];

        num = num / 10;
        str = num % 10;
        images[1].sprite = numberSprites[str];

        num = num / 10;
        str = num % 10;
        images[2].sprite = numberSprites[str];

        num = num / 10;
        str = num % 10;
        images[3].sprite = numberSprites[str];

        num = num / 10;
        str = num % 10;
        images[4].sprite = numberSprites[str];

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
