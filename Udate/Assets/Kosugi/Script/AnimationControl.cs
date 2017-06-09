using UnityEngine;
using System.Collections;

public class AnimationControl : MonoBehaviour {

    [SerializeField, TooltipAttribute("弟アニメーション用オブジェクト")]
    private GameObject m_BrosAnim;
    private Vector3 pos;
    
    [HideInInspector]
    public Animator m_Anim;

    private Vector3 positiveScale, negativeScale;

    private BrotherStateManager m_BrosManager;

    // Use this for initialization
    void Start () {
        m_Anim = m_BrosAnim.GetComponent<Animator>();
        m_BrosManager = GetComponent<BrotherStateManager>();

        pos = new Vector3(0, 0.45f, 0);

        positiveScale = new Vector3(m_BrosAnim.transform.localScale.x, m_BrosAnim.transform.localScale.y, m_BrosAnim.transform.localScale.z);
        negativeScale = new Vector3(-m_BrosAnim.transform.localScale.x, m_BrosAnim.transform.localScale.y, m_BrosAnim.transform.localScale.z);

        m_Anim.SetBool("death", false);
    }
	
	// Update is called once per frame
	void Update () {
        if (m_Anim.GetCurrentAnimatorStateInfo(0).fullPathHash != Animator.StringToHash("Base Layer.wait"))
            m_BrosAnim.transform.position = transform.position + pos;
        else
            m_BrosAnim.transform.position = transform.position;

        if (m_BrosManager.GetState() != BrotherState.NORMAL)
            m_Anim.GetComponent<SpriteRenderer>().enabled = true;
        else
            m_Anim.GetComponent<SpriteRenderer>().enabled = false;



        if (Vector3.Dot(transform.forward, new Vector3(1, 0, 0)) >= 0)
        {
            m_BrosAnim.transform.localScale = positiveScale;
            m_Anim.SetFloat("Speed", -1);
        }
        else
        {
            m_BrosAnim.transform.localScale = negativeScale;
            m_Anim.SetFloat("Speed", 1);
        }

        if (!GameDatas.isPlayerLive)
            m_Anim.SetTrigger("death");
    }
}
