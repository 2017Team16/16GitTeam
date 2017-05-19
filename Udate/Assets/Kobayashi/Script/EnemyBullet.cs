using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{

    public float bulletTime = 0;
    private float m_Time = 0;

    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        m_Time += Time.deltaTime;
        if (m_Time >= bulletTime)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
