using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResultEvent : MonoBehaviour
{
    [SerializeField, Header("SE用弟")]
    public AudioSource _audio;
    public AudioClip _clip01;
    public AudioClip _clip02;
    public AudioClip _clip03;


    private float _Score;
    private float _Chain;
    private float _Maxpush;
    [SerializeField, Header("画面表示用のオブジェクト")]
    private GameObject _ScoreObj;
    private Text _displayScore;
    public GameObject _EffectObg;
    [SerializeField, Header("今のイベント状態")]
    private int _EventNum = 0;
    [SerializeField, Header("加算オブジェ")]
    private GameObject _AdditionObj;
    private Text _AdditionText;
    private float _Timer;


    [SerializeField, Header("選択中のオブジェクト")]
    private GameObject _SelectWaku;

    public Vector3 test;
    // Use this for initialization
    void Start ()
    {
        _Score = Score.getScore() ;
        _Chain = Score.getChain();
        _Maxpush = Score.getMaxPush();
        _EventNum = 0;
        _Timer =0 ;
        _displayScore = _ScoreObj.GetComponent<Text>();
        _AdditionText = _AdditionObj.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        _Timer += Time.deltaTime;
        _displayScore.text = _Score.ToString();
        switch (_EventNum)//Event状態に合わせて動かすものを変える
        {
            case 0: Event0(); break;
            case 1: Event1(); break;
            case 2: Event2(); break;
            case 3: Event3(); break;
        }
    }
    void Event0()//最初のアニメーション
    {

        if (_Timer >= 2)
        {
            _audio.PlayOneShot(_clip01);
            _Timer = 0;
            _EventNum += 1;
            _Score += _Chain * 50;
            print(_Score);
            _ScoreObj.GetComponent<Animator>().Play("ResultScoreAnimation", 0, 0);
            GameObject wreckClone = (GameObject)Instantiate
              (_EffectObg,
               new Vector3(_ScoreObj.transform.position.x, _ScoreObj.transform.position.y, _ScoreObj.transform.position.z - 10),
               Quaternion.identity);
        }
    }
    void Event1()//最初のアニメーション
    {
        _AdditionText.text = "+連続コンボ　：　"+ _Chain;
        if (_Timer >= 2)
        {
            _audio.PlayOneShot(_clip02);
            _Timer = 0;
            _EventNum += 1;
            _Score += _Maxpush * 100;
            print(_Score);
            _ScoreObj.GetComponent<Animator>().Play("ResultScoreAnimation",0,0);
            //Effectを出す
            GameObject wreckClone = (GameObject)Instantiate
               (_EffectObg,
               new Vector3(_ScoreObj.transform.position.x, _ScoreObj.transform.position.y, _ScoreObj.transform.position.z - 10),
                Quaternion.identity);
        }
    }
    void Event2()//_Chainの加算アニメーション
    {

        _AdditionText.text = "+一回で潰した最大数　：　" + _Maxpush;
        if (_Timer >= 2)
        {
            _audio.PlayOneShot(_clip03);
            _Timer = 0;
            _EventNum += 1;
        }
    }
    void Event3()//_MaxPushの加算アニメーション
    {
        iTween.MoveTo(_ScoreObj, iTween.Hash("position" , test, "time", 3));
        iTween.ScaleTo(_ScoreObj, iTween.Hash("x", 1, "y", 1, "time", 3));
        _SelectWaku.SetActive(true);
        gameObject.SetActive(false);
    }

}
