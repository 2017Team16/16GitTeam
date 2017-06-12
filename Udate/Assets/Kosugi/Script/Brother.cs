using UnityEngine;
using System.Collections;     

public class Brother : MonoBehaviour {

    [SerializeField, Header("ポジション用オブジェクト")]
    private Transform BrotherPosition;

    [HideInInspector, Header("床に当たっているか")]
    public bool _isFloor;

    //弟管理クラス
    private BrotherStateManager m_BrotherStateManager;
    
    // Use this for initialization
    void Start()
    {
        m_BrotherStateManager = GetComponent<BrotherStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BrotherPosition != null)
        {
            transform.position = BrotherPosition.position;
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        if(Input.GetButtonDown("XboxL1"))
        {
            m_BrotherStateManager.SetState(BrotherState.THROW);
        }
        /*デバッグ用*/
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    m_BrotherStateManager.SetState(BrotherState.SPECIAL);
        //    GetComponent<AnimationControl>().m_Anim.SetTrigger("fly");
        //    GetComponent<AnimationControl>().m_Anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        //    Time.timeScale = 0;
        //}
    }

    public void Special()
    {
        GetComponent<AnimationControl>().m_Anim.GetComponent<SpriteRenderer>().enabled = true;
        m_BrotherStateManager.SetState(BrotherState.SPECIAL);
        GetComponent<AnimationControl>().m_Anim.SetTrigger("fly");
        GetComponent<AnimationControl>().m_Anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        Time.timeScale = 0;
    }

    public void OnCollisionEnter(Collision collision)
    {

    }
}
