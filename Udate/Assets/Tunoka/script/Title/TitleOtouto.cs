using UnityEngine;
using System.Collections;

public class TitleOtouto : MonoBehaviour {


    [SerializeField, Header("タイトル用弟")]
    private TitleAni _Ani;

    [SerializeField, Header("移動場所")]
    private GameObject[] _MovePoint;
    [SerializeField, Header("カメラ")]
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
            "z", _MovePoint[0].transform.position.z, "time", 4));

   
        _cTime += Time.deltaTime;
        if (_cTime >= 3)
        {

        }

        if (_cTime >= 6)
        {
            _cTime = 0;
            ChangePattern(2);
            _Ani.ChangePattern(3);
        }
    }
    void Move02()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", _MovePoint[1].transform.position.y,
               "time", 6, "EaseType", iTween.EaseType.easeInCirc));
    }
    void Move03()
    {
    }
    public void ChangePattern(int i)
    {
        _PatternNum = i;
    }
}
