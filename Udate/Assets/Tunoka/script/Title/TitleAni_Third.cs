using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    //アニメ用変数たち
    private Animator m_Animator;
    private AnimatorStateInfo stateInfo;
    // Use this for initialization
    void Start ()
    {
        _cTime = 0;

        m_Animator = transform.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        Flashing();
        switch (_PatternNum)
        {
            case 0:
                
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
        if (_cTime >= 4)
        {
            m_Animator.Play("PlayerThrowStart");
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
        stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowBrother") &&
            stateInfo.fullPathHash != Animator.StringToHash("Base Layer.PlayerWaitSolo"))
        {
            m_Animator.Play("PlayerWaitSolo");
        }
        _cTime += Time.deltaTime;
        if (_cTime >= 4)
        {
            ChangePattern(3);
        }
    }
    void Move03()
    {
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        iTween.MoveTo(gameObject, iTween.Hash("x", _MovePoint[2].transform.position.x, "time", 20));
        
        ChangePattern(0);
      
    }
    public void ChangePattern(int i)
    {
        _PatternNum = i;
    }
    bool alphaZero = true;
    void Flashing()
    {
        //アルファが0.5以下になったら透明　0.8以上になったら見えてる
        if (_NextText.GetComponent<Image>().color.a <= 0.5f)
        {
            alphaZero = true;
        }
        else if (_NextText.GetComponent<Image>().color.a >= 1f)
        {
            alphaZero = false;
        }

        if (alphaZero == false)
        {
            _NextText.GetComponent<Image>().color -= new Color(0,0,0,0.02f);
        }
        else
        {
            _NextText.GetComponent<Image>().color += new Color(0, 0, 0, 0.02f);
        }
    }
}
