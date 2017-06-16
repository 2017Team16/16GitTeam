using UnityEngine;
using System.Collections;

public class SPstar : MonoBehaviour {

    [SerializeField, Header("弟オブジェクト")]
    private GameObject m_Brother;

    // Use this for initialization
    void Start () {
	
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
