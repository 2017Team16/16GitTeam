using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OlderBrotherHamster : MonoBehaviour
{
    enum PlayerState
    {
        WALK,
        CLIMB
    }
    [Header("体力")]
    public int m_Life = 3;
    [Header("無敵時間")]
    public float m_InvincibleInterval = 3.0f;
    private float m_InvincibleTime; //無敵時間をはかる変数
    [Header("速さ")]
    public float m_Speed = 1;
    [Header("ジャンプ力")]
    public float m_Jump = 3.0f; //デフォルト
    private float jumpVector = 0.0f;  //実際に与えるジャンプ力(重力計算用)
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

    [SerializeField, Header("必殺ゲージ用の値")]
    private float m_SpecialPoint = 0.0f;
    [Header("必殺の継続時間")]
    public float m_SpecialTime = 5.0f;

    private CharacterController m_Controller;
    private PlayerState m_State = PlayerState.WALK;

    private Vector3 climbStartPoint = Vector3.zero;
    private Vector3 climbEndPoint = Vector3.zero;
    private Vector3 climbEndVector = Vector3.zero;
    [Header("登る速さ")]
    public float climbSpeed = 1.0f;
    private float climbStartTime;
    private float climbDistance;

    [Header("ゲームルール(スコア)")]
    public Score gameScore;

    // Use this for initialization
    void Start()
    {
        m_Texture = transform.FindChild("Quad").gameObject;
        m_Scale = m_Texture.transform.localScale;
        reverseScale = new Vector3(m_Scale.x * -1, m_Scale.y, m_Scale.z);

        youngerBrotherPosition = transform.FindChild("BrosPosition").gameObject;
        m_InvincibleTime = m_InvincibleInterval;

        brotherState = youngerBrother.GetComponent<BrotherStateManager>();

        m_Controller = GetComponent<CharacterController>();

        GameDatas.isPlayerLive = true;
        GameDatas.isSpecialAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case PlayerState.WALK: Move(); break;
            case PlayerState.CLIMB: Climb(); break;
        }
        BrotherGet();
        if (GameDatas.isSpecialAttack)
        {
            m_SpecialPoint -= 100.0f / m_SpecialTime * Time.deltaTime;
            if (m_SpecialPoint <= 0.0f)
            {
                m_SpecialPoint = 0.0f;
                GameDatas.isSpecialAttack = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) //デバック用
        {
            Damage();
        }
        m_InvincibleTime += Time.deltaTime;
    }

    /// <summary>プレイヤーの移動</summary>
    private void Move()
    {
        if (!m_Controller.isGrounded)
        {
            jumpVector -= 10 * Time.deltaTime;
        }
        else
        {
            jumpVector = 0.0f;
            if (Input.GetKeyDown(KeyCode.F) && enemyCount == 0)
            {
                jumpVector = m_Jump;
                if (brotherState.GetState() != BrotherState.NORMAL && brotherState.GetState() != BrotherState.THROW) jumpVector *= 1.5f;
            }
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal") / (enemyCount + 1), jumpVector, Input.GetAxis("Vertical") / (enemyCount + 1)) * 10.0f * Time.deltaTime;
        TextureLR();
        if (GameDatas.isSpecialAttack)
        {
            move.x *= 2;
            move.z *= 2;
        }

        m_Controller.Move(move);
    }

    /// <summary>画像をどっち向きにするか</summary>
    private void TextureLR()
    {
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
    }
    

    private void Climb()
    {
        float elapsedTime = (Time.time - climbStartTime) * climbSpeed;
        float nowPoint = elapsedTime / climbDistance;
        transform.position = Vector3.Lerp(climbStartPoint, climbEndPoint, nowPoint);

        if (Vector3.Distance(transform.position, climbEndPoint) < 0.1f)
        {
            m_Controller.Move(climbEndVector);
            m_State = PlayerState.WALK;
        }
    }

    /// <summary>スタンした敵と触れた時の処理</summary>
    /// <param name="enemy">敵のゲームオブジェクト</param>
    private void EnemyGet(GameObject enemy)
    {
        enemyCount++;
        enemy.transform.parent = transform;
        enemy.transform.localPosition = new Vector3(0, enemyInterval * enemyCount, 0);
        enemy.SendMessage("ChangeState", 4, SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>持っている弟の処理</summary>
    private void BrotherGet()
    {
        youngerBrotherPosition.transform.localPosition = new Vector3(0, enemyInterval * (enemyCount + 1), 0);
        if (brotherState.GetState() == BrotherState.NORMAL) //持っているなら
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                EnemyKill();
            }
            if (Input.GetKeyDown(KeyCode.E) && m_SpecialPoint >= 100.0f)
            {
                SpecialAttack();
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
                if (GameDatas.isSpecialAttack)
                {
                    gameScore.Pointscore(10 * enemyCount * 2); //仮
                }
                else
                {
                    m_SpecialPoint += 10.0f;
                    if (m_SpecialPoint > 100.0f) m_SpecialPoint = 100.0f;
                    gameScore.Pointscore(10 * enemyCount); //仮
                }
                Destroy(chird.gameObject);
            }
        }
        enemyCount = 0;
    }

    /// <summary>必殺技</summary>
    private void SpecialAttack()
    {
        GameDatas.isSpecialAttack = true;
        youngerBrother.SendMessage("必殺技", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>敵に当たったときの処理</summary>
    private void Damage()
    {
        if (m_InvincibleTime >= m_InvincibleInterval)
        {
            m_InvincibleTime = 0.0f;
            m_Life--;

            getenemys.Clear();

            foreach (Transform chird in transform)
            {
                if (chird.tag == "Enemy")
                {
                    getenemys.Add(chird);
                }
            }
            for (int i = 0; i < getenemys.Count; i++)
            {
                getenemys[i].SendMessage("ChangeState", 5, SendMessageOptions.DontRequireReceiver);
                //Rigidbody rb = getenemys[i].GetComponent<Rigidbody>();
                //rb.velocity = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                //NavMeshHit navHit = new NavMeshHit();
                //NavMeshAgent e_Agent = getenemys[i].GetComponent<NavMeshAgent>();
                //NavMesh.SamplePosition(e_Agent.transform.localPosition, out navHit, 3.0f, NavMesh.AllAreas);
                //getenemys[i].transform.localPosition = navHit.position;
                getenemys[i].localPosition = new Vector3(Random.Range(-3, 3), -0.5f, Random.Range(-3, 3));
                getenemys[i].parent = null;
            }
            enemyCount = 0;
        }
        if (m_Life <= 0)
        {
            GameDatas.isPlayerLive = false;
            Debug.Log("死んだ");
            //Destroy(gameObject);
        }
        m_State = PlayerState.WALK;
    }

    /// <summary>必殺ゲージ用float型を返す</summary>
    public float GetSpacialPoint()
    {
        return m_SpecialPoint / 100.0f;
    }

    private void ClimbPreparation(float y)
    {
        climbStartPoint = transform.position;
        climbEndPoint = new Vector3(transform.position.x, y, transform.position.z);
        m_State = PlayerState.CLIMB;
        climbStartTime = Time.time;
        climbDistance = Vector3.Distance(climbStartPoint, climbEndPoint);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "FrontClimbStart" && Input.GetAxis("Vertical") > 0.0f)
        {
            Transform end = other.transform.FindChild("ClimbEnd");
            if (end == null) return; ClimbPreparation(end.position.y);
            climbEndVector = Vector3.forward;
        }
        if (other.transform.tag == "LeftClimbStart" && Input.GetAxis("Horizontal") > 0.0f)
        {
            Transform end = other.transform.FindChild("ClimbEnd");
            if (end == null) return; ClimbPreparation(end.position.y);
            climbEndVector = Vector3.right;
        }
        if (other.transform.tag == "RightClimbStart" && Input.GetAxis("Horizontal") < 0.0f)
        {
            Transform end = other.transform.FindChild("ClimbEnd");
            if (end == null) return; ClimbPreparation(end.position.y);
            climbEndVector = Vector3.left;
        }
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Enemy")
        {
            EnemyBase.EnemyState enemyState = hit.gameObject.GetComponent<EnemyBase>().GetEnemyState();
            if (enemyState == EnemyBase.EnemyState.WALKING ||
                enemyState == EnemyBase.EnemyState.CHARGING ||
                enemyState == EnemyBase.EnemyState.ATTACK)
            {
                Damage();
                Debug.Log("いて");
            }
            if (enemyState == EnemyBase.EnemyState.SUTAN)
            {
                EnemyGet(hit.gameObject);
            }
        }
    }
}
