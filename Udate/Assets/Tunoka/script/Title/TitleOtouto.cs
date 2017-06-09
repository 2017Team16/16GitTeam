using UnityEngine;
using System.Collections;

public class TitleOtouto : MonoBehaviour {

    [SerializeField, Header("SE用弟")]
    public AudioSource _audio;
    public AudioClip _clip01;

    [SerializeField, Header("タイトル用弟")]
    private TitleAni _Ani;

    [SerializeField, Header("移動場所")]
    private GameObject[] _MovePoint;

    [SerializeField, Header("独立先")]
    private GameObject _RootObj;
    [SerializeField, Header("かめら")]
    private GameObject _camera;
    public int _PatternNum = 0;



    public float _cTime;
    // Use this for initialization
    void Start () {

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

        iTween.MoveTo(gameObject, iTween.Hash(
            "x", _MovePoint[0].transform.position.x,
            "y", _MovePoint[0].transform.position.y,
            "z", _MovePoint[0].transform.position.z, "time", 2));
        transform.parent = _RootObj.transform;

        _cTime += Time.deltaTime;
        if (_cTime >= 2)
        {
            _audio.PlayOneShot(_clip01);
            iTween.ShakePosition(_camera.gameObject, iTween.Hash("x", 0.3f, "y", 0.3f, "time", 2));
            ChangePattern(3);
            _Ani.ChangePattern(2);

        }
     

   
    }
    void Move02()
    {

    }
    void Move03()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", _MovePoint[1].transform.position.y,
                  "time", 3, "EaseType", iTween.EaseType.easeInCirc));

    }
    public void ChangePattern(int i)
    {
        _PatternNum = i;
    }
}
