using UnityEngine;
using System.Collections;

public class ChaseEnemy : EnemyBase {

    public float m_Speed = 0f;

    protected override void WalkingState()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
        SetNewPatrolPointToDestination();
    }
    protected override void ChargingState()
    {
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
