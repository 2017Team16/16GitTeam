using UnityEngine;
using System.Collections;

public class BrotherBack : MonoBehaviour
{
    /*------外部設定------*/
    [SerializeField, Header("プレイヤーオブジェクト(シーンから)")]
    private GameObject Player;

    [SerializeField, Header("小屋用ポイント(ステージ2のみシーンから2つ)")]
    private Transform[] HousePoints;

    [SerializeField, Header("戻る速度")]
    private float _speed = 5;


    /*------内部設定------*/
    [Header("ナビメッシュ")]
    private NavMeshAgent m_Nav;

    [Header("サウンド")]
    private AudioSource m_Audio;

    //弟管理クラス
    private BrotherStateManager m_BrotherStateManager;

    void Awake()
    {
        m_Nav = GetComponent<NavMeshAgent>();

        m_BrotherStateManager = GetComponent<BrotherStateManager>();

        m_Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_BrotherStateManager.GetState() == BrotherState.BACK)
        {
            m_Nav.destination = Player.transform.position;
        }
    }

    public void Move()
    { 
        StartCoroutine(GoBack());
    }

    IEnumerator GoBack()
    {
        GetComponent<NavMeshAgent>().enabled = true;

        yield return new WaitForSeconds(0.1f);

        transform.LookAt(Player.transform.position);
        GetComponent<AnimationControl>().m_Anim.SetTrigger("back");

        m_Nav.speed = _speed;

        float walkTime = 0;
        while (m_BrotherStateManager.GetState() == BrotherState.BACK)
        {
            walkTime += Time.deltaTime;

            if (m_Nav.isOnOffMeshLink)
            {
                m_Nav.Stop();

                if (HousePoints.Length != 0 &&
                    (m_Nav.currentOffMeshLinkData.endPos == HousePoints[0].position ||
                    m_Nav.currentOffMeshLinkData.endPos == HousePoints[1].position))
                {
                    transform.localPosition = Vector3.MoveTowards(
                                                    transform.localPosition,
                                                    m_Nav.currentOffMeshLinkData.endPos,
                                                    (m_Nav.speed * 2) * Time.deltaTime);
                }
                else
                {
                    if (m_Nav.currentOffMeshLinkData.endPos.y > transform.localPosition.y)
                    {
                        if (Mathf.Abs(m_Nav.currentOffMeshLinkData.endPos.z - transform.position.z) > 1.0f)
                            GetComponent<AnimationControl>().m_Anim.SetBool("climb", true);
                        else if (Mathf.Abs(m_Nav.currentOffMeshLinkData.endPos.x - transform.position.x) > 1.0f)
                            GetComponent<AnimationControl>().m_Anim.SetBool("climbSide", true);

                        transform.localPosition = Vector3.MoveTowards(
                                                    transform.localPosition,
                                                    new Vector3(transform.localPosition.x, m_Nav.currentOffMeshLinkData.endPos.y, transform.localPosition.z),
                                                    (m_Nav.speed * 2) * Time.deltaTime);
                    }
                    else
                    {
                        GetComponent<AnimationControl>().isClimb = true;

                        if (Mathf.Abs(m_Nav.currentOffMeshLinkData.endPos.z - transform.position.z) > 1.0f)
                            GetComponent<AnimationControl>().m_Anim.SetBool("climb", true);
                        else if (Mathf.Abs(m_Nav.currentOffMeshLinkData.endPos.x - transform.position.x) > 1.0f)
                        {
                            GetComponent<AnimationControl>().m_Anim.SetBool("climbSide", true);
                            if (m_Nav.currentOffMeshLinkData.endPos.x < transform.position.x)
                            {
                                GetComponent<AnimationControl>().m_BrosAnimation.transform.localScale
                            = new Vector3(-GetComponent<AnimationControl>().m_BrosAnimation.transform.localScale.x, GetComponent<AnimationControl>().m_BrosAnimation.transform.localScale.y, GetComponent<AnimationControl>().m_BrosAnimation.transform.localScale.z);
                            }
                            else
                            {
                                GetComponent<AnimationControl>().m_BrosAnimation.transform.localScale
                            = new Vector3(GetComponent<AnimationControl>().m_BrosAnimation.transform.localScale.x, GetComponent<AnimationControl>().m_BrosAnimation.transform.localScale.y, GetComponent<AnimationControl>().m_BrosAnimation.transform.localScale.z);
                            }
                        }

                        transform.localPosition = Vector3.MoveTowards(
                                                new Vector3(m_Nav.currentOffMeshLinkData.endPos.x, transform.localPosition.y, m_Nav.currentOffMeshLinkData.endPos.z),
                                                m_Nav.currentOffMeshLinkData.endPos,
                                                (m_Nav.speed * 2) * Time.deltaTime);
                    }
                }
                if (Vector3.Distance(transform.localPosition, m_Nav.currentOffMeshLinkData.endPos) < 0.1f)
                {
                    GetComponent<AnimationControl>().isClimb = false;

                    GetComponent<AnimationControl>().m_Anim.SetBool("climb", false);
                    GetComponent<AnimationControl>().m_Anim.SetBool("climbSide", false);
                    m_Nav.CompleteOffMeshLink();
                    m_Nav.Resume();
                }

                yield return null;
            }
            else
            {
                if (walkTime > 0.5f)
                {
                    m_Audio.PlayOneShot(m_BrotherStateManager.m_SE[0]);
                    walkTime = 0;
                }
                yield return null;
            }
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
