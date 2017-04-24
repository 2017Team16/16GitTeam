using UnityEngine;
using System.Collections;

public class PlayerSupport : MonoBehaviour {

    private float m_InvincibleInterval = 3.0f;
    private float m_InvincibleTime = 0.0f;
    
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    if(transform.tag == "RecoveryEnemy")
        {
            m_InvincibleTime += Time.deltaTime;
            if(m_InvincibleTime > m_InvincibleInterval)
            {
                Sutan();
            }
        }
	}

    private void Sutan()
    {
        transform.tag = "SutanEnemy";
        transform.rotation = Quaternion.Euler(0, 0, 0);
        m_InvincibleTime = 0.0f;
    }

    public void ChangeGet()
    {
        transform.tag = "GetEnemy";
    }

    public void ChangeRecovery()
    {
        transform.tag = "RecoveryEnemy";
    }
}
