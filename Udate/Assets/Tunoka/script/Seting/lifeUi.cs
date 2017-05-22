using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class lifeUi : MonoBehaviour {

    private OlderBrotherHamster _Player;//プレイヤーの　HPがある場所
    private int _Hp;
    private Image _ime;
    // Use this for initialization
    void Start () {

        _Player = GameObject.Find("Player").GetComponent<OlderBrotherHamster>() ;
        _Hp = _Player.m_Life;
        _ime = transform.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        _Hp = _Player.m_Life;
        if ((int.Parse(transform.name) * 2) - _Hp <= 0)
        {
            _ime.fillAmount = 1;
        }
        else if ((int.Parse(transform.name) * 2) - _Hp == 1)
        {
            _ime.fillAmount = 0.5f;
        }
        else
        {
            _ime.fillAmount = 0;
        }


    }
}
