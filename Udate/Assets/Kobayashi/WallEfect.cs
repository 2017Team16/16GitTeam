using UnityEngine;
using System.Collections;

public class WallEfect : MonoBehaviour
{
    private GameObject m_brother;
    public GameObject m_Effect;
    public float effectTime = 0.0f;

    // Use this for initialization
    void Start()
    {
        m_brother = GameObject.Find("Brother");
    }

    // Update is called once per frame
    void Update()
    {

    }
    //弟が壁とぶつかったら
    public void OnCollisionEnter(Collision collision)
    {
        //壁に当たったエフェクト
        if (collision.gameObject == m_brother)
        {
            foreach(ContactPoint point in collision.contacts)
            {
                GameObject effect = (GameObject)Instantiate(m_Effect);
                effect.transform.position = point.point;
                Vector3 p = m_brother.transform.position - point.point;
                Quaternion rotation = Quaternion.LookRotation(p);
                effect.transform.rotation = rotation;
                Destroy(effect, effectTime);
            }
            //WallEffect();
        }
    }
    public void WallEffect()
    {
        GameObject effect = (GameObject)Instantiate(m_Effect, m_brother.transform.position, Quaternion.identity);
    }
}
