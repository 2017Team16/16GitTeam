using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TutorialAnitest : OlderBrotherHamster
{

    public TutorialManager tManager;
    private float a;
    public Image sorry;
    public Text mainscore;
    private int sumscore = 0;

    // Use this for initialization
    void Start () {
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

        a = 0;
        sorry.color = new Color(1, 1, 1, a);
    }
	
	// Update is called once per frame
	void Update () {
        if (tManager.GetTutorialNumber() == 28)
        {
            WalkAnimeControl();
            return;
        }
        if (a < 0) a = 0;
        sorry.color = new Color(1, 1, 1, a);
        a -= Time.deltaTime;

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

        Sweat();
    }
    
    protected override void BrotherGet()
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
                        if (!IsCrush())
                        {
                            a = 1;
                            return;
                        }
                        //EnemyKill();
                        m_State = PlayerState.CRUSH;
                        brotherAnimator.Play("BrotherCrushStart");
                    }
                    if (Input.GetButtonDown("XboxR1") && m_SpecialPoint >= 100.0f)
                    {
                        if (!IsSpecial())
                        {
                            a = 1;
                            return;
                        }

                        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
                        if (enemys.Length == 0) return;

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
            if (Input.GetButtonDown("XboxR1") && m_SpecialPoint >= 100.0f)
            {
                GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
                if (enemys.Length == 0) return;

                SpecialAttack();
            }
            if (Input.GetButtonDown("XboxL1") && Time.timeScale != 0)
            {
                if (!IsThrow())
                {
                    a = 1;
                    return;
                }
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

    protected override void EnemyKill()
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
        sumscore += score * enemyCount;
        mainscore.text = sumscore.ToString();
        m_CrushScore.SetNumbers(score * enemyCount);
        enemyCount = 0;
    }

    protected override void Damage()
    {
        if (!IsDamage()) return;
        base.Damage();
    }

    public override void AddLife(int n)
    {
        m_Life += n;
        m_Life = Mathf.Clamp(m_Life, 1, m_MaxLife[m_MaxLifeIndex]);
    }

    private bool IsClimb()
    {
        if (tManager.GetTutorialNumber() >= 17) return true;
        else return false;
    }

    private bool IsCrush()
    {
        if (tManager.GetTutorialNumber() >= 19 && tManager.GetTutorialNumber() <= 21 ||
            tManager.GetTutorialNumber() < 12) return false;
        else if (tManager.GetTutorialNumber() == 22 && enemyCount < 4) return false;
        else return true;
    }

    private bool IsThrow()
    {
        if (tManager.GetTutorialNumber() <= 2 || tManager.GetTutorialNumber() == 8 ||
            tManager.GetTutorialNumber() == 9 || tManager.GetTutorialNumber() == 10) return false;
        else return true;
    }

    private bool IsDamage()
    {
        if (tManager.GetTutorialNumber() == 5) return false;
        else return true;
    }

    private bool IsSpecial()
    {
        if (tManager.GetTutorialNumber() >= 10) return true;
        else return false;
    }
    public int GetEnemyCount()
    {
        return enemyCount;
    }

    public override void OnTriggerStay(Collider other)
    {
        if (!IsClimb()) return;
        base.OnTriggerStay(other);
    }
}
