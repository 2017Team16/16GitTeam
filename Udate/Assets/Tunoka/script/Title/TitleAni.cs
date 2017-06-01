using UnityEngine;
using System.Collections;

public class TitleAni : MonoBehaviour {

    [SerializeField, Header("Title用弟")]
    private TitleOtouto _otuto;

    [SerializeField, Header("移動場所")]
    private GameObject[] _MovePoint;
    [SerializeField, Header("アニメーションの親")]
    private TitleAnimController _TitleAnimController;
    public int _PatternNum = 1;
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
        iTween.MoveTo(gameObject, iTween.Hash("x", _MovePoint[0].transform.position.x, "time", 10));
        _cTime += Time.deltaTime;
        if (_cTime >= 6 )
        {
            _cTime = 0;
            ChangePattern(2);
            _otuto.ChangePattern(1);
        }
    }
    void Move02()
    {

    }
    void Move03()
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", _MovePoint[1].transform.position.x, "time", 20));
        _cTime += Time.deltaTime;
        if (_cTime >= 1)
        {
            _cTime = 0;
            ChangePattern(0);
            _TitleAnimController.NextAnimationstep();
        }
    }

    public void ChangePattern(int i)
    {
        _PatternNum = i ;
    }

}
