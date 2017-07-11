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

        if (GameDatas.isSpecialAttack == true)//必殺時は時を止める
        {
            return;
        }

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
         }

        if (_ENDTime - _gameTime <= 3)//終了三秒前処理
        {
            if (_Check != (int)(_ENDTime - _gameTime))
            {
                _StartEND_Production.End_Production(_Check);
                _Check = (int)(_ENDTime - _gameTime);
            }
            else if (_Check == 3)
            {
                _StartEND_Production.End_Production(_Check);
            }
        }
        else//三秒前から解消されたら
        {
            _StartEND_Production.End_ProductionOff();
        }
    }
    public void HeelTime(int num)//時間回復
    {
        for (int i = 0; i <= num; i++)
        {
            if (_gameTime <= 1)
            {
                return;
            }
            _gameTime--;
        }

    }
}
