using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

public class testScri : MonoBehaviour {

    public string guitxt = "";


    private string musicName; // 読み込む譜面の名前
    private string level; // 難易度
    private TextAsset csvFile; // CSVファイル
    private List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト
    private int height = 0; // CSVの行数

    void Start()
    {
        StreamReader reder;
        reder = new StreamReader(
            Application.streamingAssetsPath + "//FileRead01.txt",
            System.Text.Encoding.GetEncoding("utf-8"));
        while (reder.Peek() > -1)
        {
            string line = reder.ReadLine();
            guitxt = line;
            csvDatas.Add(line.Split(',')); // リストに入れる
            height++; // 行数加算
        }
        transform.GetComponent<Text>().text = guitxt;
        reder.Close();
    }
        // Update is called once per frame
        void Update () {
        //transform.GetComponent<Text>().text = (Application.dataPath + "/StreamingAssets" + "/FileRead0" + StageSelectController.getStageNum().ToString() + ".txt");
    }
 
}
