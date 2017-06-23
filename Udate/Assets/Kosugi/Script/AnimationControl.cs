using UnityEngine;
using System.Collections;

public class AnimationControl : MonoBehaviour {

    /*--外部設定オブジェクト--*/
    [SerializeField, Header("弟アニメーションオブジェクト")]
    private GameObject m_BrosAnimation;
    [Header("弟固定ポジション")]
    private Vector3 pos;


    /*------内部設定変数------*/
    [HideInInspector, Header("アニメーター")]
    public Animator m_Anim;

    [Header("反転用")]
    private Vector3 positiveScale, negativeScale;


    [Header("弟管理クラス")]
    private BrotherStateManager m_BrotherStateManager;

    // Use this for initialization
    void Start () {
        m_Anim = m_BrosAnimation.GetComponent<Animator>();
        m_BrosAnimation.GetComponent<SpriteRenderer>().enabled = false;

        m_BrotherStateManager = GetComponent<BrotherStateManager>();

        pos = new Vector3(0, 0.45f, 0);

        positiveScale = new Vector3(m_BrosAnimation.transform.localScale.x, m_BrosAnimation.transform.localScale.y, m_BrosAnimation.transform.localScale.z);
        negativeScale = new Vector3(-m_BrosAnimation.transform.localScale.x, m_BrosAnimation.transform.localScale.y, m_BrosAnimation.transform.localScale.z);

        m_Anim.SetBool("death", false);
    }
	
	// Update is called once per frame
	void Update () {
        if (GameDatas.isPlayerLive)
        {
            if (m_Anim.GetCurrentAnimatorStateInfo(0).fullPathHash != Animator.StringToHash("Base Layer.wait"))
                m_BrosAnimation.transform.position = transform.position + pos;
            else
                m_BrosAnimation.transform.position = transform.position;
        }
        else
        {
            m_BrosAnimation.transform.position = new Vector3(transform.position.x + 1.0f, 1.5f, transform.position.z + 1.0f);
            m_Anim.SetTrigger("death");
            m_Anim.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (Vector3.Dot(transform.forward, new Vector3(1, 0, 0)) >= 0)
        {
            m_BrosAnimation.transform.localScale = positiveScale;
            m_Anim.SetFloat("Speed", -1);
        }
        else
        {
            m_BrosAnimation.transform.localScale = negativeScale;
            m_Anim.SetFloat("Speed", 1);
        }
    }
}
