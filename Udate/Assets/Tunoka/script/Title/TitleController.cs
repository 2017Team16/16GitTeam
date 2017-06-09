using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

    public AudioSource _audio;
    public AudioClip _clip01;
    public AudioClip _clip02;

    private TitleAnimController _TAnim;
    private SceneChanger SChang;

    [SerializeField, Header("ルール説明テキスト")]
    private GameObject _RuleIme;


    [SerializeField, Header("Cursor番号")]
    public float _CursorNum = 0;
    // Use this for initialization
    void Start () {
        _TAnim = transform.GetComponent<TitleAnimController>();
        SChang = transform.GetComponent<SceneChanger>();
       _CursorNum = 0;
}

    // Update is called once per frame
    void Update()
    {

        if (_CursorNum == 3)//説明画面表示中
        {
            if (Input.GetButtonDown("XboxB"))
            {
                _audio.PlayOneShot(_clip02);
                _CursorNum = 0;
            }
        }
        else if (_TAnim.GetAnimeFlag() && _CursorNum >= 0)
        {
            _RuleIme.SetActive(false);
            if (_CursorNum == 0 && Input.GetButtonDown("XboxB"))
            {
                _audio.PlayOneShot(_clip02);
                SceneChangeButton();
            }
            if (_CursorNum == 1 && Input.GetButtonDown("XboxB"))
            {
                RuleButton();
                _audio.PlayOneShot(_clip02);
            }
            if (Input.GetAxis("Vertical") > 0)
            {
                if(_CursorNum != 0)
                {
                    _audio.PlayOneShot(_clip01);
                }
                _CursorNum = 0;
            }
            if (Input.GetAxis("Vertical") < 0)
            {
                if (_CursorNum != 1)
                {
                    _audio.PlayOneShot(_clip01);
                }
                _CursorNum = 1;
            }
        }

    }
    public void SceneChangeButton()
    {
        _CursorNum = -1;//カーソル操作を終了させる
        print("シーンチェンジだよ");
        SChang.FadeOut();

    }
    public void RuleButton()
    {
        print("説明");
        _CursorNum = 3;
        _RuleIme.SetActive(true);
    }

    
}
