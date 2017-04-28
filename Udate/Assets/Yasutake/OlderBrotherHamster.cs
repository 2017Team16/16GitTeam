using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OlderBrotherHamster : MonoBehaviour
{
    [Header("体力")]
    public int m_Life = 3;
    [Header("無敵時間")]
    public float m_InvincibleInterval = 3.0f;
    private float m_InvincibleTime; //無敵時間をはかる変数
    [Header("速さ")]
    public float m_Speed = 1;
    private GameObject m_Texture; //画像を貼っている子供（仮）
    private Vector3 m_Scale; //画像の向き、右（仮、子の向き）
    private Vector3 reverseScale; //画像の向き、左
    private bool lVec = false; //左を向くか

    [Header("持っている敵の間隔")]
    public float enemyInterval = 1.0f;
    private int enemyCount = 0; //持っている敵の数

    private GameObject youngerBrotherPosition; //弟の位置
    public GameObject youngerBrother; //弟
    private BrotherStateManager brotherState; //弟の状態

    List<Transform> getenemys = new List<Transform>(); //持っている敵

    // Use this for initialization
    void Start()
    {
        m_Texture = transform.FindChild("Quad").gameObject;
        m_Scale = m_Texture.transform.localScale;
        reverseScale = new Vector3(m_Scale.x * -1, m_Scale.y, m_Scale.z);

        youngerBrotherPosition = transform.FindChild("BrosPosition").gameObject;
        m_InvincibleTime = m_InvincibleInterval;

        brotherState = youngerBrother.GetComponent<BrotherStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        BrotherGet();
        m_InvincibleTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F))  //仮
        {
            GameDatas.isFever = true;
        }
    }

    /// <summary>プレイヤーの移動</summary>
    private void Move()
    {
        Vector3 move = transform.position;
        move += new Vector3(Input.GetAxis("Horizontal") * m_Speed, 0, Input.GetAxis("Vertical") * m_Speed)/(enemyCount + 1);
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
        enemy.SendMessage("ChangeState",4, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>持っている弟の処理</summary>
    private void BrotherGet()
    {
        youngerBrotherPosition.transform.localPosition = new Vector3(enemyInterval * (enemyCount + 1), 0, 0);
        if (brotherState.GetState() == BrotherState.NORMAL)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                EnemyKill();
            }
        }
    }

    /// <summary>敵をつぶす</summary>
    private void EnemyKill()
    {
        foreach (Transform chird in transform)
        {
            if (chird.tag == "Enemy")
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
                if(chird.tag == "Enemy")
                {
                    getenemys.Add(chird);
                }
            }
            for(int i = 0; i < getenemys.Count; i++)
            {
                getenemys[i].SendMessage("ChangeState",5, SendMessageOptions.DontRequireReceiver);
                Rigidbody rb = getenemys[i].GetComponent<Rigidbody>();
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
        if (collision.transform.tag == "Enemy")
        {
            Enemy.EnemyState enemyState = collision.gameObject.GetComponent<Enemy>().GetEnemyState();
            if (enemyState == Enemy.EnemyState.WALKING ||
                enemyState == Enemy.EnemyState.CHARGING ||
                enemyState == Enemy.EnemyState.TACKLE)
            {
                if (GameDatas.isFever)
                {
                    EnemyGet(collision.gameObject);
                }
                else
                {
                    Damage();
                }
            }
            if (enemyState == Enemy.EnemyState.SUTAN)
            {
                EnemyGet(collision.gameObject);
            }
        }
    }
}
