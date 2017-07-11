using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class BrotherSpecial : MonoBehaviour
{
    /*--外部設定オブジェクト--*/
    [SerializeField, Header("ポジション用オブジェクト")]
    private GameObject BrotherPosition;

    [SerializeField, Header("必殺技用Canvas内オブジェクト")]
    private GameObject Box;
    [SerializeField, Header("パーティクルオブジェクト")]
    public GameObject m_Particle;


    /*------外部設定変数------*/
    [SerializeField, Header("投げる速度")]
    private float _speed = 2.5f;


    /*------内部設定変数------*/
    [Header("投げる角度")]
    private float _flyingAngle = 45.0f;
    [Header("重力")]
    private float _gravity = 9.8f;

    [Header("フィールドにいる敵のリスト")]
    private List<GameObject> m_Enemys;

    [HideInInspector, Header("当たったかどうか")]
    public bool _hit = false;

    [Header("サウンド")]
    private AudioSource m_Audio;

    [Header("弟管理クラス")]
    private BrotherStateManager m_BrotherStateManager;

    // Use this for initialization
    void Start()
    {
        m_Enemys = new List<GameObject>();

        m_BrotherStateManager = GetComponent<BrotherStateManager>();

        m_Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpecialSet()
    {
        Box.SetActive(true);
        m_Audio.PlayOneShot(m_BrotherStateManager.m_SE[4]);
        //EnemySet();
    }

    void EnemySet()
    {
        GameDatas.isBrotherSpecialMove = true;
        GetComponent<AnimationControl>().m_Anim.SetTrigger("fly");

        Transform[] enemysTrans = GameObject.FindGameObjectsWithTag("Enemy").Select(e=>e.transform).ToArray();
        Transform[] enemys = enemysTrans.OrderBy(e => e.transform.position.y).ToArray();
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].GetComponent<EnemyBase>().GetEnemyState() != EnemyBase.EnemyState.GET)
                m_Enemys.Add(enemys[i].gameObject);
            //m_Enemys[i].GetComponent<NavMeshAgent>().speed = 0.1f;
        }

        m_Enemys.Add(BrotherPosition);
        StartCoroutine(SpecialMove());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerator SpecialMove()
    {

        GetComponent<AnimationControl>().m_Anim.SetBool("rotate", true);

        for (int i = 0; i < m_Enemys.Count; i++)
        {
            _hit = false;

            // 投げるオブジェクトの開始位置
            transform.position = transform.position + new Vector3(0, 0.0f, 0);

            // 投げるオブジェクトからターゲットまでの距離を計算
            float _targetDistance = Vector3.Distance(transform.position, m_Enemys[i].transform.position);

            // 指定した角度でオブジェクトをターゲットまで投げる時の速度を計算
            float projectile_Velocity = _targetDistance / (Mathf.Sin(2 * _flyingAngle * Mathf.Deg2Rad) / (_gravity * _speed));

            // X軸とY軸での速度をそれぞれ計算
            float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(_flyingAngle * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(_flyingAngle * Mathf.Deg2Rad);

            // 滞空時間を計算
            float flightDuration = _targetDistance / Vx;

            // ターゲットまで投げる時のオブジェクトの回転度合い
            transform.rotation = Quaternion.LookRotation(m_Enemys[i].transform.position - transform.position);
            
            // 放物線の計算
            float elapse_time = 0;
            float flightTimer = flightDuration;
            while (!_hit && m_BrotherStateManager.GetState() == BrotherState.SPECIAL)
            {
                if (GameDatas.isBrotherSpecialMove)
                {
                    transform.Translate(0, (Vy - (_gravity * _speed * elapse_time)) * Time.unscaledDeltaTime, Vx * Time.unscaledDeltaTime);
                    elapse_time += Time.unscaledDeltaTime;
                    GetComponent<AnimationControl>().m_Anim.speed = 1;
                }
                else
                    GetComponent<AnimationControl>().m_Anim.speed = 0;

                m_Particle.GetComponent<ParticleSystem>().Play();

                if (elapse_time >= flightDuration)
                {
                    if (m_Enemys[i] == BrotherPosition && i == m_Enemys.Count - 1)
                    {
                        Time.timeScale = 1;

                        m_Particle.GetComponent<ParticleSystem>().Stop();
                        GameDatas.isBrotherSpecialMove = false;

                        GetComponent<AnimationControl>().m_Anim.SetBool("rotate", false);

                        _hit = true;
                        m_Enemys.Clear();
                        m_BrotherStateManager.SetState(BrotherState.NORMAL);

                        m_Audio.PlayOneShot(m_BrotherStateManager.m_SE[1]);
                    }
                    else
                    {
                        m_Enemys[i].gameObject.SendMessage("ChangeState", 3, SendMessageOptions.DontRequireReceiver);
                        m_Audio.PlayOneShot(m_BrotherStateManager.m_SE[1]);
                    }
                    _hit = true;
                }

                yield return null;
            }
        }
    }

    public void IsTriggerOff()
    {
        if (m_Enemys.Count != 0)
        {
            for (int i = 0; i < m_Enemys.Count; i++)
            {
                if (m_Enemys[i] == BrotherPosition) return;
                //m_Enemys[i].GetComponent<NavMeshAgent>().speed = 3.5f;
            }
        }
        
        //m_EnemyList.Clear();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && m_BrotherStateManager.GetState() == BrotherState.SPECIAL)
        {
            _hit = true;
        }
        if (collision.gameObject.tag == "Player" && m_BrotherStateManager.GetState() == BrotherState.SPECIAL)
        {
            //_hit = true;
            //m_Enemys.Clear();
            //m_BrotherStateManager.SetState(BrotherState.NORMAL);

            //m_Audio.PlayOneShot(m_BrotherStateManager.m_SE[1]);
        }
        if (collision.gameObject.tag == "Enemy" && m_BrotherStateManager.GetState() == BrotherState.SPECIAL)
        {
            if (collision.gameObject.GetComponent<EnemyBase>().GetEnemyState() != EnemyBase.EnemyState.GET)
            {
                
                _hit = true;
                collision.gameObject.SendMessage("ChangeState", 3, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
