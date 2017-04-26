using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        WALKING,
        CHARGING,
        TACKLE,
        SUTAN,
        GET,
        RECOVERY
    }
    private EnemyState m_State = EnemyState.WALKING;
    private GameObject m_Player;
    private Vector3 vec = Vector3.zero;
    private float m_Time = 0.0f;
    private NavMeshAgent m_Agent;
    private Rigidbody m_Rigidbody;
    private Renderer m_Renderer;
    private int m_CurrentPatrolPointIndex = -1;
    public float chargingTime = 1.0f;
    public float tackleTime = 2.0f;
    public float recoveryTime = 1.0f;
    public float sutanTime = 1.0f;
    public float m_Speed = 1.0f;
    public float searchRange = 5.0f;
    //プレイヤー参照
    // Use this for initialization
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Rigidbody = GetComponent<Rigidbody>();

        SetNewPatrolPointToDestination();

        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Player == null) return;
        m_Time += Time.deltaTime;
        StateUpdate();
        if (HasArrived())
        {
            SetNewPatrolPointToDestination();
        }
    }

    private void StateUpdate()
    {
        switch (m_State)
        {
            case EnemyState.WALKING: WalkingState(); break;
            case EnemyState.CHARGING: ChargingState(); break;
            case EnemyState.TACKLE: TackleState(); break;
            case EnemyState.SUTAN: SutanState(); break;
            case EnemyState.GET: GetState(); break;
            case EnemyState.RECOVERY: RecoveryState(); break;
        }
    }

    private void WalkingState()
    {
        m_Renderer.material.color = Color.blue;
        vec = m_Player.transform.position - transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        vec.y = 0;
        m_Agent.enabled = true;
        if (vec.sqrMagnitude < searchRange)
        {
            vec.Normalize();
            ChangeState(1);
            m_Agent.enabled = false;

        }
    }

    private void ChargingState()
    {
        m_Renderer.material.color = Color.red;
        if (m_Time >= chargingTime)
        {
            ChangeState(2);
        }
    }

    private void TackleState()
    {
        transform.position += vec * m_Speed;
        if (m_Time >= tackleTime)
        {
            ChangeState(0);
        }
    }

    private void SutanState()
    {
        m_Renderer.material.color = Color.black;
        m_Agent.enabled = false;
        if (m_Time >= sutanTime)
        {
            ChangeState(0);
        }

    }

    private void GetState()
    {
        m_Renderer.material.color = Color.black;
    }

    private void RecoveryState()
    {
        //if (m_Time >= recoveryTime)
        //{
        //    ChangeState(3);
        //}
    }

    public void ChangeState(int s)
    {
        m_Time = 0.0f;
        switch (s)
        {
            case 0: m_State = EnemyState.WALKING;
                GetComponent<Renderer>().material.color = Color.blue;
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll; break;
            case 1: m_State = EnemyState.CHARGING; break;
            case 2: m_State = EnemyState.TACKLE; break;
            case 3: m_State = EnemyState.SUTAN;
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                transform.rotation = Quaternion.Euler(0, 0, 0); break;
            case 4: m_State = EnemyState.GET; break;
            case 5: m_State = EnemyState.RECOVERY;
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY; break;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(m_State == EnemyState.RECOVERY && collision.transform.tag == "Floor")
        {
            ChangeState(3);
        }
    }

    private void SetNewPatrolPointToDestination()
    {
        m_Agent.destination = new Vector3(Random.Range(-3, 3), 0, Random.Range(-5, 5));

    }
    private bool HasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.position) < 2.0f);
    }

    public EnemyState GetEnemyState()
    {
        return m_State;
    }
}
