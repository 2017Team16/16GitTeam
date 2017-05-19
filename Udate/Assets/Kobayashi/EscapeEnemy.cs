using UnityEngine;
using System.Collections;

public class EscapeEnemy : EnemyBase
{
    public float chargingTime = 1.0f;
    public float escapeTime = 2.0f;
    public float escapedTime = 1.0f;
    public float m_Speed = 1.0f;
    public float searchRange = 5.0f;
    public int _Counter = 0;
    public int escapedCounter = 0;

    protected override void WalkingState()
    {
        GetComponent<Renderer>().material.color = Color.magenta;
        vec = m_Player.transform.position - transform.position;
        //transform.rotation = Quaternion.Euler(0, 0, 0);
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
            _Counter++;
        }
        else if (_Counter >= escapedCounter)
        {
            ChangeState(6);
        }

    }

    protected override void AttackState()
    {
        //transform.position -= vec * m_Speed;
        m_Agent.Move(-vec * m_Speed * Time.deltaTime);
        if (m_Time >= escapeTime)
        {
            ChangeState(0);
        }
    }
    protected override void WaitState()
    {
        if (m_Time >= escapedTime)
        {
            gameObject.SetActive(false);
        }
    }
    protected override void SetNewPatrolPointToDestination()
    {
        m_Agent.destination = new Vector3(Random.Range(-4, 4), 0, Random.Range(-5, 5));
    }

}
