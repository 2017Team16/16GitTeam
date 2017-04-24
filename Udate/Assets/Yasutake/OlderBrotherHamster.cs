using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OlderBrotherHamster : MonoBehaviour
{
    public int m_Life = 3; //プレイヤーの体力
    private float m_InvincibleTime; //無敵時間をはかる変数
    public float m_InvincibleInterval = 3.0f; //無敵時間

    public float m_Speed = 1; //プレイヤーの速さ
    private GameObject m_Texture; //画像を貼っている子供（仮）
    private Vector3 m_Scale; //画像の向き、右（仮、子の向き）
    private Vector3 reverseScale; //画像の向き、左
    private bool lVec = false; //左を向くか

    private int enemyCount = 0; //持っている敵の数
    public float enemyInterval = 1.0f; //持っている敵の間隔

    private GameObject youngerBrother; //弟

    List<Transform> getenemys = new List<Transform>();

    // Use this for initialization
    void Start()
    {
        m_Texture = transform.FindChild("Quad").gameObject;
        m_Scale = m_Texture.transform.localScale;
        reverseScale = new Vector3(m_Scale.x * -1, m_Scale.y, m_Scale.z);

        youngerBrother = transform.FindChild("Sphere").gameObject;
        m_InvincibleTime = m_InvincibleInterval;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        BrotherGet();
        m_InvincibleTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.E))
        {
            youngerBrother.SendMessage("投げるメソッド", SendMessageOptions.DontRequireReceiver);
            Debug.Log("poi");
        }
    }

    /// <summary>プレイヤーの移動</summary>
    private void Move()
    {
        Vector3 move = transform.position;
        move += new Vector3(Input.GetAxis("Horizontal") * m_Speed, 0, Input.GetAxis("Vertical") * m_Speed);
        if (Input.GetAxis("Horizontal") < 0)
        {
            m_Texture.transform.localScale = reverseScale;
            lVec = true;
        }
        else if (Input.GetAxis("Horizontal") > 0 || !lVec)
        {
            m_Texture.transform.localScale = m_Scale;
            lVec = false;
        }
        transform.position = move;
    }

    /// <summary>スタンした敵と触れた時の処理</summary>
    /// <param name="enemy">敵のゲームオブジェクト</param>
    private void EnemyGet(GameObject enemy)
    {
        enemyCount++;
        enemy.transform.parent = transform;
        enemy.transform.localPosition = new Vector3(enemyInterval * enemyCount, 0, 0);
        enemy.SendMessage("ChangeGet", SendMessageOptions.DontRequireReceiver);
        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    /// <summary>持っている弟の処理</summary>
    private void BrotherGet()
    {
        if (youngerBrother.transform.tag == "YoungerBrother")
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                EnemyKill();
            }
            youngerBrother.transform.localPosition = new Vector3(enemyInterval * (enemyCount + 1), 0, 0);
        }
    }

    /// <summary>敵をつぶす</summary>
    private void EnemyKill()
    {
        foreach (Transform chird in transform)
        {
            if (chird.tag == "GetEnemy")
            {
                Destroy(chird.gameObject);
            }
        }
        enemyCount = 0;
    }

    /// <summary>敵に当たったときの処理</summary>
    private void Damage()
    {
        if(m_InvincibleTime >= m_InvincibleInterval)
        {
            m_InvincibleTime = 0.0f;
            m_Life--;

            getenemys.Clear();

            foreach(Transform chird in transform)
            {
                if(chird.tag == "GetEnemy")
                {
                    getenemys.Add(chird);
                }
            }
            for(int i = 0; i < getenemys.Count; i++)
            {
                getenemys[i].SendMessage("ChangeRecovery", SendMessageOptions.DontRequireReceiver);
                Rigidbody rb = getenemys[i].GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                rb.velocity = new Vector3(Random.Range(-5, 5), Random.Range(3, 6), Random.Range(-5, 5));
                getenemys[i].parent = null;
            }
            enemyCount = 0;
        }
        if(m_Life <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            Damage();
        }
        if (collision.transform.tag == "SutanEnemy")
        {
            EnemyGet(collision.gameObject);
        }
    }
}
