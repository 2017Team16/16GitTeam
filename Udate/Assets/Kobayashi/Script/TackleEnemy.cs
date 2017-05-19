using UnityEngine;
using System.Collections;

public class TackleEnemy : EnemyBase
{
    public float chargingTime = 1.0f;
    public float tackleTime = 2.0f;
    public float m_Speed = 1.0f;
    public float searchRange = 5.0f;
    //private ParticleSystem m_Particl;


    void Start()
    {

        m_Rigidbody = GetComponent<Rigidbody>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Player = GameObject.FindGameObjectWithTag("Player");
        SetNewPatrolPointToDestination();
        //m_Particl =transform.FindChild("Charging").GetComponent<ParticleSystem>();

    }
    protected override void WalkingState()
    {
        GetComponent<Renderer>().material.color = Color.blue;
        vec = m_Player.transform.position - transform.position;
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        vec.y = 0;
        m_Agent.Resume();
        if (vec.sqrMagnitude < searchRange)
        {
            vec.Normalize();
            ChangeState(1);
            m_Agent.Stop();
            //m_Particl.Play();
            

        }
    }

    protected override void ChargingState()
    {
        GetComponent<Renderer>().material.color = Color.red;
        if (m_Time >= chargingTime)
        {
            ChangeState(2);
            //m_Particl.Stop();
        }
    }

    protected override void AttackState()
    {
        //m_Rigidbody.velocity = vec * m_Speed;
        //transform.position += vec * m_Speed;
        m_Agent.Move(vec * m_Speed * Time.deltaTime);
        if (m_Time >= tackleTime)
        {
            ChangeState(0);
        }
    }
    protected override void SetNewPatrolPointToDestination()
    {
        m_Agent.destination = new Vector3(Random.Range(-4, 4), 0, Random.Range(-5, 5));
    }
}
