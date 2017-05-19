﻿using UnityEngine;
using System.Collections;
using System.IO; 
using System; 
using System.Text;
using System.Collections.Generic;


public class RankingSeting : MonoBehaviour {

    public string guitxt = "";
    [SerializeField, Header("ランキングリスト ")]
    public List<float> _Rank;

    // Use this for initialization
    void Awake() {

        ReadFile();//テキストの読み込み
        string[] test = guitxt.Split(',');//,で区切る
        for (int i = 0; i < test.Length - 1; i++)
        {
            if (i < 5)//六個目以降は読み取らない
            {
                _Rank.Add(float.Parse(test[i]));//リストの中に入れる
            }
        }
        _Rank.Sort();//リストの中を整理
        _Rank.Reverse();//並びを逆にする
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_Rank.Count > 5)
        {
            _Rank.RemoveAt(5);//リストの中を整理
        }
    }
    public void RankListIn(float num)//リザルトを更新
    {
        _Rank.Add(num);
        _Rank.Sort();//リストの中を整理
        _Rank.Reverse();//並びを逆にする
        WritingFile();//リザルトを書き直す
    }
    public float getRank(int num)//リザルト取得用
    {
        return _Rank[num-1];
    }
    void ReadFile()
    {
        StreamReader reder;
        reder = new StreamReader(
           Application.dataPath + "/Tunoka/Resources/" + "FileReadTest.txt",
            System.Text.Encoding.GetEncoding("shift_jis"));

        guitxt = reder.ReadLine();
        reder.Close();
    }
    public void WritingFile()
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + "/Tunoka/Resources/" + "FileReadTest.txt", false, System.Text.Encoding.GetEncoding("shift_jis")); //true=追記 false=上書き

        for (int i = 0; i < _Rank.Count; i++)
        {
            sw.Write(_Rank[i] + ",");
        }
        sw.Flush();
        sw.Close();

    }
}
