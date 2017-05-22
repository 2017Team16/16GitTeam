using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpecialUi : MonoBehaviour {

    private OlderBrotherHamster _Player;//プレイヤーの　スペシャルがある場所
    private float _Special;
    private Image _ime;
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
    }
}
