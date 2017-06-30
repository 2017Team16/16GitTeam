using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {


    [SerializeField, Header("ゲーム終了時間")]
    public float _ENDTime;

    [SerializeField, Header("経過時間")]
    private float _gameTime;

    [SerializeField, Header("画面表示用のオブジェクト")]
    private clock _displayTime;

    [SerializeField, Header("ゲーム終了演出")]
    private StartEND_Production _StartEND_Production;

    private int _Check;

    // Use this for initialization
    void Start () {
        //初期化
        _gameTime = 0;
        _Check = 3;
    }
	
	// Update is called once per frame
	void Update () {


        if (_gameTime < 0)
        {
            _StartEND_Production.End_Production(0);
            return;//マイナスの時これ以上時間の処理を行わない
        }
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

            _displayTime.SetTimer(1 -(_gameTime / _ENDTime));
            //_displayTime.text = "Time : " + (_ENDTime - (int)_gameTime).ToString();
        }

        if (_ENDTime - _gameTime <= 3)
        {
            if (_Check != (int)(_ENDTime - _gameTime ))
            {
                _StartEND_Production.End_Production(_Check);
                _Check = (int)(_ENDTime - _gameTime );
            }
            else if (_Check == 3)
            {
                _StartEND_Production.End_Production(_Check);
            }
        }
    }
    public void HeelTime(int num)
    {
        for (int i = 0; i <= num; i++)
        {
            if (_gameTime <= 1)
            {
                _gameTime = 0;
                return;
            }
            _gameTime--;
        }
    }
}
