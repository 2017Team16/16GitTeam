using UnityEngine;
using System.Collections;

public class SPstar : MonoBehaviour {

    /*------内部設定------*/
    [Header("弟オブジェクト")]
    private GameObject m_Brother;

    // Use this for initialization
    void Awake () {
        m_Brother = GameObject.Find("Brother");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void AnimEnd()
    {
        transform.parent.gameObject.SetActive(false);
        m_Brother.SendMessage("EnemySet", SendMessageOptions.DontRequireReceiver);
    }
}
