using UnityEngine;
using System.Collections;

public class StageSelectController : MonoBehaviour {

    public AudioSource _audio;
    public AudioClip _clip01;
    public AudioClip _clip02;

    private SceneChanger SChang;
    [SerializeField, Header("Cursor番号")]
    public int _CursorNum = 0;


    private bool Neutral = true;


    public static int StageNum = 0;

    public static int getStageNum()
    {
        return StageNum;
    }


    void Start () {
        SChang = transform.GetComponent<SceneChanger>();
        _CursorNum = 1;
    }
	
	// Update is called once per frame
	void Update () {
        
      if ( _CursorNum >= 0)
        {
            if (Input.GetButtonDown("XboxB"))
            {
                SChang.FadeOut("MainPlay0" + (_CursorNum -1).ToString());

                _audio.PlayOneShot(_clip02);
                _CursorNum = -1;
            }
            

            if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Horizontal") > 0)
            {
                if (Neutral == true)
                {
                    _audio.PlayOneShot(_clip01);
                    
                    CursorAdjustment(+1);
                    Neutral = false;
                }
                return;
            }
            if (Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") < 0)
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
        StageNum = _CursorNum;
    }
    public int GetCursorNum()
    {
        return _CursorNum;
    }
}
