using UnityEngine;
using System.Collections;

public class TitleAnimController : MonoBehaviour {
    [SerializeField, Header("Title時のアニメーション　終わったらtrue")]
    private bool _TitleAnimeEnd = false;

    [SerializeField, Header("アニメーション終了時動くもの")]
    private GameObject _TitleAnimOffObj;

    [SerializeField, Header("現在進行中のアニメーション　マイナスなら終了")]
    private int _Animationstep = 0;
    // Use this for initialization
    void Start () {

        _TitleAnimeEnd = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (_TitleAnimeEnd == false)
        {
            TitleAnimOn();
        }
    }
    void TitleAnimOn()
    {
        //デバック用=======================================================================
        if (Input.anyKeyDown)
        {
            _Animationstep = -1;//アニメーションを終了させる
            TitleAnimOff();//アニメーションが終了した際に動くものを動かす
            _TitleAnimeEnd = true;
        }
        //=================================================================================
    }
    void TitleAnimOff()
    {
        _TitleAnimOffObj.gameObject.SetActive(true);
    }


    public  bool GetAnimeFlag()
    {
        return _TitleAnimeEnd;
    }
    public int GetAnimationStep()
    {
        return _Animationstep;
    }
    public void NextAnimationstep()
    {
        _Animationstep++;
    }
}
