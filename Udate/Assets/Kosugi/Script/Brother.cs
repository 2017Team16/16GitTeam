using UnityEngine;
using System.Collections;     

public class Brother : MonoBehaviour {

    [SerializeField, TooltipAttribute("弟用の座標オブジェクト")]
    private Transform BrotherPosition;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_BrotherStateManager.SetState(BrotherState.THROW);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _isFloor = true;
        }
    }
}
