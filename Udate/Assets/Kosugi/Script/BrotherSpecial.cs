using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrotherSpecial : MonoBehaviour
{
    [SerializeField, TooltipAttribute("プレイヤーオブジェクト")]
    public GameObject Player;

    [HideInInspector, TooltipAttribute("投げる角度")]
    private float _firingAngle = 45.0f;
    [HideInInspector, TooltipAttribute("重力(変更禁止)")]
    private float _gravity = 9.8f;
    [SerializeField, TooltipAttribute("速度調整用掛け数(掛けるほど速い)")]
    private float _gravityMultiply = 2.0f;

    [HideInInspector, TooltipAttribute("当たったかどうか")]
    public bool _hit = false;
    [HideInInspector, TooltipAttribute("フィールドにいる敵のリスト")]
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
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemys.Length; i++)
        {
            m_Enemys.Add(enemys[i]);
            //m_Enemys[i].GetComponent<NavMeshAgent>().speed = 0.1f;
        }
        m_Enemys.Add(Player);
        StartCoroutine(SpecialMove());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerator SpecialMove()
    {
        for (int i = 0; i < m_Enemys.Count; i++)
        {
            print(m_Enemys.Count);
            _hit = false;
            if (m_Enemys[i] == Player)
            {
                _isPlayer = true;
            }
            else
                _isPlayer = false;

            // 投げるオブジェクトの開始位置
            transform.position = transform.position + new Vector3(0, 0.0f, 0);

            // 投げるオブジェクトからターゲットまでの距離を計算
            float _targetDistance = Vector3.Distance(transform.position, m_Enemys[i].transform.position);

            // 指定した角度でオブジェクトをターゲットまで投げる時の速度を計算
            float projectile_Velocity = _targetDistance / (Mathf.Sin(2 * _firingAngle * Mathf.Deg2Rad) / (_gravity * _gravityMultiply));

            // X軸とY軸での速度をそれぞれ計算
            float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(_firingAngle * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(_firingAngle * Mathf.Deg2Rad);

            // 滞空時間を計算
            float flightDuration = _targetDistance / Vx;

            // ターゲットまで投げる時のオブジェクトの回転度合い
            transform.rotation = Quaternion.LookRotation(m_Enemys[i].transform.position - transform.position);

            // 放物線の計算
            float elapse_time = 0;
            while (!_hit && m_BrotherStateManager.GetState() == BrotherState.SPECIAL)
            {
                transform.Translate(0, (Vy - ((_gravity * _gravityMultiply) * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

                elapse_time += Time.deltaTime;

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
        if (collision.gameObject.tag == "Player" && m_BrotherStateManager.GetState() == BrotherState.SPECIAL && _isPlayer == true)
        {
            _hit = true;
            m_Enemys.Clear();
            m_BrotherStateManager.SetState(BrotherState.NORMAL);
        }
        if (collision.gameObject.tag == "Enemy" && m_BrotherStateManager.GetState() == BrotherState.SPECIAL)
        {
            //if (collision.gameObject.GetComponent<EnemyBase>().GetEnemyState() != EnemyBase.EnemyState.SUTAN)
            {
                _hit = true;
                collision.gameObject.SendMessage("ChangeState", 3, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
