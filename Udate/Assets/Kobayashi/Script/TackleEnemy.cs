using UnityEngine;
using System.Collections;

public class TackleEnemy : EnemyBase
{
    public float chargingTime = 1.0f;
    public float tackleTime = 2.0f;
    public float m_Speed = 1.0f;
    public float searchRange = 5.0f;
    public AudioClip dash;


    protected override void WalkingState()
    {
        GetComponent<Renderer>().material.color = Color.blue;
        vec = m_Player.transform.position - transform.position;
        vec.y = 0;
        m_Agent.Resume();
        if (vec.sqrMagnitude < searchRange)
        {
            vec.Normalize();
            ChangeState(1);
            m_Agent.Stop();
            

        }
    }

    protected override void ChargingState()
    {
        GetComponent<Renderer>().material.color = Color.red;
        if (m_Time >= chargingTime)
        {
            ChangeState(2);
            audioSorce.PlayOneShot(dash);
        }
    }

    protected override void AttackState()
    {
       m_Agent.Move(vec * m_Speed * Time.deltaTime);
        if (m_Time >= tackleTime)
        {
            ChangeState(0);
        }
    }
    protected override void SetNewPatrolPointToDestination()
    {
        m_Agent.destination = new Vector3(Random.Range(-7, 7), 0, Random.Range(-7, 7));
    }
}
