using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {


    [SerializeField, Header("確認用スコア(いじらない)")]
    private float _checkScore;
    public static float _score;

    [SerializeField, Header("画面表示用のオブジェクト")]
    private Text _displayScore;

    //スコアを取得
    public static float getScore()
    {
        return _score;
    }
    // Use this for initialization
    void Start ()
    {
        _score = 0;
        _checkScore = getScore();
    }
	
	// Update is called once per frame
	void Update ()
    {
        _checkScore = getScore();//確認用スコアに現在のスコアを入れる

        //デバック用=======================================================================
        if (Input.GetKey(KeyCode.P))
        {
            Pointscore(10);
        }
        //=================================================================================

        if (_displayScore != null)//画面表示用のTextが存在したらそれに表示させる
        {
            _displayScore.text = "Score : " + _checkScore.ToString();
        }
    }
    public void Pointscore(float score)//スコアを変更させる
    {
        _score += score;
    }
}
