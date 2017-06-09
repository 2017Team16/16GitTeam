using UnityEngine;
using System.Collections;

public class BrotherBack : MonoBehaviour
{
    [SerializeField, TooltipAttribute("プレイヤーオブジェクト")]
    private GameObject Player;

    //移動速度
    public float _speed = 5;

    //public bool _isBack = false;
    //public bool _isMove = false;

    NavMeshAgent m_Nav;

    //弟管理クラス
    private BrotherStateManager m_BrotherStateManager;

    void Start()
    {
        m_Nav = GetComponent<NavMeshAgent>();

        m_BrotherStateManager = GetComponent<BrotherStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_BrotherStateManager.GetState() == BrotherState.BACK)
        {
            m_Nav.destination = Player.transform.position;

            

            //if (_isBack)
            //    m_BrotherStateManager.SetState(BrotherState.NORMAL);


        }
    }

    public void Move()
    { 
        StartCoroutine(GoBack());
    }

    IEnumerator GoBack()
    {
        //m_Nav.autoTraverseOffMeshLink = false; // OffMeshLinkによる移動を禁止

        GetComponent<NavMeshAgent>().enabled = true;

        yield return new WaitForSeconds(0.1f);

        transform.LookAt(Player.transform.position);
        GetComponent<AnimationControl>().m_Anim.SetTrigger("back");

        while (m_BrotherStateManager.GetState() == BrotherState.BACK)
        {
            // OffmeshLinkに乗るまで普通に移動
            //yield return new WaitWhile(() => m_Nav.isOnOffMeshLink == false);

            m_Nav.speed = 6;

            while (m_Nav.isOnOffMeshLink == true)
            {
                m_Nav.speed = 2;

                yield return null;
            }

            //    // OffMeshLinkに乗ったので、NavmeshAgentによる移動を止めて、
            //    // OffMeshLinkの終わりまでNavmeshAgent.speedと同じ速度で移動
            //    m_Nav.Stop();

            //    while (Vector3.Distance(transform.position, new Vector3(m_Nav.currentOffMeshLinkData.endPos.x, transform.position.y, m_Nav.currentOffMeshLinkData.endPos.z)) > 0.1f)
            //    {
            //        transform.position =
            //            Vector3.MoveTowards(
            //                transform.position,
            //                new Vector3(m_Nav.currentOffMeshLinkData.endPos.x, transform.position.y, m_Nav.currentOffMeshLinkData.endPos.z),
            //                m_Nav.speed * Time.deltaTime);

            //        yield return null;
            //    }
            //    //{
            //    //    transform.position = Vector3.MoveTowards(
            //    //                                transform.position,
            //    //                                m_Nav.currentOffMeshLinkData.endPos, m_Nav.speed * Time.deltaTime);
            //    //    return Vector3.Distance(transform.position, m_Nav.currentOffMeshLinkData.endPos) > 0.1f;
            //    //});
            //    transform.LookAt(new Vector3(m_Nav.currentOffMeshLinkData.endPos.x, transform.position.y, m_Nav.currentOffMeshLinkData.endPos.z));
            //    // NavmeshAgentを到達した事にして、Navmeshを再開
            //    m_Nav.CompleteOffMeshLink();
            //    m_Nav.Resume();

            //    yield break;
            yield return null;
        }
        yield return null;
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == ("Player") && m_BrotherStateManager.GetState() == BrotherState.BACK)
        {
            m_BrotherStateManager.SetState(BrotherState.NORMAL);
        }
    }
}
