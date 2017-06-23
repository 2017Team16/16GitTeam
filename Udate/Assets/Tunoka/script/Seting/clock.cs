using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class clock : MonoBehaviour {

    private float _Timer;
    [SerializeField, Header("時計のゲージ")]
    private Image _clock;
    [SerializeField, Header("時計の針")]
    private GameObject _clockPointr;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SetTimer(float Time)
    {
        _clock.fillAmount = Time;
        print(_clockPointr.transform.rotation);
        _clockPointr.transform.rotation = Quaternion.Euler(0, 0, 360 * (Time));
    }
}
