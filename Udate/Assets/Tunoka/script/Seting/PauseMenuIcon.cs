using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PauseMenuIcon : MonoBehaviour {
    [SerializeField, Header("ResultEvent")]
    private Pause _Pause;

    [SerializeField, Header("画像リスト")]
    private Sprite[] _ImeS;

    [SerializeField, Header("表示番号")]
    public int _ListNum = 0;
    [SerializeField, Header("サイズ変更が必要な番号")]
    private int _ChangeSizeTr;
    [SerializeField, Header("変更サイズ")]
    private Vector3 _ChangeSize;
    private Vector3 _StateSize;
    void Start()
    {
        _StateSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        _ListNum = _Pause._ListNum;
        if (_ListNum >= _ImeS.Length)
        {
            print("サイズ外を指定外か終了しています");
            return;
        }

        if (_ChangeSizeTr == _ListNum)//サイズ調整が必要な画像
        {
            transform.localScale = _ChangeSize;
        }
        else
        {
            transform.localScale = _StateSize;
        }
        transform.GetComponent<Image>().sprite = _ImeS[_ListNum];
    }
}
