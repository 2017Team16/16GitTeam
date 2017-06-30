using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ResultController : MonoBehaviour {


    [SerializeField, Header("確認用スコア(いじらない)")]
    private float _Score;
    [SerializeField, Header("確認用連続チェイン(いじらない)")]
    private float _Chain;
    [SerializeField, Header("確認用一回の最大数(いじらない)")]
    private float _Maxpush;
    [SerializeField, Header("確認用倒した数(いじらない)")]
    private float _Kill;
    [SerializeField, Header("確認用倒した数(いじらない)")]
    private float LastScore;

    [SerializeField, Header("画面表示用のオブジェクト")]
    private Text _displayLastScore;




    
    private RankingSeting _rankingSeting;
    // Use this for initialization
    void Start () {
        _Score = Score.getScore();
        _Chain = Score.getChain();
        _Maxpush = Score.getMaxPush();
        _Kill = Score.getKill();


        LastScore = _Score + _Maxpush * 100 + _Chain * 50  ;

        _rankingSeting = GameObject.Find("Ranking").GetComponent<RankingSeting>();

        //_rankingSeting.RankListIn(LastScore);
        if (_displayLastScore != null)//画面表示用のTextが存在したらそれに表示させる
        {
            _displayLastScore.text = LastScore.ToString();
        }
    }
	
	// Update is called once per frame
	void Update () {

	}
}
