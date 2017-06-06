using UnityEngine;
using System.Collections;

public class ChaseEnemy : EnemyBase
{

    public float m_Speed = 0f;

    protected override void WalkingState()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
        SetNewPatrolPointToDestination();
        if (m_Agent.isOnOffMeshLink == true)
        {
            m_Agent.Stop();
            ChangeState(1);
        }
    }
    protected override void ChargingState()
    {
        transform.localPosition = Vector3.MoveTowards(
                                            transform.localPosition,
                                            m_Agent.currentOffMeshLinkData.endPos, m_Agent.speed * Time.deltaTime);
        if (Vector3.Distance(transform.localPosition, m_Agent.currentOffMeshLinkData.endPos) < 0.1f)
        {
            m_Agent.CompleteOffMeshLink();
            m_Agent.Resume();
            ChangeState(0);
        }
    }

    protected override void AttackState()
    {
    }
    protected override void WaitState()
    {
    }
    protected override void SetNewPatrolPointToDestination()
    {
        m_Agent.destination = m_Player.transform.position;
    }
}
