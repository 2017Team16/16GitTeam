using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AniTest : MonoBehaviour
{
    enum PlayerState
    {
        WALK,
        CLIMB,
        CRUSH
    }
    [Header("体力")]
    public int m_Life = 6;
    private int[] m_MaxLife = { 6, 8, 10 }; //最大体力の候補
    private int m_MaxLifeIndex = 0; //いまの最大体力はどれか
    [Header("無敵時間")]
    public float m_InvincibleInterval = 3.0f;
    private float m_InvincibleTime = 0; //無敵時間をはかる変数
    [Header("速さ")]
    public float m_Speed = 1;
    [Header("ジャンプ力")]
    public float m_Jump = 3.0f; //デフォルト
    private GameObject m_Texture; //画像を貼っている子供（仮）
    private Vector3 m_Scale; //画像の向き、右（仮、子の向き）
    private Vector3 reverseScale; //画像の向き、左
    private bool lVec = false; //左を向くか

    [Header("持っている敵の間隔")]
    public float enemyInterval = 1.0f;
    private float enemyIntervalDefault = 0.0f;
    private int enemyCount = 0; //持っている敵の数

    private GameObject youngerBrotherPosition; //弟の位置
    public GameObject youngerBrother; //弟
    private BrotherStateManager brotherState; //弟の状態

    List<Transform> getenemys = new List<Transform>(); //持っている敵

    [SerializeField, Header("必殺ゲージ用の値")]
    private float m_SpecialPoint = 0.0f;
    [Header("必殺の継続時間")]
    public float m_SpecialTime = 5.0f;

    private Rigidbody m_Rigidbody;
    private PlayerState m_State = PlayerState.WALK;

    private Vector3 climbStartPoint = Vector3.zero; //壁のぼり開始地点
    private Vector3 climbEndPoint = Vector3.zero; //壁のぼり終了地点
    private Vector3 climbEndVector = Vector3.zero; //壁のぼり後、落ちないように進む方向
    [Header("登る速さ")]
    public float climbSpeed = 1.0f;
    private float climbStartTime; //壁のぼり開始時間
    private float climbDistance; //登る壁の長さ

    private int m_Chain = 0;


    //アニメ用変数たち
    private Animator m_Animator;
    private bool isWithBrother = false;
    private BrotherState maeBroState = BrotherState.NONE;
    private AnimatorStateInfo stateInfo;

    private Animator brotherAnimator;
    private AnimatorStateInfo brotherstateInfo;
    private Vector3 brotherTarget = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        AddLife(0);
        m_Texture = transform.FindChild("Ani").gameObject;
        m_Scale = m_Texture.transform.localScale;
        reverseScale = new Vector3(m_Scale.x * -1, m_Scale.y, m_Scale.z);
        m_Animator = m_Texture.GetComponent<Animator>();

        youngerBrotherPosition = transform.FindChild("BrosPosition").gameObject;
        brotherAnimator = youngerBrotherPosition.GetComponent<Animator>();

        m_InvincibleTime = m_InvincibleInterval;
        enemyIntervalDefault = enemyInterval;

        brotherState = youngerBrother.GetComponent<BrotherStateManager>();

        m_Rigidbody = GetComponent<Rigidbody>();

        GameDatas.isPlayerLive = true;
        GameDatas.isSpecialAttack = false;
        isWithBrother = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameDatas.isPlayerLive)
        {
            return;
        }
        stateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        brotherstateInfo = brotherAnimator.GetCurrentAnimatorStateInfo(0);
        switch (m_State)
        {
            case PlayerState.WALK: Move(); break;
            case PlayerState.CLIMB: Climb(); break;
            case PlayerState.CRUSH: Crush(); break;
        }
        if (m_InvincibleTime < m_InvincibleInterval)
        {
            m_Texture.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.8f);

            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerDamageWithBrother") ||
                stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerDamageWithEnemy") ||
                stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerDamageSolo"))
            {
                float duration = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                if (duration > 0.9f && m_InvincibleTime > 0)
                {
                    WalkAnimeControl();
                }
            }

            m_InvincibleTime += Time.deltaTime;
            if (m_InvincibleTime >= m_InvincibleInterval)
            {
                m_Texture.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        if (m_State == PlayerState.CRUSH) return;

        BrotherGet();
        if (GameDatas.isSpecialAttack) //必殺技の効果時間
        {
            m_SpecialPoint -= 100.0f / m_SpecialTime * Time.deltaTime;
            if (m_SpecialPoint <= 0.0f)
            {
                m_SpecialPoint = 0.0f;
                GameDatas.isSpecialAttack = false;
            }
        }

        maeBroState = brotherState.GetState();
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerPickUpSolo") ||
           stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerPickUpWithBrother") ||
           stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowBrother"))
        {
            float duration = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (duration > 0.9f) //再生終わりまで行ったら歩きなどのアニメへ
            {
                WalkAnimeControl();
            }
        }
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerJumpPoseSolo") && m_Rigidbody.velocity.y < 0 ||
            stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerJumpPoseWithBrother") && m_Rigidbody.velocity.y < 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 1.0f)) //ジャンプ中地面についたら歩きへ
            {
                WalkAnimeControl();
            }
        }
    }

    /// <summary>プレイヤーの移動</summary>
    private void Move()
    {
        Vector3 newVelocity = m_Rigidbody.velocity;
        RaycastHit hit;
        Debug.DrawRay(transform.position, -Vector3.up, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 1.0f) && Input.GetButtonDown("XboxA") && enemyCount == 0)
        {
            newVelocity.y = m_Jump;
            if (GameDatas.isBrotherFlying)
            {
                newVelocity.y *= 1.5f;
                m_Animator.Play("PlayerJumpStartSolo");
            }
            else
            {
                m_Animator.Play("PlayerJumpStartWithBrother");
            }
            m_Rigidbody.velocity = newVelocity;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal") / (enemyCount + 1), 0,
            Input.GetAxis("Vertical") / (enemyCount + 1)) * m_Speed * Time.deltaTime;
        TextureLR();
        if (GameDatas.isSpecialAttack)
        {
            move.x *= 2;
            move.z *= 2;
        }
        m_Rigidbody.MovePosition(transform.position + move);
        m_Animator.SetFloat("speed", Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")));
    }

    /// <summary>画像をどっち向きにするか</summary>
    private void TextureLR()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            m_Texture.transform.localScale = reverseScale;
            youngerBrotherPosition.transform.localScale = reverseScale;
            lVec = true;
        }
        else if (Input.GetAxis("Horizontal") > 0 || !lVec)
        {
            m_Texture.transform.localScale = m_Scale;
            youngerBrotherPosition.transform.localScale = m_Scale;
            lVec = false;
        }
    }

    /// <summary>壁のぼり</summary>
    private void Climb()
    {
        float speed;
        speed = (GameDatas.isSpecialAttack) ? climbSpeed * 2 : climbSpeed;

        float elapsedTime = (Time.time - climbStartTime) * speed / (enemyCount + 1);
        float nowPoint = elapsedTime / climbDistance;
        transform.position = Vector3.Lerp(climbStartPoint, climbEndPoint, nowPoint);

        if (Vector3.Distance(transform.position, climbEndPoint) < 0.1f)
        {
            m_Rigidbody.MovePosition(transform.position + climbEndVector);
            m_State = PlayerState.WALK;
            WalkAnimeControl();
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            m_State = PlayerState.WALK;
            if (GameDatas.isBrotherFlying) m_Animator.Play("PlayerJumpPoseSolo");
            else m_Animator.Play("PlayerJumpPoseWithBrother");
        }
        m_Rigidbody.velocity = Vector3.zero;
    }

    /// <summary>敵をつぶす演出</summary>
    private void Crush()
    {
        if (brotherstateInfo.fullPathHash == Animator.StringToHash("Base Layer.BrotherCrushStart"))
        {
            float duration = brotherAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (duration > 0.9f)
            {
                enemyInterval *= 0.95f;
                int getenemycount = 0;
                foreach (Transform chird in transform)
                {
                    if (chird.tag == "Enemy")
                    {
                        getenemycount++;
                        Vector3 newScale = chird.transform.localScale;
                        newScale.y *= 0.95f;
                        chird.transform.localScale = newScale;
                        chird.transform.localPosition = new Vector3(0, enemyInterval * (getenemycount - 1) + 3, 0);
                    }
                }
            }
        }

        youngerBrotherPosition.transform.localPosition = new Vector3(0, enemyInterval * enemyCount + 3, 0);
        if (enemyInterval < enemyIntervalDefault / 2.0f)
        {
            brotherAnimator.Play("BrotherCrushEnd");
            EnemyKill();
            enemyInterval = enemyIntervalDefault;
            m_State = PlayerState.WALK;
        }
    }

    /// <summary>スタンした敵と触れた時の処理</summary>
    /// <param name="enemy">敵のゲームオブジェクト</param>
    private void EnemyGet(GameObject enemy)
    {
        if (enemyCount == 0 && !isWithBrother) m_Animator.Play("PlayerPickUpSolo");
        else m_Animator.Play("PlayerPickUpWithBrother");
        enemyCount++;
        enemy.transform.parent = transform;
        enemy.transform.localPosition = new Vector3(0, enemyInterval * (enemyCount - 1) + 3, 0);
        enemy.SendMessage("ChangeState", 4, SendMessageOptions.DontRequireReceiver);
        enemy.GetComponent<Collider>().enabled = false;
    }

    /// <summary>持っている弟の処理</summary>
    private void BrotherGet()
    {
        if (brotherState.GetState() == BrotherState.NORMAL) //持っているなら
        {
            if (isWithBrother)
            {
                youngerBrotherPosition.transform.localPosition = new Vector3(0, enemyInterval * enemyCount + 3, 0);
                if (Input.GetButtonDown("XboxB"))
                {
                    //EnemyKill();
                    m_State = PlayerState.CRUSH;
                    brotherAnimator.Play("BrotherCrushStart");
                }
                if (Input.GetButtonDown("XboxR1") && m_SpecialPoint >= 100.0f)
                {
                    SpecialAttack();
                }
                GetComponent<CapsuleCollider>().height = 2 + enemyInterval * enemyCount + 1; //兄の分＋敵の分＋弟の分のあたり判定
                GetComponent<CapsuleCollider>().center = new Vector3(0, GetComponent<CapsuleCollider>().height / 2, 0);
            }
            else
            {
                youngerBrother.GetComponent<MeshRenderer>().enabled = false;
                youngerBrotherPosition.GetComponent<SpriteRenderer>().enabled = true;
                youngerBrother.GetComponent<Collider>().enabled = false;
                GameDatas.isBrotherFlying = false;
                youngerBrotherPosition.transform.localPosition += Vector3.up / 5.0f;

                if (lVec)
                {
                    youngerBrotherPosition.transform.localPosition = new Vector3(1.0f, youngerBrotherPosition.transform.localPosition.y, 0);
                    brotherTarget.x = 1.0f;
                }
                else
                {
                    youngerBrotherPosition.transform.localPosition = new Vector3(-1.0f, youngerBrotherPosition.transform.localPosition.y, 0);
                    brotherTarget.x = -1.0f;
                }

                if (Vector3.Distance(youngerBrotherPosition.transform.localPosition, brotherTarget) < 0.5f)
                {
                    isWithBrother = true;
                    brotherAnimator.Play("BrotherWait");
                    WalkAnimeControl();
                }
            }
            youngerBrother.GetComponent<MeshRenderer>().enabled = false;
            youngerBrotherPosition.GetComponent<SpriteRenderer>().enabled = true;
            youngerBrother.GetComponent<Collider>().enabled = false;
            GameDatas.isBrotherFlying = false;
        }
        else if (brotherState.GetState() == BrotherState.THROW &&
            !GameDatas.isBrotherFlying && maeBroState == brotherState.GetState())
        {
            if (!isWithBrother)
            {
                youngerBrotherPosition.transform.localPosition = new Vector3(0, enemyInterval * enemyCount + 3, 0);
                isWithBrother = true;
                brotherAnimator.Play("BrotherWait");
                WalkAnimeControl();
            }

            GetComponent<CapsuleCollider>().height = 2 + enemyInterval * enemyCount + 1; //兄の分＋敵の分＋弟の分のあたり判定
            GetComponent<CapsuleCollider>().center = new Vector3(0, GetComponent<CapsuleCollider>().height / 2, 0);
            if (Input.GetButtonDown("XboxL1"))
            {
                ThrowAnimeControl();
            }
            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowBrother") ||
                brotherstateInfo.fullPathHash == Animator.StringToHash("Base Layer.BrotherFlyStartEnd"))
            {
                GameDatas.isBrotherFlying = true;
                isWithBrother = false;
            }
        }
        else if (GameDatas.isBrotherFlying)
        {
            youngerBrother.GetComponent<MeshRenderer>().enabled = true;
            youngerBrotherPosition.GetComponent<SpriteRenderer>().enabled = false;
            youngerBrother.GetComponent<Collider>().enabled = true;
            GetComponent<CapsuleCollider>().height = 2 + enemyInterval * enemyCount;
            GetComponent<CapsuleCollider>().center = new Vector3(0, GetComponent<CapsuleCollider>().height / 2, 0);
        }
    }

    /// <summary>敵をつぶす</summary>
    private void EnemyKill()
    {
        float score = 0;

        foreach (Transform chird in transform)
        {
            if (chird.tag == "Enemy")
            {
                if (GameDatas.isSpecialAttack)
                {
                    score += chird.GetComponent<EnemyBase>().EnemyScore() * 2;
                }
                else
                {
                    score += chird.GetComponent<EnemyBase>().EnemyScore();
                    m_SpecialPoint += 10.0f;
                    if (m_SpecialPoint > 100.0f) m_SpecialPoint = 100.0f;
                }
                m_Chain++;
                Destroy(chird.gameObject);
            }
        }
        enemyCount = 0;
    }

    /// <summary>必殺技</summary>
    private void SpecialAttack()
    {
        GameDatas.isSpecialAttack = true;
        youngerBrother.SendMessage("Special", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>敵に当たったときの処理</summary>
    private void Damage()
    {
        if (m_InvincibleTime >= m_InvincibleInterval)
        {
            m_InvincibleTime = 0.0f;

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
                float randX = Random.Range(-1.0f, 1.0f);
                float randZ = Random.Range(-1.0f, 1.0f);
                getenemys[i].parent = null;
                getenemys[i].GetComponent<Collider>().enabled = true;
                Rigidbody rb = getenemys[i].GetComponent<Rigidbody>();
                rb.velocity = new Vector3(randX * 10, Random.Range(0.1f, 1.0f), randZ * 10);
            }
            DamageAnimeControl();
            enemyCount = 0;
            m_Chain = 0;
            m_State = PlayerState.WALK;
            AddLife(-1);
        }

    }
    /// <summary>体力の増減</summary>
    /// <param name="n">足す数値</param>
    public void AddLife(int n)
    {
        m_Life += n;
        m_Life = Mathf.Clamp(m_Life, 0, m_MaxLife[m_MaxLifeIndex]);
        if (m_Life == 0)
        {
            GameDatas.isPlayerLive = false;
            m_Animator.Play("PlayerDeath");
            youngerBrother.GetComponent<MeshRenderer>().enabled = true;
            youngerBrotherPosition.GetComponent<SpriteRenderer>().enabled = false;
            youngerBrother.GetComponent<Collider>().enabled = true;
        }
        if (m_MaxLifeIndex == 2 && m_Life <= m_MaxLife[1]) m_MaxLifeIndex--;
        if (m_MaxLifeIndex == 1 && m_Life <= m_MaxLife[0]) m_MaxLifeIndex--;
    }

    /// <summary>最大体力の増加</summary>
    public void AddMaxLife()
    {
        m_MaxLifeIndex++;
        if (m_MaxLifeIndex > 2) m_MaxLifeIndex = 2;
        AddLife(2);
    }

    /// <summary>必殺ゲージ用float型を返す</summary>
    public float GetSpacialPoint()
    {
        return m_SpecialPoint / 100.0f;
    }

    /// <summary>壁のぼりの準備</summary>
    /// <param name="y">登る高さ</param>
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
        if (m_State != PlayerState.WALK || !GameDatas.isPlayerLive ||
            stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowStart") ||
            stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowBrother")) return;
        if (other.transform.tag == "FrontClimbStart" && Input.GetAxis("Vertical") > 0.0f)
        {
            Transform end = other.transform.FindChild("ClimbEnd");
            if (end == null) return; ClimbPreparation(end.position.y);
            climbEndVector = Vector3.forward;
            m_Animator.Play("PlayerClimbZ");
        }
        if (other.transform.tag == "LeftClimbStart" && Input.GetAxis("Horizontal") > 0.0f)
        {
            Transform end = other.transform.FindChild("ClimbEnd");
            if (end == null) return; ClimbPreparation(end.position.y);
            climbEndVector = Vector3.right;
            m_Animator.Play("PlayerClimbX");
        }
        if (other.transform.tag == "RightClimbStart" && Input.GetAxis("Horizontal") < 0.0f)
        {
            Transform end = other.transform.FindChild("ClimbEnd");
            if (end == null) return; ClimbPreparation(end.position.y);
            climbEndVector = Vector3.left;
            m_Animator.Play("PlayerClimbX");
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (m_State == PlayerState.CRUSH || !GameDatas.isPlayerLive ||
            stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowStart") ||
            stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowBrother")) return;
        if (collision.transform.tag == "Enemy")
        {
            EnemyBase.EnemyState enemyState = collision.gameObject.GetComponent<EnemyBase>().GetEnemyState();
            if (enemyState == EnemyBase.EnemyState.WALKING ||
                enemyState == EnemyBase.EnemyState.CHARGING ||
                enemyState == EnemyBase.EnemyState.ATTACK)
            {
                Damage();
            }
            if (enemyState == EnemyBase.EnemyState.SUTAN)
            {
                EnemyGet(collision.gameObject);
            }
        }
        if (collision.transform.tag == "EnemyBullet")
        {
            Damage();
        }
        if (collision.gameObject == youngerBrother && brotherState.GetState() == BrotherState.BACK)
        {
            brotherTarget = new Vector3(-1, enemyInterval * enemyCount + 3, 0);
            youngerBrotherPosition.transform.localPosition = new Vector3(-1, 1, 0);
            brotherAnimator.Play("BrotherClimb");
        }
    }

    private void WalkAnimeControl()
    {
        if (enemyCount == 0 && !isWithBrother)
        {
            if (m_Animator.GetFloat("speed") < 0.1) m_Animator.Play("PlayerWaitSolo");
            else m_Animator.Play("PlayerRunSolo");
        }
        else
        {
            if (m_Animator.GetFloat("speed") < 0.1) m_Animator.Play("PlayerWaitWithEnemy");
            else m_Animator.Play("PlayerRunWithEnemy");
        }
    }

    private void DamageAnimeControl()
    {
        if (isWithBrother)
        {
            m_Animator.Play("PlayerDamageWithBrother");
        }
        else if (enemyCount == 0)
        {
            m_Animator.Play("PlayerDamageSolo");
        }
        else
        {
            m_Animator.Play("PlayerDamageWithEnemy");
        }
    }

    private void ThrowAnimeControl()
    {
        if (enemyCount == 0 && m_State != PlayerState.CLIMB)
        {
            m_Animator.Play("PlayerThrowStart");
        }
        else
        {
            brotherAnimator.Play("BrotherFlyStart");
        }
    }

}
