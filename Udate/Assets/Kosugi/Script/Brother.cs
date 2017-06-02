using UnityEngine;
using System.Collections;     

public class Brother : MonoBehaviour {

    [SerializeField, TooltipAttribute("ポジション用オブジェクト")]
    private Transform BrotherPosition;

    [HideInInspector, TooltipAttribute("床に当たっているか")]
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            m_BrotherStateManager.SetState(BrotherState.SPECIAL);
            Time.timeScale = 0;
        }
    }

    public void Special()
    {
        m_BrotherStateManager.SetState(BrotherState.SPECIAL);
        Time.timeScale = 0;
    }

    public void OnCollisionEnter(Collision collision)
    {

    }
}
