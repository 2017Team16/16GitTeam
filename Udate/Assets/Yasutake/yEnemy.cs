using UnityEngine;
using System.Collections;

public class yEnemy : MonoBehaviour {

    public enum yEnemyState
    {
        WALKING,
        CHARGING,
        TACKLE,
        SUTAN,
        GET,
        RECOVERY
    }

    public yEnemyState m_State = yEnemyState.WALKING;
    private GameObject player;
    private Vector3 vec = Vector3.zero;
    public float chargingTime = 1.0f;
    public float tackleTime = 2.0f;
    public float m_Speed = 1.0f;
    public float sutanTime = 3.0f;
    public float recoveryTime = 2.0f;
    private float m_Time = 0.0f;

    public float searchRange = 5.0f;


	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null) return;
        m_Time += Time.deltaTime;
        StateUpdate();
	}

    private void StateUpdate()
    {
        switch (m_State)
        {
            case yEnemyState.WALKING: WalkingState();break;
            case yEnemyState.CHARGING: ChargingState(); break;
            case yEnemyState.TACKLE: TackleState(); break;
            case yEnemyState.SUTAN: SutanState(); break;
            case yEnemyState.GET: GetState(); break;
            case yEnemyState.RECOVERY: RecoveryState(); break;
        }
    }

    private void WalkingState()
    {
        vec = player.transform.position - transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (vec.sqrMagnitude < searchRange)
        {
            vec.Normalize();
            ChangeState(1);
        }
    }

    private void ChargingState()
    {
        if(m_Time >= chargingTime)
        {
            ChangeState(2);
        }
    }

    private void TackleState()
    {
        transform.position += vec * m_Speed;
        if(m_Time >= tackleTime)
        {
            ChangeState(0);
        }
    }

    private void SutanState()
    {
        if(m_Time >= sutanTime)
        {
            ChangeState(0);
        }
    }

    private void GetState()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    private void RecoveryState()
    {
        if(m_Time >= recoveryTime)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            ChangeState(3);
        }
    }

    public void ChangeState(int s)
    {
        m_Time = 0.0f;
        switch (s)
        {
            case 0: m_State = yEnemyState.WALKING;break;
            case 1: m_State = yEnemyState.CHARGING; break;
            case 2: m_State = yEnemyState.TACKLE; break;
            case 3: m_State = yEnemyState.SUTAN; break;
            case 4: m_State = yEnemyState.GET; break;
            case 5: m_State = yEnemyState.RECOVERY; break;
        }
    }

    public yEnemyState GetEnemyState()
    {
        return m_State;
    }
}
