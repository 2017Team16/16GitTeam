using UnityEngine;
using System.Collections;

public class TutorialEnemy : EnemyBase
{

    public float m_Speed = 1.0f;


    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
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
    }
    /// <summary>待機中 </summary>
    protected override void ChargingState()
    {
    }
    protected override void AttackState()
    {
    }
    /// <summary>ポイントをセット</summary>
    protected override void SetNewPatrolPointToDestination()
    {
        m_Agent.destination = transform.position;
    }
    
}

