using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public float m_Speed;
    public float b_Speed;
    public GameObject bullet;
    public Transform muzzle;

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Fire();
        }
    }
    void Move()
    {
        Vector3 Move = transform.position;
        Move += new Vector3(Input.GetAxis("Horizontal") * m_Speed, 0, Input.GetAxis("Vertical") * m_Speed);
        transform.position = Move;
    }
    private void Fire()
    {
        GameObject bullets = GameObject.Instantiate(bullet) as GameObject;

        Vector3 force;
        force = this.gameObject.transform.forward * b_Speed;

        bullets.GetComponent<Rigidbody>().AddForce(force);

        bullets.transform.position = muzzle.position;
    }
}

