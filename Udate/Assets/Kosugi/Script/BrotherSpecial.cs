using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrotherSpecial : MonoBehaviour
{
    private List<GameObject> m_Enemys;

    [HideInInspector, TooltipAttribute("投げる角度")]
    private float _firingAngle = 45.0f;
    //重力(変更不要)
    private float _gravity = 9.8f;

    /*BrotherThrowから*/
    //[HideInInspector, TooltipAttribute("エネミーに当たったかどうか")]
    public bool _enemyHit = false;
    //当たった敵のリスト
    private List<GameObject> m_EnemyList;
    //プレイヤーオブジェクト
    private GameObject Player;
    /*----------------*/

    //弟管理クラス
    private BrotherStateManager m_BrotherStateManager;

    // Use this for initialization
    void Start()
    {
        _firingAngle = GetComponent<BrotherThrow>()._firingAngle;
        Player = GetComponent<BrotherThrow>().Player;
        m_Enemys = new List<GameObject>();
        m_EnemyList = new List<GameObject>();

        m_BrotherStateManager = GetComponent<BrotherStateManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnemySet()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        print(enemys.Length);
        for (int i = 0; i < enemys.Length; i++)
        {
            m_Enemys.Add(enemys[i]);
            m_Enemys[i].GetComponent<NavMeshAgent>().speed = 0.1f;
        }
        m_Enemys.Add(Player);
        StartCoroutine(Special());
        //m_BrotherStateManager.SetState(BrotherState.NORMAL); 
    }
    IEnumerator Special()
    {
            for (int i = 0; i < m_Enemys.Count; i++)
            {
                _enemyHit = false;
                // 投げるオブジェクトの開始位置
                //Projectile.position = transform.position + new Vector3(0, 0.0f, 0);
                transform.position = transform.position + new Vector3(0, 0.0f, 0);

                // 投げるオブジェクトからターゲットまでの距離を計算
                float _targetDistance = Vector3.Distance(transform.position, m_Enemys[i].transform.position);/**/

                // 指定した角度でオブジェクトをターゲットまで投げる時の速度を計算
                float projectile_Velocity = _targetDistance / (Mathf.Sin(2 * _firingAngle * Mathf.Deg2Rad) / _gravity);

                // X軸とY軸での速度をそれぞれ計算
                float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(_firingAngle * Mathf.Deg2Rad);
                float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(_firingAngle * Mathf.Deg2Rad);

                // 滞空時間を計算
                float flightDuration = _targetDistance / Vx;

                // ターゲットまで投げる時のオブジェクトの回転度合い
                transform.rotation = Quaternion.LookRotation(m_Enemys[i].transform.position - transform.position);/**/

                // 放物線の計算
                float elapse_time = 0;
                while (!_enemyHit)//elapse_time < flightDuration)
                {
                    transform.Translate(0, (Vy - (_gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

                    elapse_time += Time.deltaTime;

                    yield return null;
                }
            }       
    }

    public void IsTriggerOff()
    {
        if (m_EnemyList.Count != 0)
        {
            for (int i = 0; i < m_EnemyList.Count; i++)
            {
                m_Enemys[i].GetComponent<NavMeshAgent>().speed = 3.5f;
                m_EnemyList[i].GetComponent<Collider>().isTrigger = false;
            }
        }
        m_Enemys.Clear();
        m_EnemyList.Clear();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && m_BrotherStateManager.GetState() == BrotherState.SPECIAL)
        {
            //m_BrotherStateManager.SetState(BrotherState.NORMAL);
            //GetComponent<Rigidbody>().useGravity = true;
        }
        if(collision.gameObject.tag == "Player" && m_BrotherStateManager.GetState() == BrotherState.SPECIAL)
        {
            _enemyHit = true;
            m_BrotherStateManager.SetState(BrotherState.NORMAL);
        }
        if (collision.gameObject.tag == "Enemy" && m_BrotherStateManager.GetState() == BrotherState.SPECIAL)
        {
            if (collision.gameObject.GetComponent<EnemyBase>().GetEnemyState() != EnemyBase.EnemyState.GET)
            {
                _enemyHit = true;
                //collision.gameObject.GetComponent<Collider>().isTrigger = true;
                m_EnemyList.Add(collision.gameObject);
                collision.gameObject.SendMessage("ChangeState", 3, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
