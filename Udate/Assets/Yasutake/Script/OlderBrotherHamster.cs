using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OlderBrotherHamster : MonoBehaviour
{
    protected enum PlayerState
    {
        WALK,
        CLIMB,
        CRUSH,
        GETTING
    }

    //体力関係
    [Header("体力")]
    public int m_Life = 6;
    protected int[] m_MaxLife = { 6, 8, 10 }; //最大体力の候補
    protected int m_MaxLifeIndex = 0; //いまの最大体力はどれか
    [Header("無敵時間")]
    public float m_InvincibleInterval = 3.0f;
    protected float m_InvincibleTime = 0; //無敵時間をはかる変数

    //速さ関係
    [Header("速さ")]
    public float m_Speed = 1;
    protected bool isSpeedUp = false; //速さのアイテムの効果時間中か
    [Header("アイテムによるデフォルトのスピードになる効果時間")]
    public float m_SpeedUpTime = 10.0f;
    private float m_SpeedUpNowTime;
    [Header("ジャンプ力")]
    public float m_Jump = 3.0f; //デフォルト

    //画像の向き関係
    protected GameObject m_Texture; //画像を貼っている子供（仮）
    protected Vector3 m_Scale; //画像の向き、右（仮、子の向き）
    protected Vector3 reverseScale; //画像の向き、左
    protected bool lVec = false; //左を向くか

    //敵関係
    [Header("持っている敵の間隔")]
    public float enemyInterval = 1.0f;
    protected float enemyIntervalDefault = 0.0f; //敵のデフォルトの間隔
    protected int enemyCount = 0; //持っている敵の数
    protected List<Transform> getenemys = new List<Transform>(); //持っている敵

    //弟関係
    protected GameObject youngerBrotherPosition; //弟の位置
    public GameObject youngerBrother; //弟
    protected BrotherStateManager brotherState; //弟の状態

    //必殺関係
    [SerializeField, Header("必殺ゲージ用の値（確認用）")]
    protected float m_SpecialPoint = 0.0f;
    [Header("必殺の継続時間")]
    public float m_SpecialTime = 5.0f;
    [Header("アイテムによる必殺ゲージ増加量")]
    public float m_SpecialItem = 50.0f;
    [SerializeField, Header("必殺ゲージ用の値 max値")]
    protected float maxSpecial = 0.0f;

    //壁のぼり関係
    protected Vector3 climbStartPoint = Vector3.zero; //壁のぼり開始地点
    protected Vector3 climbEndPoint = Vector3.zero; //壁のぼり終了地点
    protected Vector3 climbEndVector = Vector3.zero; //壁のぼり後、落ちないように進む方向
    [Header("登る速さ")]
    public float climbSpeed = 1.0f;
    protected float climbStartTime; //壁のぼり開始時間
    protected float climbDistance; //登る壁の長さ

    //その他
    protected Rigidbody m_Rigidbody; //リジッドボディ
    protected PlayerState m_State = PlayerState.WALK;  //プレイヤーの状態

    //スコア関係
    [Header("ゲームルール(スコア)")]
    public Score gameScore;
    protected int m_Chain = 0; //ダメージ食らうまでに連続して潰した数
    protected CrushScore m_CrushScore; //敵を倒した時のプレイヤーの近くのスコア

    //サウンド関係
    protected AudioSource m_Audio; //オーディオソース
    [Header("プレイヤーのサウンドたち")]
    public AudioClip[] m_Clips;
    protected float walkSoundPlayInterval = 0.0f; //歩きの音の間隔
    protected bool isJump = false; //ジャンプ中か
    protected int soundNum = 0;  //音の番号
    protected int soundCount = 0;  //敵を倒した時の何回音を鳴らすか

    //アニメ用変数たち
    protected Animator m_Animator; // アニメーター
    protected bool isWithBrother = false;  //弟と一緒か
    protected BrotherState maeBroState = BrotherState.NONE;  //弟の前の状態
    protected AnimatorStateInfo stateInfo;  //アニメーターステイトインフォ 

    protected Animator brotherAnimator; //弟(兄所持時)のアニメーター
    protected AnimatorStateInfo brotherstateInfo; //弟のstateInfo
    protected Vector3 brotherTarget = Vector3.zero; //弟が兄を登るときの目印

    //パーティクル関係
    protected GameObject m_CrushParticle; //敵をつぶした時の煙？エフェクト の親
    protected GameObject m_ChirdJumpParent; //敵を持ち上げるときに、今まで持っていた敵が飛ぶための親
    protected GameObject m_GettingEnemy; //持ち上げる対象
    protected GameObject m_GettingEnemyParent; //持ち上げるときに、対象を移動させるアニメをする親

    [Header("魂のプレハブ")]
    public GameObject mySoul;
    [Header("汗のプレハブ")]
    public GameObject mySweat;
    protected float sewatTime = 0.0f; //汗を出す間隔


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
        m_SpeedUpNowTime = 0.0f;

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
        if (transform.position.y <= -5.0f) //もしステージから落下した場合
        {
            transform.position = Vector3.up;
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
            Invincible();
        }

        SpecialGage();
        if (m_State == PlayerState.CRUSH || m_State == PlayerState.GETTING) return;

        if (soundCount > 0)
        {
            m_Audio.PlayOneShot(m_Clips[soundNum]);
            soundCount--;
        }

        BrotherGet();

        maeBroState = brotherState.GetState();

        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowBrother"))
        {
            float duration = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (duration > 0.9f) //再生終わりまで行ったら歩きなどのアニメへ
            {
                WalkAnimeControl();
            }
        }
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerJumpPoseSolo") && m_Rigidbody.velocity.y <= 0 ||
            stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerJumpPoseWithBrother") && m_Rigidbody.velocity.y <= 0)
        {
            if (IsGround()) //ジャンプ中地面についたら歩きへ
            {
                WalkAnimeControl();
                isJump = false;
            }
        }

        if (!GameDatas.isSpecialAttack && GetSpeedUpTime() == 0)
        {
            Sweat();
        }
    }

    /// <summary>プレイヤーの移動</summary>
    protected void Move()
    {
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerClimbX"))
        {
            WalkAnimeControl();
        }
        Jump();

        Walk();
        TextureLR();

        float f = Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));
        m_Animator.SetFloat("speed", f);
        if (f > 0.5f && !isJump)
        {
            walkSoundPlayInterval += Time.deltaTime;
            walkSoundPlayInterval *= (GameDatas.isSpecialAttack) ? 1.5f : (GetSpeedUpTime() == 0) ? 1 : 1.1f;
            if (walkSoundPlayInterval > 0.4f)
            {
                m_Audio.PlayOneShot(m_Clips[0]);
                walkSoundPlayInterval = 0.0f;
            }
        }
    }

    /// <summary>歩き　持つときも歩くから関数に</summary>
    private void Walk()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 move = (enemyCount > 3) ? input / (enemyCount + 1) : input / (enemyCount * 0.2f + 1);
        if (GameDatas.isSpecialAttack)
        {
            move = 1.5f * input;
        }
        else if (isSpeedUp)
        {
            move = input * 1.1f;
            m_SpeedUpNowTime += Time.deltaTime;
            if (m_SpeedUpTime < m_SpeedUpNowTime)
            {
                isSpeedUp = false;
                m_SpeedUpNowTime = 0.0f;
            }
        }
        m_Rigidbody.velocity = new Vector3(move.x * m_Speed, m_Rigidbody.velocity.y, move.z * m_Speed);
    }

    /// <summary>スピードアップ中の時間</summary>
    public float GetSpeedUpTime()
    {
        return (m_SpeedUpNowTime == 0.0f) ? 0.0f : (1 - (m_SpeedUpNowTime / 10.0f));
    }

    /// <summary>画像をどっち向きにするか</summary>
    private void TextureLR()
    {
        if (Time.timeScale == 0) return;
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowBrother") ||
            stateInfo.fullPathHash == Animator.StringToHash("Base Layer.PlayerThrowStart")) return;
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

    public bool GetPlayerLR()
    {
        return lVec;
    }

    /// <summary>ジャンプ</summary>
    protected void Jump()
    {
        Vector3 newVelocity = m_Rigidbody.velocity;
        if (IsGround()
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
    }

    /// <summary>兄が地面についているかどうか</summary>
    protected bool IsGround() //正確に言えば足元に何かあるかどうか　敵の種でもtrue
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 1.0f);
    }

    /// <summary>壁のぼり</summary>
    protected void Climb()
    {
        float speed;
        speed = (GameDatas.isSpecialAttack) ? climbSpeed * 1.5f : climbSpeed;

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
    protected virtual void BrotherGet()
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
                        if (enemyCount == 0)
                        {
                            Debug.Log("敵を持っていないよ");
                            return;
                        }
                        //EnemyKill();
                        m_State = PlayerState.CRUSH;
                        brotherAnimator.Play("BrotherCrushStart");
                    }
                    if (Input.GetButtonDown("XboxR1") && m_SpecialPoint >= 100.0f)
                    {
                        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
                        float noraEnemycount = enemys.Length - enemyCount;
                        if (noraEnemycount == 0) return;

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
            if (!isWithBrother) //弟が兄を登っているときに投げられそうになった場合
            {
                isWithBrother = true;
                brotherAnimator.Play("BrotherWait");
                WalkAnimeControl();
            }

            if (enemyCount != 0) youngerBrotherPosition.transform.localPosition = new Vector3(0, enemyInterval * enemyCount + 0.3f + 2.5f, 0);
            else youngerBrotherPosition.transform.localPosition = new Vector3(0, enemyInterval * enemyCount + 2.5f, 0);

            GetComponent<CapsuleCollider>().height = 2 + enemyInterval * enemyCount + 1; //兄の分＋敵の分＋弟の分のあたり判定
            GetComponent<CapsuleCollider>().center = new Vector3(0, GetComponent<CapsuleCollider>().height / 2, 0);
            if (Time.timeScale != 0 && Input.GetButtonDown("XboxR1") && m_SpecialPoint >= 100.0f &&
                stateInfo.fullPathHash != Animator.StringToHash("Base Layer.PlayerThrowStart"))
            {
                GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
                float noraEnemycount = enemys.Length - enemyCount;
                if (noraEnemycount == 0) return;

                SpecialAttack();
            }
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
        else if (GameDatas.isBrotherFlying || brotherState.GetState() == BrotherState.SPECIAL)
        {
            //youngerBrother.GetComponent<MeshRenderer>().enabled = true;
            youngerBrotherPosition.GetComponent<SpriteRenderer>().enabled = false;
            youngerBrother.GetComponent<Collider>().enabled = true;
            GetComponent<CapsuleCollider>().height = 2 + enemyInterval * enemyCount;
            GetComponent<CapsuleCollider>().center = new Vector3(0, GetComponent<CapsuleCollider>().height / 2, 0);
        }
    }

    /// <summary>必殺技</summary>
    protected void SpecialAttack()
    {
        GameDatas.isSpecialAttack = true;
        youngerBrother.SendMessage("Special", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>必殺ゲージ</summary>
    protected void SpecialGage()
    {
        if (GameDatas.isSpecialAttack) //必殺技の効果時間
        {
            m_SpecialPoint -= 100.0f / m_SpecialTime * Time.deltaTime;
            if (m_SpecialPoint <= 0.0f)
            {
                m_SpecialPoint = 0.0f;
                maxSpecial = 0.0f;
                GameDatas.isSpecialAttack = false;
            }
        }
        else
        {
            if (m_SpecialPoint < maxSpecial)
            {
                m_SpecialPoint += 100.0f / m_SpecialTime * Time.deltaTime;
                if (m_SpecialPoint >= maxSpecial)
                {
                    m_SpecialPoint = maxSpecial;
                }
            }
            if (m_SpecialPoint == maxSpecial)
            {
                AddSpecialPoint(100.0f / 60.0f * Time.deltaTime);
            }
        }

    }

    /// <summary>敵をつぶす演出</summary>
    protected void Crush()
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
    protected void Getting()
    {
        Walk();

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

    /// <summary>敵をつぶす</summary>
    protected virtual void EnemyKill()
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
                    AddSpecialPoint(10.0f);
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
    protected virtual void Damage()
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

    /// <summary>無敵中の処理</summary>
    protected void Invincible()
    {
        if ((int)(m_InvincibleTime * 10) % 2 == 0)
        {
            m_Texture.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            m_Texture.GetComponent<SpriteRenderer>().color = Color.white;
        }
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

    /// <summary>体力の増減</summary>
    /// <param name="n">足す数値</param>
    public virtual void AddLife(int n)
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
    /// <param name="n">足す数値</param>
    public void AddSpecialPoint(float n)
    {
        maxSpecial += n;
        if (maxSpecial > 100.0f) maxSpecial = 100.0f;
    }

    /// <summary>デフォルトの速さで歩く</summary>
    public void DefaultSpeedWalk()
    {
        isSpeedUp = true;
    }

    /// <summary>必殺ゲージ用float型を返す</summary>
    public float GetSpacialPoint()
    {
        return m_SpecialPoint / 100.0f;
    }

    /// <summary>歩きだすときに、どのアニメを再生するか</summary>
    protected void WalkAnimeControl()
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
    protected void ThrowAnimeControl()
    {
        if (enemyCount == 0 && m_State != PlayerState.CLIMB)
        {
            m_Audio.PlayOneShot(m_Clips[1]);
            m_Animator.Play("PlayerThrowStart");

            GameObject brotherTarget = GameObject.Find("Target(Clone)");

            if (m_Texture.transform.position.x > brotherTarget.transform.position.x)
            {
                m_Texture.transform.localScale = reverseScale;
                youngerBrotherPosition.transform.localScale = reverseScale;
                lVec = true;
            }
            else
            {
                m_Texture.transform.localScale = m_Scale;
                youngerBrotherPosition.transform.localScale = m_Scale;
                lVec = false;
            }
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
    protected void Sweat()
    {
        sewatTime += Time.deltaTime;
        if (enemyCount >= 4 && sewatTime > 0.3f)
        {
            GameObject sweat = Instantiate(mySweat);
            sweat.transform.parent = transform;
            sewatTime = 0.0f;
        }
    }

    public virtual void OnTriggerStay(Collider other)
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
        if (collision.gameObject == youngerBrother && !isWithBrother)
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
