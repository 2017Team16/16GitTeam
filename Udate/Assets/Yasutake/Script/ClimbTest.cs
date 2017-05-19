using UnityEngine;
using System.Collections;

public class ClimbTest : MonoBehaviour
{

    enum TestState
    {
        WALK,
        CLIMB
    }

    private CharacterController m_Controller;
    private TestState m_State = TestState.WALK;
    private float m_VectorY = 0.0f;
    private Vector3 climbStartPoint = Vector3.zero;
    private Vector3 climbEndPoint = Vector3.zero;
    private Vector3 climbEndVector = Vector3.zero;
    public float climbSpeed = 1.0f;
    private float climbStartTime;
    private float climbDistance;


    // Use this for initialization
    void Start()
    {
        m_Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case TestState.WALK: Move(); break;
            case TestState.CLIMB: Climb(); break;
        }
    }

    private void Move()
    {
        if (!m_Controller.isGrounded)
        {
            m_VectorY -= 10 * Time.deltaTime;
        }
        else
        {
            m_VectorY = 0.0f;
            if (Input.GetKeyDown(KeyCode.F))
            {
                m_VectorY = 3.0f;
            }
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), m_VectorY, Input.GetAxis("Vertical")) * 10.0f * Time.deltaTime;

        m_Controller.Move(move);
    }

    private void Climb()
    {
        float elapsedTime = (Time.time - climbStartTime) * climbSpeed;
        float nowPoint = elapsedTime / climbDistance;
        transform.position = Vector3.Lerp(climbStartPoint, climbEndPoint, nowPoint);

        if (Vector3.Distance(transform.position, climbEndPoint) < 0.1f)
        {
            m_Controller.Move(climbEndVector);
            m_State = TestState.WALK;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Damage();
        }

    }

    private void Damage()
    {
        //transform.position = climbStartPoint;
        m_State = TestState.WALK;
    }

    private void ClimbPreparation(float y)
    {
        climbStartPoint = transform.position;
        climbEndPoint = new Vector3(transform.position.x, y, transform.position.z);
        m_State = TestState.CLIMB;
        climbStartTime = Time.time;
        climbDistance = Vector3.Distance(climbStartPoint, climbEndPoint);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "FrontClimbStart" && Input.GetAxis("Vertical") > 0.0f)
        {
            Transform end = other.transform.FindChild("ClimbEnd");
            if (end == null) return; ClimbPreparation(end.position.y);
             climbEndVector = Vector3.forward;
        }
        if (other.transform.tag == "LeftClimbStart" && Input.GetAxis("Horizontal") > 0.0f)
        {
            Transform end = other.transform.FindChild("ClimbEnd");
            if (end == null) return; ClimbPreparation(end.position.y);
            climbEndVector = Vector3.right;
        }
        if (other.transform.tag == "RightClimbStart" && Input.GetAxis("Horizontal") < 0.0f)
        {
            Transform end = other.transform.FindChild("ClimbEnd");
            if (end == null) return; ClimbPreparation(end.position.y);
            climbEndVector = Vector3.left;
        }
    }
}
