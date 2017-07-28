using UnityEngine;
using System.Collections;
using System.IO; 
using System; 
using System.Text;
using System.Collections.Generic;


public class RankingSeting : MonoBehaviour {

    public string guitxt = "";
    [SerializeField, Header("ランキングリスト ")]
    public List<float> _Rank;
    [SerializeField, Header("選ばれたStage ")]
    private int StageNum;

    public testScri test;
    [SerializeField, Header("Newイラスト ")]
    private GameObject[] _NewIme;

    // Use this for initialization
    void Awake() {
        StageNum = StageSelectController.getStageNum();

        if (StageNum == 0) StageNum = 1;


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
        int _Count = 0;
        foreach (var n in _Rank) {
            if (_Count >= 0)
            {
                _Count++;
                if (n <= num)
                {
                    if (_Count <= 3)
                    {
                        _NewIme[_Count - 1].SetActive(true);
                        _Count = -1;
                    }
                }
            }
          
        }
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
           Application.streamingAssetsPath + "//FileRead0" + StageNum.ToString() + ".txt",
            System.Text.Encoding.GetEncoding("utf-8"));

        guitxt = reder.ReadLine();
        reder.Close();
    }
    public void WritingFile()
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + "/streamingAssets" + "/FileRead0" + StageNum.ToString() + ".txt", false, System.Text.Encoding.GetEncoding("utf-8")); //true=追記 false=上書き

        for (int i = 0; i < _Rank.Count; i++)
        {
            sw.Write(_Rank[i] + ",");
        }
        sw.Flush();
        sw.Close();

    }
}
