using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpecialUi : MonoBehaviour {

    private OlderBrotherHamster _Player;//プレイヤーの　スペシャルがある場所
    private float _Special;
    private Image _ime;

    [SerializeField, Header("画像リスト")]
    private Sprite[] _ImeS;
    public int _ListNum = 0;
    // Use this for initialization
    void Start () {

        _Player = GameObject.Find("Player").GetComponent<OlderBrotherHamster>();
        _Special = _Player.GetSpacialPoint();
        _ime = transform.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        _Special = _Player.GetSpacialPoint();
        _ime.fillAmount = _Special;

        if (_Special == 1 || GameDatas.isSpecialAttack == true)//サイズ調整が必要な画像
        {
            _ListNum = 1;
        }
        else
        {
            _ListNum = 0;
        }
        transform.GetComponent<Image>().sprite = _ImeS[_ListNum];


       
    }
}
