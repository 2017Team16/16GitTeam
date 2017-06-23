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

        audioSorce = GetComponent<AudioSource>();
        m_Texture = transform.FindChild("EnemyTexture").gameObject;
        m_Scale = m_Texture.transform.localScale;
        reverseScale = new Vector3(m_Scale.x * -1, m_Scale.y, m_Scale.z);
        m_Animator = m_Texture.GetComponent<Animator>();
    }

    void Update()
    {
        if (m_Player == null) return;
        m_Time += Time.deltaTime;
        StateUpdate();
        TextureLR();
    }
    /// <summary>移動中 </summary>
    protected override void WalkingState()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
        if (HasArrived())
        {
            SetNewPatrolPointToDestination();
            ChangeState(1);
            m_Agent.Stop();
        }
    }
    /// <summary>待機中 </summary>
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
    /// <summary>ポイントをセット</summary>
    protected override void SetNewPatrolPointToDestination()
    {
        m_CurrentPatrolPointIndex
     = (m_CurrentPatrolPointIndex + 1) % m_PatrolPoints.Length;

        m_Agent.destination = m_PatrolPoints[m_CurrentPatrolPointIndex].position;
    }

    bool HasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.position) < 0.5f);
    }
}
