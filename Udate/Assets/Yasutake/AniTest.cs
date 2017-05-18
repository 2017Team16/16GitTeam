using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AniTest : MonoBehaviour {

    
    [Header("速さ")]
    public float m_Speed = 1;
    [Header("ジャンプ力")]
    public float m_Jump = 300.0f;
    private float jumpVector = 0.0f;
    private Vector3 maenoVector = Vector3.zero;
    private NavMeshAgent m_Agent;
    private CharacterController m_Controller;

    // Use this for initialization
    void Start()
    {
        
        m_Agent = GetComponent<NavMeshAgent>();
        m_Controller = GetComponent<CharacterController>();

        GameDatas.isPlayerLive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Agent.updatePosition)
        {
            Move();
        }
        else
        {
            Jump();
        }
    }

    /// <summary>プレイヤーの移動</summary>
    private void Move()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal") * m_Speed, 0, Input.GetAxis("Vertical") * m_Speed);
        m_Agent.destination = transform.position + move;


        if (Input.GetKeyDown(KeyCode.F))  //仮
        {
            NavMeshHit navHit = new NavMeshHit();
            m_Agent.updatePosition = false;
            NavMesh.SamplePosition(m_Agent.transform.localPosition, out navHit, 1.5f, NavMesh.AllAreas);
            transform.localPosition = navHit.position + new Vector3(0, transform.localPosition.y, 0);
            jumpVector = m_Jump;
        }
    }

    private void Jump()
    {
        jumpVector -= 10 * Time.deltaTime;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), jumpVector, Input.GetAxis("Vertical")) * m_Speed * Time.deltaTime;
        //Quaternion q = Quaternion.LookRotation(new Vector3(move.x,0,move.z));
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 360.0f);
        m_Controller.Move(move);
        if (m_Controller.isGrounded)
        {
            maenoVector = move;
            hukki();
        }
    }

    private void hukki()
    {
        NavMeshHit navHit = new NavMeshHit();
        NavMesh.SamplePosition(m_Agent.transform.localPosition, out navHit, 1.5f, NavMesh.AllAreas);
        m_Agent.Resume();
        m_Agent.Warp(navHit.position);
        m_Agent.updatePosition = true;
        m_Agent.destination = transform.position + maenoVector;
    }
}
