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

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Player = GameObject.FindGameObjectWithTag("Player");
        SetNewPatrolPointToDestination();
        m_PlayerLookPoint = m_Player.transform.Find("LookPoint");
        m_EyePoint = transform.Find("EyePoint");
    }


    protected override void WalkingState()
    {
        if (CanSeePlayer())
        {
            print("みえてる");

            if (m_Time >= bulletTime)
            {
                Shot();
                m_Time = 0;
            }

            //transform.LookAt(m_PlayerLookPoint);
        }
        else
        {
            print("見えてない");
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
    private bool IsPlayerInViewingDistance()
    {
        float distanceToPlayer
            = Vector3.Distance(m_PlayerLookPoint.position, m_EyePoint.position);

        return (distanceToPlayer <= m_ViewingDistance);
    }
    private bool IsPlayerInViewingAngle()
    {
        Vector3 directionToPlayer = m_PlayerLookPoint.position - m_EyePoint.position;

        float angleToPlayer = Vector3.Angle(m_EyePoint.forward, directionToPlayer);

        return (Mathf.Abs(angleToPlayer) <= m_ViewingAngle);
    }
    private bool CanHitRayToPlayer()
    {
        Vector3 directionToPlayer = m_PlayerLookPoint.position - m_EyePoint.position;

        RaycastHit hitInfo;
        bool hit
            = Physics.Raycast(m_EyePoint.position, directionToPlayer, out hitInfo);

        return (hit && hitInfo.collider.tag == "Player");
    }
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
    public void Shot()
    {
        Vector3 vec = m_Player.transform.position - transform.position;
        vec.Normalize();
        bullets = GameObject.Instantiate(m_Bullet) as GameObject;
        Vector3 force;
        force = vec * b_Speed;
        bullets.GetComponent<Rigidbody>().AddForce(force);
        bullets.transform.position = muzzle.position;
    }
}
