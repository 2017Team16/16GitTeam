using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

    public AudioSource _audio;
    public AudioClip _clip01;
    public AudioClip _clip02;

    private TitleAnimController _TAnim;
    private SceneChanger SChang;


    [SerializeField, Header("Cursor番号")]
    public int _CursorNum = 0;


    private bool Neutral = true;

    // Use this for initialization
    void Start () {
        _TAnim = transform.GetComponent<TitleAnimController>();
        SChang = transform.GetComponent<SceneChanger>();
       _CursorNum = 1;
}

    // Update is called once per frame
    void Update()
    {
        if (_TAnim.GetAnimeFlag() == false) return;

        if (_CursorNum >= 0)
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
        _CursorNum += i;
        if (_CursorNum < 1)
        {
            _CursorNum = 3;
        }
        else if (_CursorNum > 3)
        {
            _CursorNum = 1;
        }
    }
    public void SceneChangeButton()
    {
        switch (_CursorNum)
        {
            case 1: SChang.FadeOut(); break;
            case 2: SChang.FadeOut("MainTutorial"); break;
            case 3: print("ゲーム終了"); Application.Quit(); break;
        }
        _CursorNum = -1;//カーソル操作を終了させる
        
    }
    public int GetCursorNum()
    {
        return (int)_CursorNum;
    }

    
}
