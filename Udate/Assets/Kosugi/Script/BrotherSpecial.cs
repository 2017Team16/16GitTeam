using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BrotherSpecial : MonoBehaviour
{
    [SerializeField, Header("プレイヤーオブジェクト")]
    public GameObject Player;

    [HideInInspector, Header("投げる角度")]
    private float _firingAngle = 45.0f;
    [HideInInspector, Header("重力(変更禁止)")]
    private float _gravity = 9.8f;

    [HideInInspector, Header("当たったかどうか")]
    public bool _hit = false;
    [HideInInspector, Header("フィールドにいる敵のリスト")]
    private List<GameObject> m_Enemys;

    //プレイヤーへと戻っているか
    private bool _isPlayer;

    //弟管理クラス
    private BrotherStateManager m_BrotherStateManager;

    // Use this for initialization
    void Start()
    {
        _firingAngle = GetComponent<BrotherThrow>()._firingAngle;

        m_Enemys = new List<GameObject>();

        m_BrotherStateManager = GetComponent<BrotherStateManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnemySet()
    {
        Transform[] enemysTrans = GameObject.FindGameObjectsWithTag("Enemy").Select(e=>e.transform).ToArray();
        Transform[] enemys = enemysTrans.OrderBy(e => e.transform.position.y).ToArray();
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].GetComponent<EnemyBase>().GetEnemyState() != EnemyBase.EnemyState.GET)
                m_Enemys.Add(enemys[i].gameObject);
            //m_Enemys[i].GetComponent<NavMeshAgent>().speed = 0.1f;
        }
        m_Enemys.Add(Player);
        print(m_Enemys.Count);
        StartCoroutine(SpecialMove());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerator SpecialMove()
    {
        for (int i = 0; i < m_Enemys.Count; i++)
        {
            print(m_Enemys[i].name);
            _hit = false;

            // 投げるオブジェクトの開始位置
            transform.position = transform.position + new Vector3(0, 0.0f, 0);

            // 投げるオブジェクトからターゲットまでの距離を計算
            float _targetDistance = Vector3.Distance(transform.position, m_Enemys[i].transform.position);

            // 指定した角度でオブジェクトをターゲットまで投げる時の速度を計算
            float projectile_Velocity = _targetDistance / (Mathf.Sin(2 * _firingAngle * Mathf.Deg2Rad) / (_gravity *3));

            // X軸とY軸での速度をそれぞれ計算
            float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(_firingAngle * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(_firingAngle * Mathf.Deg2Rad);

            // 滞空時間を計算
            float flightDuration = _targetDistance / Vx;

            // ターゲットまで投げる時のオブジェクトの回転度合い
            transform.rotation = Quaternion.LookRotation(m_Enemys[i].transform.position - transform.position);

            // 放物線の計算
            float elapse_time = 0;
            float flightTimer = flightDuration;
            while (!_hit && m_BrotherStateManager.GetState() == BrotherState.SPECIAL)
            {
                transform.Translate(0, (Vy - (_gravity*3 * elapse_time)) * Time.unscaledDeltaTime, Vx * Time.unscaledDeltaTime);

                elapse_time += Time.unscaledDeltaTime;

                if (elapse_time >= flightDuration)
                {
                    if (m_Enemys[i] == Player && i == m_Enemys.Count - 1)
                    {
                        Time.timeScale = 1;
                        //_hit = true;
                        //m_Enemys.Clear();

                        //m_BrotherStateManager.SetState(BrotherState.NORMAL);
                    }
                    else
                    {
                        m_Enemys[i].gameObject.SendMessage("ChangeState", 3, SendMessageOptions.DontRequireReceiver);
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
                if (m_Enemys[i] == Player) return;
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
            _hit = true;
            m_Enemys.Clear();
            m_BrotherStateManager.SetState(BrotherState.NORMAL);
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
