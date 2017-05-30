using UnityEngine;
using System.Collections;

public class PatrolEnemy : EnemyBase
{

    public float m_Speed = 1.0f;
    public float stayTime = 0;
    public Transform[] m_PatrolPoints;

    private int m_CurrentPatrolPointIndex = -1;


    void Start()
    {
        GameObject generator = GameObject.FindGameObjectWithTag("Epoint");
        for (int point = 0; point <= m_PatrolPoints.Length - 1; point++)
        {
            m_PatrolPoints[point] = generator.transform.FindChild("point" + (Random.Range(1, 9)).ToString()).transform;
        }

        m_Rigidbody = GetComponent<Rigidbody>();//Epoint
        m_Agent = GetComponent<NavMeshAgent>();
        m_Player = GameObject.FindGameObjectWithTag("Player");
        SetNewPatrolPointToDestination();

    }

    void Update()
    {
        if (m_Player == null) return;
        m_Time += Time.deltaTime;
        StateUpdate();
    }

    protected override void WalkingState()
    {
        GetComponent<Renderer>().material.color = Color.blue;
        if (HasArrived())
        {
            SetNewPatrolPointToDestination();
            ChangeState(1);
            m_Agent.Stop();
        }
    }

    protected override void ChargingState()
    {
        if(m_Time >= stayTime)
        {
            ChangeState(0);
            m_Time = 0;
            m_Agent.Resume();
        }
    }

    protected override void AttackState()
    {
    }
    protected override void SetNewPatrolPointToDestination()
    {
        //m_Agent.destination = new Vector3(Random.Range(-4, 4), 0, Random.Range(-5, 5));

        m_CurrentPatrolPointIndex
     = (m_CurrentPatrolPointIndex + 1) % m_PatrolPoints.Length;

        m_Agent.destination = m_PatrolPoints[m_CurrentPatrolPointIndex].position;


    }

    bool HasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.position) < 0.5f);
    }
}
