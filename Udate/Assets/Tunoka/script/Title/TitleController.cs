﻿using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {


    private TitleAnimController _TAnim;
    private SceneChanger SChang;

    [SerializeField, Header("ルール説明テキスト")]
    private GameObject _RuleIme;


    [SerializeField, Header("Cursor番号")]
    public float _CursorNum = 0;
    // Use this for initialization
    void Start () {
        _TAnim = transform.GetComponent<TitleAnimController>();
        SChang = transform.GetComponent<SceneChanger>();
       _CursorNum = 0;
}

    // Update is called once per frame
    void Update()
    {

        if (_CursorNum == 3)//説明画面表示中
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _CursorNum = 0;
            }
        }
        else if (_TAnim.GetAnimeFlag() )
        {
            _RuleIme.SetActive(false);
            if (_CursorNum == 0 && Input.GetKeyDown(KeyCode.Space))
            {
                SceneChangeButton();
            }
            if (_CursorNum == 1 && Input.GetKeyDown(KeyCode.Space))
            {
                RuleButton();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                _CursorNum = 0;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _CursorNum = 1;
            }
        }

    }
    public void SceneChangeButton()
    {

        print("シーンチェンジだよ");
        SChang.FadeOut();

    }
    public void RuleButton()
    {
        print("説明");
        _CursorNum = 3;
        _RuleIme.SetActive(true);
    }

    
}
