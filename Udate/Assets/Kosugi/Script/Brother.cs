using UnityEngine;
using System.Collections;     

public class Brother : MonoBehaviour {

    /*--外部設定オブジェクト--*/
    [SerializeField, Header("ポジション用オブジェクト")]
    private Transform BrotherPosition;

    [Header("弟管理クラス")]
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

        if (Input.GetButtonDown("XboxL1") && Time.timeScale != 0)
        {
            m_BrotherStateManager.SetState(BrotherState.THROW);
        }
    }

    public void Special()
    {
        GetComponent<AnimationControl>().m_Anim.GetComponent<SpriteRenderer>().enabled = true;
        m_BrotherStateManager.SetState(BrotherState.SPECIAL);
        GetComponent<AnimationControl>().m_Anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        Time.timeScale = 0;
    }

    public void OnCollisionEnter(Collision collision)
    {

    }
}
