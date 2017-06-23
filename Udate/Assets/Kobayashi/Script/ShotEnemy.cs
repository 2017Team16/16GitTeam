using UnityEngine;
using System.Collections;

public class ShotEnemy : EnemyBase
{

    public float chargingTime = 1.0f;
    public float searchRange = 5.0f;
    public float m_ViewingDistance;
    public float m_ViewingAngle;
    public GameObject m_Bullet;
    public Transform muzzle;
    GameObject bullets;
    public float b_Speed = 0;
    public float bulletTime = 0;

    private Transform m_PlayerLookPoint;
    private Transform m_EyePoint;

    public AudioClip shot;

    private float pVec = 0.0f;


    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Player = GameObject.FindGameObjectWithTag("Player");
        SetNewPatrolPointToDestination();
        m_PlayerLookPoint = m_Player.transform.Find("LookPoint");
        m_EyePoint = transform.Find("EyePoint");
        audioSorce = GetComponent<AudioSource>();
        m_Texture = transform.FindChild("EnemyTexture").gameObject;
        m_Scale = m_Texture.transform.localScale;
        reverseScale = new Vector3(m_Scale.x * -1, m_Scale.y, m_Scale.z);
        m_Animator = m_Texture.GetComponent<Animator>();
    }

    /// <summary>プレイヤーを捜索 </summary>
    protected override void WalkingState()
    {
        if (CanSeePlayer())
        {
            if (m_Time >= bulletTime)
            {
                m_Animator.Play("Shot");
                Shot();
                m_Time = 0;
            }
        }
    }

    protected override void ChargingState()
    {
    }

    protected override void AttackState()
    {
    }

    protected override void SetNewPatrolPointToDestination()
    {
    }

    /// <summary>プレイヤーの見える距離 </summary>
    private bool IsPlayerInViewingDistance()
    {
        float distanceToPlayer
            = Vector3.Distance(m_PlayerLookPoint.position, m_EyePoint.position);

        return (distanceToPlayer <= m_ViewingDistance);
    }
    /// <summary>プレイヤーの見える角度 </summary>
    private bool IsPlayerInViewingAngle()
    {
        Vector3 directionToPlayer = m_PlayerLookPoint.position - m_EyePoint.position;

        float angleToPlayer = Vector3.Angle(m_EyePoint.forward, directionToPlayer);

        return (Mathf.Abs(angleToPlayer) <= m_ViewingAngle);
    }
    /// <summary>プレイヤーにレイは当たってるのか </summary>
    private bool CanHitRayToPlayer()
    {
        Vector3 directionToPlayer = m_PlayerLookPoint.position - m_EyePoint.position;

        RaycastHit hitInfo;
        bool hit
            = Physics.Raycast(m_EyePoint.position, directionToPlayer, out hitInfo);

        return (hit && hitInfo.collider.tag == "Player");
    }
    /// <summary>プレイヤーは見えているか </summary>
    private bool CanSeePlayer()
    {
        if (!IsPlayerInViewingDistance())
        {
            return false;
        }
        if (!IsPlayerInViewingAngle())
        {
            return false;
        }
        if (!CanHitRayToPlayer())
        {
            return false;
        }

        return true;
    }
    /// <summary>球の処理 </summary>
    public void Shot()
    {
        Vector3 vec = m_Player.transform.position - transform.position;
        pVec = vec.x;
        vec.Normalize();
        bullets = GameObject.Instantiate(m_Bullet) as GameObject;
        Vector3 force;
        force = vec * b_Speed;
        bullets.GetComponent<Rigidbody>().AddForce(force);
        bullets.transform.position = transform.position + new Vector3(vec.x,0.5f,vec.z) * 1.5f;
        audioSorce.PlayOneShot(shot);
    }
    /// <summary>向いてる方向 </summary>
    protected override void TextureLR()
    {
        if (pVec < 0 || maePosX < transform.position.x)
        {
            m_Texture.transform.localScale = reverseScale;
            lVec = true;
        }
        else if (pVec > 0 || maePosX > transform.position.x || !lVec)
        {
            m_Texture.transform.localScale = m_Scale;
            lVec = false;
        }

        maePosX = transform.position.x;
    }
}
