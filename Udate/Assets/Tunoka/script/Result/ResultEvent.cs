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


    private float _Timer;

    private float _Score;
    private float _Chain;
    private float _Maxpush;
    private float _life;
    [SerializeField, Header("最終スコア表示用")]
    private Text _displayScore;
    [SerializeField, Header("最終サブスコア表示用")]
    private Text[] _SabScoreObj;
    [SerializeField, Header("新記録表示用")]
    private GameObject _NewRecordObj;
    [SerializeField, Header("壁紙用")]
    private GameObject _GameClearback00;
    [SerializeField, Header("ランキングデータ読み取り")]
    private RankingSeting _RankingSeting;

    [SerializeField, Header("リザルトOrタイトル")]
    private int _NextSelctNum;

    [SerializeField, Header("エフェクト")]
    public GameObject _EffectObg;
    [SerializeField, Header("今のイベント状態[確認用]")]
    private int _EventNum = 0;

    [SerializeField, Header("選択番号")]
    public int _ListNum;
    private bool Neutral = true;


    [SerializeField, Header("scene管理")]
    private SceneChanger SChang;

    [SerializeField, Header("選ばれたStage ")]
    private int StageNum;
    // Use this for initialization
    void Start ()
    {
        StageNum = StageSelectController.getStageNum();
        print(StageNum);
        if (StageNum == 0) StageNum = 1;

        _Score = Score.getScore() ;
        _Chain = Score.getChain();
        _Maxpush = Score.getMaxPush();
        _life = Score.getLife(); ;
        _RankingSeting = GameObject.Find("Ranking").GetComponent<RankingSeting>();

        _EventNum = 0;
        _Timer =0 ;
        _ListNum = 0;

        _displayScore.text = _Score.ToString();

    }
	
	// Update is called once per frame
	void Update () {
        _Timer += Time.deltaTime;
        switch (_EventNum)//Event状態に合わせて動かすものを変える
        {
            case 0: Event0(); break;
            case 1: Event1(); break;
            case 2: Event2(); break;
            case 3: Event3(); break;
            case 4: Event4(); break;
        }
    }
    void Event0()//最初のアニメーション _Chainの加算アニメーション
    {
        _SabScoreObj[0].text = (Random.Range(0, 80)).ToString();
        _SabScoreObj[1].text = (Random.Range(0, 80)).ToString();
        _SabScoreObj[2].text = (Random.Range(0, 80)).ToString();
        if (_Timer >= 2)
        {
            _SabScoreObj[0].text = _Maxpush.ToString();
            _audio.PlayOneShot(_clip01);
            _displayScore.text = (_Score + _Chain * 50 ).ToString();
            _EventNum++;
            _Timer = 0;
            return;
        }
    }

    void Event1()//_MaxPushの加算アニメーション
    {
        _SabScoreObj[1].text = (Random.Range(0, 80)).ToString();
        _SabScoreObj[2].text = (Random.Range(0, 80)).ToString();
        if (_Timer >= 1)
        {
            _SabScoreObj[1].text = _Chain.ToString();
            _audio.PlayOneShot(_clip01);
            _displayScore.text = (_Score + _Chain * 50 + _Maxpush * 100).ToString();
            _EventNum++;
            _Timer = 0;
            return;
        }
    }
    void Event2()//_lifeの加算アニメーション
    {
        _SabScoreObj[2].text = (Random.Range(0, 80)).ToString();
        if (_Timer >= 1)
        {
            _SabScoreObj[2].text = _life.ToString();//残りlifeを入れる
            _audio.PlayOneShot(_clip01);
            _displayScore.text = (_Score + _Chain * 50 + _Maxpush * 100 + _life * 50).ToString();
            _EventNum++;
            _Timer = 0;
            return;
        }

    }
    void Event3()//新記録かどうか
    {
        if (_RankingSeting.getRank(1) <= int.Parse(_displayScore.text))//ランキング1位より高かったら新記録アニメーションをおこなう
        {
            if (_NewRecordObj.activeSelf == false)
            {
                _audio.PlayOneShot(_clip02);
            }
            _NewRecordObj.SetActive(true);
            iTween.ScaleTo(_NewRecordObj.transform.FindChild("seat Icon").gameObject, iTween.Hash("x", 6, "y", 3,  "time", 2));
          
            if (_Timer >= 3)
            {
                _RankingSeting.RankListIn(int.Parse(_displayScore.text));
                _EventNum++;
                _NewRecordObj.SetActive(false);
                _GameClearback00.transform.parent = transform;

                iTween.RotateTo(GameObject.Find("RankingCenter").gameObject, iTween.Hash("z", 0, "time",5, "EaseType", iTween.EaseType.easeOutElastic));
                _Timer = 0;
            }
        }
        else
        {
            _GameClearback00.transform.parent = transform;
            iTween.RotateTo(GameObject.Find("RankingCenter").gameObject, iTween.Hash("z", 0, "time", 5, "EaseType", iTween.EaseType.easeOutElastic));
            _RankingSeting.RankListIn(int.Parse(_displayScore.text));
            _Timer = 0;
            _EventNum++;
        }
    }
    void Event4()//リザルト描画
    {
        if (_ListNum >= 0)
        {
            if (Input.GetButtonDown("XboxB"))
            {
                SceneChangeButton();
                _audio.PlayOneShot(_clip02);
            }


            if (Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") > 0)
            {
                if (Neutral == true)
                {
                    _audio.PlayOneShot(_clip01);

                    CursorAdjustment(+1);
                    Neutral = false;
                }
                return;
            }
            if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Horizontal") < 0)
            {
                if (Neutral == true)
                {
                    _audio.PlayOneShot(_clip01);
                    CursorAdjustment(-1);
                    Neutral = false;
                }
                return;
            }

            Neutral = true;
        }
    }
    void CursorAdjustment(int i)//カーソルの調整
    {
        _ListNum += i;
        if (_ListNum < 0)
        {
            _ListNum = 1;
        }
        else if (_ListNum > 1)
        {
            _ListNum = 0;
        }
    }
    public void SceneChangeButton()
    {
        switch (_ListNum)
        {
            case 0: SChang.FadeOut("MainTitle");  break;
            case 1: SChang.FadeOut("MainPlay0" + (StageNum - 1).ToString()); break;
        }
        _ListNum = 100;//カーソル操作を終了させる
    }
}
