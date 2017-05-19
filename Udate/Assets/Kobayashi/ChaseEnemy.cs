using UnityEngine;
using System.Collections;

public class ChaseEnemy : EnemyBase {

    public float m_Speed = 0f;

    protected override void WalkingState()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
        vec = m_Player.transform.position - transform.position;
       // transform.rotation = Quaternion.Euler(0, 0, 0);
        vec.y = 0;
        vec.Normalize();
        m_Agent.Move(vec * m_Speed * Time.deltaTime);
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
       // m_Agent.destination = new Vector3(Random.Range(-4, 4), 0, Random.Range(-5, 5));
    }
}
