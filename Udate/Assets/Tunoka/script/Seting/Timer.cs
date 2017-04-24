using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {


    [SerializeField, Header("ゲーム終了時間")]
    public float _ENDTime;

    [SerializeField, Header("経過時間")]
    private float _gameTime;

    [SerializeField, Header("画面表示用のオブジェクト")]
    private Text _displayTime;


    // Use this for initialization
    void Start () {
        //初期化
        _gameTime = 0;

    }
	
	// Update is called once per frame
	void Update () {
        if (_gameTime < 0) return;//マイナスの時これ以上時間の処理を行わない

        //経過時間が終了時間を超えたら処理する
        if (_gameTime >= _ENDTime)
        {
            transform.GetComponent<GameMainRule>().GameClear();
            _gameTime = -1;//時間の処理を終わらせる
            return;
        }

        _gameTime += Time.deltaTime;//時間を進める

        if (_displayTime != null)//画面表示用のTextが存在したらそれに表示させる
        {
            _displayTime.text = "Time : " + (_ENDTime - (int)_gameTime).ToString();
        }
    }
}
