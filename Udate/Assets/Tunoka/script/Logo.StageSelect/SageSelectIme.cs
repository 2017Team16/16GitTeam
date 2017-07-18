﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SageSelectIme : MonoBehaviour {

    private StageSelectController _StageSelectController;
    [SerializeField, Header("自分の番号")]
    private int _MyNumber;
    private int _RootNumber;
    [SerializeField, Header("元の画像")]
    private Sprite _protoIme;
    [SerializeField, Header("変更後の画像")]
    private Sprite _ChangeIme;
    [SerializeField, Header("サイズ変更")]
    private bool _ChangeSize;
    private Vector3 _StateSize;
    
    [SerializeField, Header("往復速度")]
    private float speed = 5;
    [SerializeField, Header("移動範囲")]
    private float range = 1;

    private Vector3 origin;//起点（移動の中心）
    private float myTime = 0.0f;

    // Use this for initialization
    void Start () {
        _StageSelectController = GameObject.Find("StageSelectController").GetComponent<StageSelectController>();
        _StateSize = transform.localScale;

        origin = transform.position;
        if (_protoIme.ToString() == "UIMask (UnityEngine.Sprite)") speed = 0;
    }

    // Update is called once per frame
    void Update () {
        _RootNumber = _StageSelectController.GetCursorNum();//コントローラーのカーソルのいる番号を持ってくる

        if (_RootNumber == _MyNumber)
        {
            if (_ChangeSize == true)//サイズ調整が必要な時
            {
                transform.localScale = new Vector3(2,2,2);
            }
            transform.GetComponent<Image>().sprite = _ChangeIme;
            myTime += 1 / 60.0f;
            transform.position =
                            origin + Vector3.up * Mathf.Sin(myTime * speed) * range;
            return;
        }
        transform.localScale = _StateSize;
        if (_RootNumber < 0) return;
        transform.GetComponent<Image>().sprite = _protoIme;
        myTime = 0.0f;
        transform.position = origin;


    }

}
