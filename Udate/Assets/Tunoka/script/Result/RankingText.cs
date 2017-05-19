using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RankingText : MonoBehaviour {

    [SerializeField, Header("画面表示用のオブジェクト")]
    private Text _displayRankingScore;
    [SerializeField, Header("表示のランク番号")]
    private int  _num;


    public RankingSeting _rankingSeting;
    // Use this for initialization
    void Start ()
    {
        _rankingSeting = GameObject.Find("Ranking").GetComponent<RankingSeting>();
    }
	
	// Update is called once per frame
	void Update () {
        if (_displayRankingScore != null)//画面表示用のTextが存在したらそれに表示させる
        {
            _displayRankingScore.text = _rankingSeting.getRank(_num).ToString();
        }
    }
}
