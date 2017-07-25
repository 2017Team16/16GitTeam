using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

    [SerializeField]
    private bool _pauseTr;
    [SerializeField]
    private GameObject _MoveObj;

    [SerializeField, Header("scene管理")]
    private SceneChanger SChang;
    public int _ListNum;
    private bool Neutral = true;

    [SerializeField, Header("操作説明")]
    private GameObject SetumeiObj;
    private bool SetumeiTr;

    private bool _TimeScaleTr;//必殺時の状態

    // Use this for initialization
    void Start ()
    {
        _TimeScaleTr = false;
        SetumeiTr = false;
        _pauseTr = false;
        Neutral = false;
        _MoveObj.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //print(GameDatas.isBrotherSpecialMove);
        if (Input.GetButtonDown("XboxStart"))//ポーズボタンを押したかどうか
        {
            _pauseTr = true;
        }

        if (SetumeiTr == true)
        {
            if (Input.GetButtonDown("XboxB"))
            {
                SetumeiObj.SetActive(false);
                SetumeiTr = false;
            }
            return;
        }
        if (GameDatas.isSpecialAttack == true)//必殺技中だったら
        {
            SpecialPause();
            return;
        }
        if (_pauseTr == true)
        {
            PausCon();
            _MoveObj.SetActive(true);
            
            Time.timeScale = 0;
        }
        else
        {
            _ListNum = 0;
            _MoveObj.SetActive(false);
            Time.timeScale = 1;
        }

        _TimeScaleTr = false;//必殺時のTrを直す
    }
    void SpecialPause()
    {
        if (_pauseTr == true)
        {
            if (Time.timeScale != 0)
            {
                _TimeScaleTr = true;
                Time.timeScale = 0;
            }
            _MoveObj.SetActive(true);
            //弟を止める
            GameDatas.isBrotherSpecialMove = false;
            PausCon();
            //
        }
        else
        {
            if (_TimeScaleTr == true)
            {
                Time.timeScale = 1;
            }
            _ListNum = 0;
            _MoveObj.SetActive(false);
            //弟を動かす
            //
        }
    }
    void PausCon()//ポーズ中の動き
    {
        if (_ListNum >= 0)
        {
            if (Input.GetButtonDown("XboxB"))
            {
                SceneChangeButton();
            }
            if (Input.GetAxisRaw("Vertical") < 0 || Input.GetAxisRaw("Horizontal") > 0)
            {
                if (Neutral == true)
                {
                    CursorAdjustment(+1);
                    Neutral = false;
                }
                return;
            }
            if (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Horizontal") < 0)
            {
                if (Neutral == true)
                {
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
            _ListNum = 2;
        }
        else if (_ListNum > 2)
        {
            _ListNum = 0;
        }
    }
    public void SceneChangeButton()
    {
        switch (_ListNum)
        {
            case 0:
                _pauseTr = false;
                GameDatas.isBrotherSpecialMove = true; break;
            case 1:
                SetumeiTr = true;
                SetumeiObj.SetActive(true); break;
            case 2:
                _ListNum = 100;//カーソル操作を終了させる
                Time.timeScale = 1;
                SChang.PauseFadeOut("MainTitle");
                break;
        }
    }

    //ポース判定の受け渡し
    public bool isPause()
    {
        return _pauseTr;
    }
}
