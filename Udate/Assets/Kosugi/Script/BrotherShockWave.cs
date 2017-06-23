using UnityEngine;
using System.Collections;

public class BrotherShockWave : MonoBehaviour {

    /*------内部設定変数------*/
    [Header("弟管理クラス")]
    private BrotherStateManager m_BrotherStateManager;

    // Use this for initialization
    void Start () {
        m_BrotherStateManager = GameObject.Find("Brother").GetComponent<BrotherStateManager>();
    }
	
	// Update is called once per frame
	void Update () {

	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" &&
            (m_BrotherStateManager.GetState() == BrotherState.THROW || m_BrotherStateManager.GetState() == BrotherState.BACK))
        {
            other.gameObject.SendMessage("ChangeState", 3, SendMessageOptions.DontRequireReceiver);
        }
    }
}
