using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {


    [SerializeField, Header("確認用スコア(いじらない)")]
    private float _checkScore;
    public static float _score;
    [SerializeField, Header("確認用連続チェイン(いじらない)")]
    private float _checkChain;
    public static float _Maxchain;
    [SerializeField, Header("確認用一回の最大数(いじらない)")]
    private float _checkMaxpush;
    public static float _MaxPush;
    [SerializeField, Header("確認用倒した数(いじらない)")]
    private float _checkKill;
    public static float _killNum;
    [SerializeField, Header("確認用体力(いじらない)")]
    private float _checkLife;
    public static float _LifeNum;
    [SerializeField, Header("画面表示用のオブジェクト")]
    private Text _displayScore;

    private OlderBrotherHamster _OlderBrotherHamster;

    //スコアを取得
    public static float getScore()
    {
        return _score;
    }
    //スコアを取得
    public static float getChain()
    {
        return _Maxchain;
    }//スコアを取得
    public static float getMaxPush()
    {
        return _MaxPush;
    }//スコアを取得
    public static float getKill()
    {
        return _killNum;
    }
    //スコアを取得
    public static float getLife()
    {
        return _LifeNum;
    }
    // Use this for initialization
    void Start ()
    {
        _score = 0;
        _MaxPush =0;
        _Maxchain = 0;
        _killNum = 0;
        _OlderBrotherHamster =  GameObject.Find("Player").GetComponent<OlderBrotherHamster>();

        _LifeNum = _OlderBrotherHamster.m_Life;
    }
	
	// Update is called once per frame
	void Update ()
    {
        _checkScore = getScore();//確認用スコアに現在のスコアを入れる
        _checkMaxpush = getMaxPush();//確認用に最大の高さを入れる
        _checkChain = getChain();//確認用に連続つぶし数入れる
        _checkKill = getKill();//キル数を入れる
        _checkLife = getLife();//Life数を入れる

        _LifeNum = _OlderBrotherHamster.m_Life;//ライフを更新する

        if (_displayScore != null)//画面表示用のTextが存在したらそれに表示させる
        {
            _displayScore.text = _checkScore.ToString();
        }
    }
    public void Pointscore(float score)//スコアを変更させる
    {
        _score += score;
    }
    public void Pointscore(float score ,float chain,float Push)//スコア,現在のチェイン,つぶした数
    {
        _score += score;
        if (_Maxchain <= chain)
        {
            _Maxchain = chain;
        }
        if (_MaxPush <= Push)
        {
            _MaxPush = Push;
        }
        _killNum += Push;
    }
}
