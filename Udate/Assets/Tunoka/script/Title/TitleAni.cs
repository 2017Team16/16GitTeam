using UnityEngine;
using System.Collections;

public class TitleAni : MonoBehaviour {

    [SerializeField, Header("SE用弟")]
    public AudioSource _audio;
    public AudioClip _clip01;

    [SerializeField, Header("Title用弟")]
    private TitleOtouto _otuto;

    [SerializeField, Header("移動場所")]
    private GameObject[] _MovePoint;
    [SerializeField, Header("アニメーションの親")]
    private TitleAnimController _TitleAnimController;
    public int _PatternNum = 1;
    public float _cTime;
    //アニメ用変数たち
    private Animator m_Animator;
    private AnimatorStateInfo stateInfo;


    // Use this for initialization
    void Start () {

        _cTime = 0;
        m_Animator = transform.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
       
        switch (_PatternNum)
        {
            case 0:
                stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowBrother")&&
                    stateInfo.fullPathHash != Animator.StringToHash("Base Layer.PlayerWaitSolo"))
                {
                    m_Animator.Play("PlayerWaitSolo");
                }
                break;
            case 1: Move01(); break;
            case 2: Move02(); break;
            case 3: Move03(); break;
        }
    }
    void Move01()
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", _MovePoint[0].transform.position.x, "time", 10));
        _cTime += Time.deltaTime;
        if (_cTime >= 4 )
        {
            m_Animator.Play("PlayerThrowStart");
            _audio.PlayOneShot(_clip01);
            _cTime = 0;
            ChangePattern(0);
            _otuto.ChangePattern(1);
        }
        
    }
    void Move02()
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", _MovePoint[1].transform.position.x, "time", 20));
        _cTime += Time.deltaTime;
        if (_cTime >= 1)
        {
            _cTime = 0;
            ChangePattern(3);
            _TitleAnimController.NextAnimationstep();
        }
    }
    void Move03()
    {
        _cTime += Time.deltaTime;
        if (_cTime >= 6)
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
