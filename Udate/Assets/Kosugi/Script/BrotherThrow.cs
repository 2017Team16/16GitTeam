using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrotherThrow : MonoBehaviour
{
    [SerializeField, TooltipAttribute("弟用の座標オブジェクト")]
    private Transform BrotherPosition;

    [SerializeField, TooltipAttribute("投げる角度")]
    private float _firingAngle = 45.0f;
    //重力(変更不要)
    private float _gravity = 9.8f;

    [SerializeField, TooltipAttribute("ターゲットオブジェクト")]
    public GameObject m_Target;

    [HideInInspector, TooltipAttribute("エネミーに当たったかどうか")]
    public bool _enemyHit;
    [HideInInspector, TooltipAttribute("距離減衰用変数")]
    public float _count = 2.0f;

    [SerializeField, TooltipAttribute("プレイヤーオブジェクト")]
    private Transform Player;

    private float _targetDistance;
    private List<GameObject> m_EnemyList;

    //弟管理クラス
    private BrotherStateManager m_BrotherStateManager;

    void Awake()
    {

    }

    void Start()
    {
        m_EnemyList = new List<GameObject>();
        m_BrotherStateManager = GetComponent<BrotherStateManager>();
    }

    void Update()
    {

    }

    public void ThrowStart()
    {
        StartCoroutine(TargetMove());
    }

    /// <summary>
    /// ターゲットの移動
    /// </summary>
    /// <returns></returns>
    IEnumerator TargetMove()
    {
        while(true)
        {
            float dx = Input.GetAxis("BrosHorizontal");
            float dz = Input.GetAxis("BrosVertical");
            m_Target.transform.transform.Translate(dx * 0.1f, 0.0f, dz * 0.1f);
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(SimulateProjectile());
                yield break;
            }
            transform.position = BrotherPosition.position;
        }
    }

    /// <summary>
    /// 最初の投げ
    /// </summary>
    /// <returns></returns>
    IEnumerator SimulateProjectile()
    {
        // プログラム開始までの待機時間
        //yield return new WaitForSeconds(1.5f);

        //重要  ターゲット(のオブジェクト)を探す→ターゲットをリアルタイムで変化させてその地点にダミーを表示させる に変更

        // 投げるオブジェクトの開始位置
        //Projectile.position = transform.position + new Vector3(0, 0.0f, 0);
        transform.position = transform.position + new Vector3(0, 0.0f, 0);

        // 投げるオブジェクトからターゲットまでの距離を計算
        _targetDistance = Vector3.Distance(transform.position, m_Target.transform.position);

        // 指定した角度でオブジェクトをターゲットまで投げる時の速度を計算
        float projectile_Velocity = _targetDistance / (Mathf.Sin(2 * _firingAngle * Mathf.Deg2Rad) / _gravity);

        // X軸とY軸での速度をそれぞれ計算
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(_firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(_firingAngle * Mathf.Deg2Rad);

        // 滞空時間を計算
        float flightDuration = _targetDistance / Vx;

        // ターゲットまで投げる時のオブジェクトの回転度合い
        transform.rotation = Quaternion.LookRotation(m_Target.transform.position - transform.position);

        // 放物線の計算
        float elapse_time = 0;
        while (!transform.GetComponent<Brother>()._isFloor)//elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (_gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            if(_enemyHit)
            {
                _enemyHit = false;
                StartCoroutine(ReSimulateProjectile(_count));
                
                yield break;
            }

            yield return null;
        }

    }
    /// <summary>
    /// 二回以降の投げ
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    IEnumerator ReSimulateProjectile(float count)
    {
        // プログラム開始までの待機時間
        //yield return new WaitForSeconds(1.5f);

        //再バウンド先
        m_Target.transform.position += new Vector3(
            (m_Target.transform.position.x - Player.position.x) / count,
            0,
            (m_Target.transform.position.z - Player.position.z) / count);

        // 投げるオブジェクトの開始位置
        //Projectile.position = transform.position + new Vector3(0, 0.0f, 0);
        transform.position = transform.position + new Vector3(0, 0.0f, 0);

        // 投げるオブジェクトからターゲットまでの距離を計算
        _targetDistance = Vector3.Distance(transform.position, m_Target.transform.position);

        // 指定した角度でオブジェクトをターゲットまで投げる時の速度を計算
        float projectile_Velocity = _targetDistance / (Mathf.Sin(2 * _firingAngle * Mathf.Deg2Rad) / _gravity);

        // X軸とY軸での速度をそれぞれ計算
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(_firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(_firingAngle * Mathf.Deg2Rad);

        // 滞空時間を計算
        float flightDuration = _targetDistance / Vx;

        // ターゲットまで投げる時のオブジェクトの回転度合い
        transform.rotation = Quaternion.LookRotation(m_Target.transform.position - transform.position);

        // 放物線の計算
        float elapse_time = 0;
        while (!transform.GetComponent<Brother>()._isFloor)//elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (_gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            if (_enemyHit)
            {
                _enemyHit = false;
                _count *= 2.0f;
                StartCoroutine(ReSimulateProjectile(_count));
                
                yield break;
            }

            yield return null;
        }

    }

    public void IsTriggerOff()
    {
        if (m_EnemyList.Count != 0)
        {
            for (int i = 0; i < m_EnemyList.Count; i++)
            {
                m_EnemyList[i].GetComponent<Collider>().isTrigger = false;
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && m_BrotherStateManager.GetState() == BrotherState.THROW)
        { 
            m_BrotherStateManager.SetState(BrotherState.WAIT);
            //GetComponent<Rigidbody>().useGravity = true;
        }
        if (collision.gameObject.tag == "Enemy" && m_BrotherStateManager.GetState() == BrotherState.THROW)
        {
            _enemyHit = true;
            collision.gameObject.GetComponent<Collider>().isTrigger = true;
            m_EnemyList.Add(collision.gameObject);
            collision.gameObject.SendMessage("ChangeState", 3, SendMessageOptions.DontRequireReceiver);           
        }
    }
}
