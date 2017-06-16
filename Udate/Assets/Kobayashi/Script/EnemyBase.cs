using UnityEngine;
using System.Collections;
using System;

public class EnemyBase : MonoBehaviour
{

    public enum EnemyState
    {
        WALKING,
        CHARGING,
        ATTACK,
        SUTAN,
        GET,
        RECOVERY,
        WAIT
    }

    protected EnemyState m_State = EnemyState.WALKING;
    protected GameObject m_Player;
    protected Vector3 vec = Vector3.zero;
    protected NavMeshAgent m_Agent;
    protected Rigidbody m_Rigidbody;
    protected float m_Time = 0;
    public float sutanTime = 1.0f;
    public float recoveryTime = 1.0f;
    public float score = 10;

    protected AudioSource audioSorce;
    public AudioClip sutan;

    protected GameObject m_Texture; //画像を貼っている子供（仮）
    protected Vector3 m_Scale; //画像の向き、右（仮、子の向き）
    protected Vector3 reverseScale; //画像の向き、左
    protected Animator m_Animator;
    protected bool lVec = false; //左を向くか
    protected float maePosX = 0;
    

    // Use this for initialization
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

    // Update is called once per frame
    void Update()
    {
        if (m_Player == null) return;
        m_Time += Time.deltaTime;

        if (HasArrived())
        {
            SetNewPatrolPointToDestination();
        }
        StateUpdate();
        TextureLR();

    }

    protected virtual void StateUpdate()
    {
        switch (m_State)
        {
            case EnemyState.WALKING: WalkingState(); break;
            case EnemyState.CHARGING: ChargingState(); break;
            case EnemyState.ATTACK: AttackState(); break;
            case EnemyState.SUTAN: SutanState(); break;
            case EnemyState.GET: GetState(); break;
            case EnemyState.RECOVERY: RecoveryState(); break;
            case EnemyState.WAIT: WaitState(); break;
        }
    }

    protected virtual void WaitState()
    {
    }

    protected virtual void WalkingState()
    {
    }

    protected virtual void ChargingState()
    {
    }

    protected virtual void AttackState()
    {
    }

    private void SutanState()
    {
        GetComponent<Renderer>().material.color = Color.black;
        if (m_Time >= sutanTime)
        {
            ChangeState(0);
        }
    }

    private void GetState()
    {
    }

    private void RecoveryState()
    {
        //if (m_Time >= recoveryTime)
        //{
        //    ChangeState(3);
        //}
    }
    protected void ChangeState(int s)
    {
        m_Time = 0.0f;
        switch (s)
        {
            case 0:
                m_State = EnemyState.WALKING;
                m_Animator.Play("Walking");
                m_Agent.enabled = true;
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                break;
            case 1:
                m_State = EnemyState.CHARGING;
                m_Animator.Play("Charge");
                break;
            case 2:
                m_State = EnemyState.ATTACK;
                m_Animator.Play("Attack");
                break;
            case 3:
                m_State = EnemyState.SUTAN;
                m_Animator.Play("Sutan");
                m_Agent.enabled = false;
                audioSorce.PlayOneShot(sutan);
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                transform.rotation = Quaternion.Euler(0, 0, 0); break;
            case 4: m_State = EnemyState.GET; break;
            case 5:
                m_State = EnemyState.RECOVERY;
                m_Animator.Play("Sutan");
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                break;
            case 6: m_State = EnemyState.WAIT; break;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (m_State == EnemyState.RECOVERY && collision.gameObject.tag == "Floor")
        {
            ChangeState(3);
        }
    }
    protected virtual void SetNewPatrolPointToDestination()
    {

    }
    private bool HasArrived()
    {
        return (Vector3.Distance(m_Agent.destination, transform.position) < 2.0f);
    }

    public EnemyState GetEnemyState()
    {
        return m_State;
    }
    public float EnemyScore()
    {
        return score;
    }

    public void Get(int count)
    {
        if(count % 2 == 0)
        {
            m_Animator.Play("Get1");
        }
        else
        {
            m_Animator.Play("Get2");
        }
    }

    protected virtual void TextureLR()
    {

        if (maePosX < transform.position.x)
        {
            m_Texture.transform.localScale = reverseScale;
            lVec = true;
        }
        else if (maePosX > transform.position.x || !lVec)
        {
            m_Texture.transform.localScale = m_Scale;
            lVec = false;
        }

        maePosX = transform.position.x;
    }
}
