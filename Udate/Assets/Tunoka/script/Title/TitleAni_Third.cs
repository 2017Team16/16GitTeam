using UnityEngine;
using System.Collections;

public class TitleAni_Third : MonoBehaviour {

    [SerializeField, Header("SE用弟")]
    public AudioSource _audio;
    public AudioClip _clip01;
    [SerializeField, Header("移動場所")]
    private GameObject[] _MovePoint;
    [SerializeField, Header("運ぶText")]
    private GameObject _NextText;
    [SerializeField, Header("独立先")]
    private GameObject _RootObj;
    public int _PatternNum = 1;
    public float _cTime;
    // Use this for initialization
    void Start ()
    {
        _cTime = 0;

    }
	
	// Update is called once per frame
	void Update () {
        switch (_PatternNum)
        {
            case 1: Move01(); break;
            case 2: Move02(); break;
            case 3: Move03(); break;
        }
    }
    void Move01()
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", _MovePoint[0].transform.position.x, "time", 10));
        _cTime += Time.deltaTime;
        if (_cTime >= 4)
        {
            _audio.PlayOneShot(_clip01);
            _cTime = 0;
            ChangePattern(2);
            _NextText.transform.parent = _RootObj.transform;
            iTween.MoveTo(_NextText, iTween.Hash(
            "x", _MovePoint[1].transform.position.x,
            "y", _MovePoint[1].transform.position.y,
            "z", _MovePoint[1].transform.position.z,
                "time", 5));

        }
    }
    void Move02()
    {
        _cTime += Time.deltaTime;
        if (_cTime >= 4)
        {
            ChangePattern(3);
        }
    }
    void Move03()
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", _MovePoint[2].transform.position.x, "time", 20));
        
        ChangePattern(0);
      
    }
    public void ChangePattern(int i)
    {
        _PatternNum = i;
    }
}
