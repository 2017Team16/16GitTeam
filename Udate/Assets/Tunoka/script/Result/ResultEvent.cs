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

    [SerializeField, Header("最終スコア表示用")]
    private GameObject _ScoreObj;
    private Text _displayScore;//表示する用の中身
    [SerializeField, Header("エフェクト")]
    public GameObject _EffectObg;
    [SerializeField, Header("今のイベント状態")]
    private int _EventNum = 0;

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

        //
        //
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

      
    }
    void Event1()//最初のアニメーション
    {
      
    }
    void Event2()//_Chainの加算アニメーション
    {
       
    }
    void Event3()//_MaxPushの加算アニメーション
    {
 
    }

}
