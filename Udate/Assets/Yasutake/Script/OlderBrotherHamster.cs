using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OlderBrotherHamster : MonoBehaviour
{
    enum PlayerState
    {
        WALK,
        CLIMB,
        CRUSH,
        GETTING
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
    private bool isDefault = false;
    [Header("アイテムによるデフォルトのスピードになる効果時間")]
    public float m_DefaultTime = 10.0f;
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
    [Header("アイテムによる必殺ゲージ増加量")]
    public float m_SpecialItem = 50.0f;

    private Rigidbody m_Rigidbody;
    private PlayerState m_State = PlayerState.WALK;

    private Vector3 climbStartPoint = Vector3.zero; //壁のぼり開始地点
    private Vector3 climbEndPoint = Vector3.zero; //壁のぼり終了地点
    private Vector3 climbEndVector = Vector3.zero; //壁のぼり後、落ちないように進む方向
    [Header("登る速さ")]
    public float climbSpeed = 1.0f;
    private float climbStartTime; //壁のぼり開始時間
    private float climbDistance; //登る壁の長さ

    [Header("ゲームルール(スコア)")]
    public Score gameScore;
    private int m_Chain = 0;

    [Header("魂のプレハブ")]
    public GameObject mySoul;
    [Header("汗のプレハブ")]
    public GameObject mySweat;
    private float sewatTime = 0.0f;

    //サウンド関係
    private AudioSource m_Audio;
    [Header("プレイヤーのサウンドたち")]
    public AudioClip[] m_Clips;
    private float walkSoundPlayInterval = 0.0f;
    private bool isJump = false;
    private int soundNum = 0;
    private int soundCount = 0;

    //アニメ用変数たち
    private Animator m_Animator;
    private bool isWithBrother = false;
    private BrotherState maeBroState = BrotherState.NONE;
    private AnimatorStateInfo stateInfo;

    private Animator brotherAnimator;
    private AnimatorStateInfo brotherstateInfo;
    private Vector3 brotherTarget = Vector3.zero;

    //パーティクル関係
    private GameObject m_CrushParticle;
    private GameObject m_ChirdJumpParent;
    private GameObject m_GettingEnemy;
    private GameObject m_GettingEnemyParent;

    private CrushScore m_CrushScore;
    private float maxSpecial = 0.0f;

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

        m_Audio = GetComponent<AudioSource>();

        m_CrushParticle = transform.FindChild("CrushEffectParent").gameObject;
        m_ChirdJumpParent = transform.FindChild("GettingEffectParent").gameObject;
        m_GettingEnemyParent = transform.FindChild("GettingEnemy").gameObject;
        
        m_CrushScore = transform.FindChild("CrushScore").GetComponent<CrushScore>();

        GameDatas.isPlayerLive = true;
        GameDatas.isSpecialAttack = false;
        GameDatas.isBrotherFlying = false;
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
            case PlayerState.GETTING: Getting(); break;
        }
        if (m_InvincibleTime < m_InvincibleInterval) //ダメージを受けて無敵の時の処理
        {
            if((int)(m_InvincibleTime *10) % 2 == 0)
            {
                m_Texture.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                m_Texture.GetComponent<SpriteRenderer>().color = Color.white;
            }
            //m_Texture.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.8f);

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

        if (m_State == PlayerState.CRUSH || m_State == PlayerState.GETTING) return;

        if(soundCount > 0)
        {
            m_Audio.PlayOneShot(m_Clips[soundNum]);
            soundCount--;
        }

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
        if (m_SpecialPoint < maxSpecial)
        {
            m_SpecialPoint += 100.0f / m_SpecialTime * Time.deltaTime;
            if (m_SpecialPoint >= maxSpecial)
            {
                m_SpecialPoint = maxSpecial;
            }
        }

        maeBroState = brotherState.GetState();

        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowBrother"))
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
                isJump = false;
            }
        }

        Sweat();
    }

    /// <summary>プレイヤーの移動</summary>
    private void Move()
    {
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerClimbX"))
        {
            WalkAnimeControl();
        }
        Vector3 newVelocity = m_Rigidbody.velocity;
        RaycastHit hit;
        Debug.DrawRay(transform.position, -Vector3.up, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 1.0f) 
            && Input.GetButtonDown("XboxA") && enemyCount == 0 && Time.timeScale != 0)
        {
            newVelocity.y = m_Jump;
            isJump = true;
            m_Audio.PlayOneShot(m_Clips[2]);
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

        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 move = input / (enemyCount * 0.2f + 1);
        if (enemyCount > 3) move = input / (enemyCount + 1);
        TextureLR();
        if (isDefault)
        {
            move = input;
            m_DefaultTime -= Time.deltaTime;
            if (m_DefaultTime < 0)
            {
                isDefault = false;
                m_DefaultTime = 10.0f;
            }
        }
        if (GameDatas.isSpecialAttack)
        {
            move = 1.5f * input;
        }
        //m_Rigidbody.velocity = new Vector3(move.x * m_Speed, m_Rigidbody.velocity.y, move.z * m_Speed);
        m_Rigidbody.MovePosition(transform.position + move * m_Speed * Time.deltaTime);
        float f = Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));
        m_Animator.SetFloat("speed", f);
        if (walkSoundPlayInterval > 0.4f && f > 0.5f && !isJump)
        {
            m_Audio.PlayOneShot(m_Clips[0]);
            walkSoundPlayInterval = 0.0f;
        }
        if (f > 0.5f && !isJump)
        {
            walkSoundPlayInterval += Time.deltaTime;
        }

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

        float elapsedTime = (Time.time - climbStartTime) * speed / (enemyCount * 0.2f + 1);
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

    /// <summary>壁のぼりの準備</summary>
    /// <param name="y">登る高さ</param>
    private void ClimbPreparation(float y)
    {
        TextureLR();
        climbStartPoint = transform.position;
        climbEndPoint = new Vector3(transform.position.x, y, transform.position.z);
        m_State = PlayerState.CLIMB;
        climbStartTime = Time.time;
        climbDistance = Vector3.Distance(climbStartPoint, climbEndPoint);
    }

    /// <summary>持っている弟の処理</summary>
    private void BrotherGet()
    {
        if (brotherState.GetState() == BrotherState.NORMAL) //持っているなら
        {
            if (isWithBrother)
            {
                //                                                                                   敵の間隔　　　　数　　　弟の位置調整　兄の高さ分
                if (enemyCount != 0) youngerBrotherPosition.transform.localPosition = new Vector3(0, enemyInterval * enemyCount + 0.3f + 2.5f, 0);
                else youngerBrotherPosition.transform.localPosition = new Vector3(0, enemyInterval * enemyCount + 2.5f, 0);
                if (Time.timeScale != 0)
                {
                    if (Input.GetButtonDown("XboxB") && m_State == PlayerState.WALK)
                    {
                        //EnemyKill();
                        m_State = PlayerState.CRUSH;
                        brotherAnimator.Play("BrotherCrushStart");
                    }
                    if (Input.GetButtonDown("XboxR1") && m_SpecialPoint >= 100.0f)
                    {
                        SpecialAttack();
                    }
                }
                GetComponent<CapsuleCollider>().height = 2 + enemyInterval * enemyCount + 1; //兄の分＋敵の分＋弟の分のあたり判定
                GetComponent<CapsuleCollider>().center = new Vector3(0, GetComponent<CapsuleCollider>().height / 2, 0);
            }
            else
            {
                //youngerBrother.GetComponent<MeshRenderer>().enabled = false;
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

                if (youngerBrotherPosition.transform.localPosition.y > brotherTarget.y)
                {
                    isWithBrother = true;
                    brotherAnimator.Play("BrotherWait");
                    WalkAnimeControl();
                }
            }
            //youngerBrother.GetComponent<MeshRenderer>().enabled = false;
            youngerBrotherPosition.GetComponent<SpriteRenderer>().enabled = true;
            youngerBrother.GetComponent<Collider>().enabled = false;
            GameDatas.isBrotherFlying = false;
        }
        else if (brotherState.GetState() == BrotherState.THROW &&
            !GameDatas.isBrotherFlying && maeBroState == brotherState.GetState())
        {
            if (!isWithBrother)
            {
                isWithBrother = true;
                brotherAnimator.Play("BrotherWait");
                WalkAnimeControl();
            }

            if (enemyCount != 0) youngerBrotherPosition.transform.localPosition = new Vector3(0, enemyInterval * enemyCount + 0.3f + 2.5f, 0);
            else youngerBrotherPosition.transform.localPosition = new Vector3(0, enemyInterval * enemyCount + 2.5f, 0);

            GetComponent<CapsuleCollider>().height = 2 + enemyInterval * enemyCount + 1; //兄の分＋敵の分＋弟の分のあたり判定
            GetComponent<CapsuleCollider>().center = new Vector3(0, GetComponent<CapsuleCollider>().height / 2, 0);
            if (Input.GetButtonDown("XboxL1") && Time.timeScale != 0)
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
        else if (GameDatas.isBrotherFlying || GameDatas.isSpecialAttack)
        {
            //youngerBrother.GetComponent<MeshRenderer>().enabled = true;
            youngerBrotherPosition.GetComponent<SpriteRenderer>().enabled = false;
            youngerBrother.GetComponent<Collider>().enabled = true;
            GetComponent<CapsuleCollider>().height = 2 + enemyInterval * enemyCount;
            GetComponent<CapsuleCollider>().center = new Vector3(0, GetComponent<CapsuleCollider>().height / 2, 0);
        }
    }

    /// <summary>必殺技</summary>
    private void SpecialAttack()
    {
        GameDatas.isSpecialAttack = true;
        youngerBrother.SendMessage("Special", SendMessageOptions.DontRequireReceiver);
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
                int getenemycount = enemyCount;
                foreach (Transform chird in transform)
                {
                    if (chird.tag == "Enemy")
                    {
                        getenemycount--;
                        Vector3 newScale = chird.transform.localScale;
                        newScale.y *= 0.95f;
                        chird.transform.localScale = newScale;
                        chird.transform.localPosition = new Vector3(0, enemyInterval * (getenemycount - 1) + 1.8f, 0);
                    }
                }
            }
        }

        youngerBrotherPosition.transform.localPosition = new Vector3(0, enemyInterval * enemyCount + 2.5f, 0);
        if (enemyInterval < enemyIntervalDefault / 2.0f)
        {
            brotherAnimator.Play("BrotherCrushEnd");
            CrushSound();
            EnemyKill();
            enemyInterval = enemyIntervalDefault;
            m_State = PlayerState.WALK;
        }
    }

    /// <summary>敵を持つときの処理</summary>
    private void Getting()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 move = input / (enemyCount * 0.2f + 1);
        if (enemyCount > 3) move = input / (enemyCount + 1);
        if (isDefault)
        {
            move = input;
            m_DefaultTime -= Time.deltaTime;
            if (m_DefaultTime < 0)
            {
                isDefault = false;
                m_DefaultTime = 10.0f;
            }
        }
        if (GameDatas.isSpecialAttack)
        {
            move = 1.5f * input;
        }
        m_Rigidbody.MovePosition(transform.position + move * m_Speed * Time.deltaTime);

        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerPickUpSolo") ||
           stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerPickUpWithBrother"))
        {
            float duration = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (duration > 0.9f) //再生終わりまで行ったら歩きなどのアニメへ
            {
                EnemyGetEnd();
            }
        }
    }

    /// <summary>敵を持つときの準備</summary>
    /// <param name="enemy">敵のゲームオブジェクト</param>
    private void EnemyGetPreparation()
    {
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
            getenemys[i].parent = m_ChirdJumpParent.transform;
        }
        youngerBrotherPosition.transform.parent = m_ChirdJumpParent.transform;
        m_GettingEnemy.transform.parent = m_GettingEnemyParent.transform;
        m_GettingEnemy.transform.localPosition = m_GettingEnemyParent.transform.localPosition;

        m_GettingEnemy.SendMessage("ChangeState", 4, SendMessageOptions.DontRequireReceiver);
        m_GettingEnemy.GetComponent<Collider>().enabled = false;



        m_ChirdJumpParent.GetComponent<Animator>().Play("PlayerGettingGetEnemys");

        if (lVec) m_GettingEnemyParent.GetComponent<Animator>().Play("PlayerGettingEnemy2");
        else m_GettingEnemyParent.GetComponent<Animator>().Play("PlayerGettingEnemy");

        if (enemyCount == 0 && !isWithBrother) m_Animator.Play("PlayerPickUpSolo");
        else m_Animator.Play("PlayerPickUpWithBrother");

        m_State = PlayerState.GETTING;
    }

    /// <summary>敵を持つときの演出の終了</summary>
    private void EnemyGetEnd()
    {
        for (int i = 0; i < getenemys.Count; i++)
        {
            getenemys[i].parent = transform;
        }
        enemyCount++;
        int getenemycount = enemyCount;
        foreach (Transform chird in transform)
        {
            if (chird.tag == "Enemy")
            {
                getenemycount--;
                chird.transform.localPosition = new Vector3(0, enemyInterval * (getenemycount) + 1.8f, 0);
            }
        }
        youngerBrotherPosition.transform.parent = transform;
        m_GettingEnemy.transform.parent = transform;
        m_GettingEnemy.transform.localPosition = new Vector3(0, 1.8f, 0);
        m_GettingEnemy.SendMessage("Get", enemyCount, SendMessageOptions.DontRequireReceiver);


        m_State = PlayerState.WALK;
        WalkAnimeControl();
    }

    ///// <summary>スタンした敵と触れた時の処理</summary>
    ///// <param name="enemy">敵のゲームオブジェクト</param>
    //private void EnemyGet(GameObject enemy)
    //{
    //    if (enemyCount == 0 && !isWithBrother) m_Animator.Play("PlayerPickUpSolo");
    //    else m_Animator.Play("PlayerPickUpWithBrother");
    //    enemyCount++;
    //    int getenemycount = enemyCount;
    //    foreach (Transform chird in transform)
    //    {
    //        if (chird.tag == "Enemy")
    //        {
    //            getenemycount--;
    //            chird.transform.localPosition = new Vector3(0, enemyInterval * (getenemycount) + 1.8f, 0);
    //        }
    //    }
    //    enemy.transform.parent = transform;
    //    enemy.transform.localPosition = new Vector3(0, 1.8f, 0);
    //    enemy.SendMessage("ChangeState", 4, SendMessageOptions.DontRequireReceiver);
    //    enemy.GetComponent<Collider>().enabled = false;

    //    enemy.SendMessage("Get", enemyCount, SendMessageOptions.DontRequireReceiver);

    //}

    /// <summary>敵をつぶす</summary>
    private void EnemyKill()
    {
        int score = 0;

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
                    maxSpecial += 10.0f;
                    if (maxSpecial > 100.0f) maxSpecial = 100.0f;
                }
                m_Chain++;
                Destroy(chird.gameObject);
            }
        }
        m_CrushScore.SetNumbers(score * enemyCount);
        gameScore.Pointscore(score, m_Chain, enemyCount);
        enemyCount = 0;
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
            if (m_Life != 0) m_Audio.PlayOneShot(m_Clips[9]);
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
            GameObject soul = Instantiate(mySoul);
            soul.transform.position = transform.position;
            soul.transform.parent = transform;
            GameDatas.isPlayerLive = false;
            m_Animator.Play("PlayerDeath");
            m_Audio.PlayOneShot(m_Clips[3]);
            //youngerBrother.GetComponent<MeshRenderer>().enabled = true;
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

    /// <summary>必殺技用ゲージの増加</summary>
    public void AddSpecialPoint()
    {
        maxSpecial += m_SpecialItem;
        if (maxSpecial > 100.0f) maxSpecial = 100.0f;
    }

    /// <summary>デフォルトの速さで歩く</summary>
    public void DefaultSpeedWalk()
    {
        isDefault = true;
    }

    /// <summary>必殺ゲージ用float型を返す</summary>
    public float GetSpacialPoint()
    {
        return m_SpecialPoint / 100.0f;
    }

    /// <summary>歩きだすときに、どのアニメを再生するか</summary>
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

    /// <summary>ダメージを受けた時に、どのアニメを再生するか</summary>
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

    /// <summary>投げるときに、どのアニメを再生するか</summary>
    private void ThrowAnimeControl()
    {
        if (enemyCount == 0 && m_State != PlayerState.CLIMB)
        {
            m_Audio.PlayOneShot(m_Clips[1]);
            m_Animator.Play("PlayerThrowStart");
        }
        else
        {
            m_Audio.PlayOneShot(m_Clips[8]);
            brotherAnimator.Play("BrotherFlyStart");
        }
    }

    /// <summary>敵をつぶすときに、どの音を鳴らすか</summary>
    private void CrushSound()
    {
        if (enemyCount == 0) return;
        int sound = 4;
        int pSN = 1;
        if (enemyCount < 3)
        {
            sound = 4;
            pSN = 1;
        }
        else
        if (enemyCount < 5)
        {
            sound = 5;
            pSN = 2;
        }
        else if (enemyCount < 7)
        {
            sound = 6;
            pSN = 3;
        }
        else
        {
            sound = 7;
            pSN = 3;
        }
        soundNum = sound;
        soundCount = enemyCount;
        //m_Audio.PlayOneShot(m_Clips[sound]);
        foreach (Transform chird in m_CrushParticle.transform)
        {
            chird.SendMessage("Crush", pSN, SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>たくさん敵を持った時のプレイヤーの汗</summary>
    private void Sweat()
    {
        sewatTime += Time.deltaTime;
        if (enemyCount >= 4 && sewatTime > 0.3f)
        {
            GameObject sweat = Instantiate(mySweat);
            sweat.transform.parent = transform;
            sewatTime = 0.0f;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (m_State != PlayerState.WALK || !GameDatas.isPlayerLive ||
            stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowStart") ||
            stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowBrother")) return;
        if (other.transform.tag == "FrontClimbStart" && Input.GetAxis("Vertical") > 0.0f)
        {
            Transform end = other.transform.FindChild("ClimbEnd");
            if (end == null) return;
            ClimbPreparation(end.position.y);
            climbEndVector = Vector3.forward;
            m_Animator.Play("PlayerClimbZ");
        }
        if (other.transform.tag == "LeftClimbStart" && Input.GetAxis("Horizontal") > 0.0f)
        {
            Transform end = other.transform.FindChild("ClimbEnd");
            if (end == null) return;
            ClimbPreparation(end.position.y);
            climbEndVector = Vector3.right;
            m_Animator.Play("PlayerClimbX");
        }
        if (other.transform.tag == "RightClimbStart" && Input.GetAxis("Horizontal") < 0.0f)
        {
            Transform end = other.transform.FindChild("ClimbEnd");
            if (end == null) return;
            ClimbPreparation(end.position.y);
            climbEndVector = Vector3.left;
            m_Animator.Play("PlayerClimbX");
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        if (m_State == PlayerState.CRUSH || m_State == PlayerState.GETTING || !GameDatas.isPlayerLive ||
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
                m_GettingEnemy = collision.gameObject;
                //EnemyGet(collision.gameObject);
                EnemyGetPreparation();
            }
        }
        if (collision.gameObject == youngerBrother && brotherState.GetState() == BrotherState.BACK)
        {
            brotherTarget = new Vector3(-1, enemyInterval * enemyCount + 2.5f, 0);
            youngerBrotherPosition.transform.localPosition = new Vector3(-1, 1, 0);
            brotherAnimator.Play("BrotherClimb");
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (m_State == PlayerState.CRUSH || m_State == PlayerState.GETTING || !GameDatas.isPlayerLive ||
           stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowStart") ||
           stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowBrother")) return;
        if (collision.transform.tag == "EnemyBullet")
        {
            Damage();
        }
        //if (collision.transform.tag == "Wall")
        //{
        //    m_Rigidbody.velocity = new Vector3(0, m_Rigidbody.velocity.y, 0);
        //}
    }

}
