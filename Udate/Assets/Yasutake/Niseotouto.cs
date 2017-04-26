using UnityEngine;
using System.Collections;

public class Niseotouto : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<Enemy>().GetEnemyState() != Enemy.EnemyState.GET)
            {
                collision.gameObject.SendMessage("ChangeState", 3, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
