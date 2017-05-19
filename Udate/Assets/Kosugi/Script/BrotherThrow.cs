using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrotherThrow : MonoBehaviour
{
    [SerializeField, TooltipAttribute("弟用の座標オブジェクト")]
    private Transform BrotherPosition;

    [SerializeField, TooltipAttribute("投げる角度")]
    public float _firingAngle = 45.0f;
    [HideInInspector, TooltipAttribute("重力(変更禁止)")]
    private float _gravity = 9.8f;
    
    [SerializeField, TooltipAttribute("ターゲットobj")]
    private GameObject m_TargetObj;
    [HideInInspector, TooltipAttribute("ターゲット")]
    public GameObject m_Target;
    [SerializeField, TooltipAttribute("ターゲット指定用obj")]
    public GameObject m_TargetPos;

    [HideInInspector, TooltipAttribute("エネミーに当たったかどうか")]
    public bool _enemyHit;
    [HideInInspector, TooltipAttribute("距離減衰用変数")]
    public float _count = 2.0f;

    [SerializeField, TooltipAttribute("プレイヤーオブジェクト")]
    public GameObject Player;

    [SerializeField, TooltipAttribute("衝撃波オブジェクト")]
    private GameObject m_ShockWave;
    private float scale;

    //開始地点Y座標
    private Vector3 StartPos;
    //着地地点Y座標
    private Vector3 EndPos;

    private float _targetDistance;
    private List<GameObject> m_EnemyList;

    private bool isflying = false;

    //弟管理クラス
    private BrotherStateManager m_BrotherStateManager;

    void Awake()
    {
        
    }

    void Start()
    {
        m_EnemyList = new List<GameObject>();
        m_BrotherStateManager = GetComponent<BrotherStateManager>();

        scale = 0.0f;
    }

    void Update()
    {

    }

    public void ThrowStart()
    {
        isflying = false;
        StartCoroutine(TargetMove());
    }

    struct RayHitInfo
{
    public RaycastHit hit;
    //当たったか？
    public bool isHit;
};
IEnumerator TargetMove()
    {
        GameObject obj = Instantiate(m_TargetObj);
        while (true)
        {
            //float dx = Input.GetAxis("BrosHorizontal");
            //float dz = Input.GetAxis("BrosVertical");
            //m_Target.transform.transform.Translate(dx * 0.1f, 0.0f, dz * 0.1f);
            //yield return null;

            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    StartCoroutine(SimulateProjectile());
            //    yield break;
            //}
            //transform.position = BrotherPosition.position;

            float dx = Input.GetAxis("BrosHorizontal");
            float dz = Input.GetAxis("BrosVertical");
            m_TargetPos.transform.Translate(dx * 0.1f, 0.0f, dz * 0.1f);

            Vector3 rayPos = m_TargetPos.transform.position;
            Ray ray = new Ray(rayPos, -m_TargetPos.transform.up);
            RaycastHit hit;
            int layermask = 1 << 8;
            RayHitInfo m_Hitinfo;
            m_Hitinfo.isHit=Physics.Raycast(ray,out hit, 20.0f, layermask, QueryTriggerInteraction.Ignore);
            m_Hitinfo.hit = hit;
            Debug.DrawRay(rayPos, -m_TargetPos.transform.up*20.0f, Color.black, 0.1f, true);

            if (m_Hitinfo.isHit)
                obj.transform.position 
                    = new Vector3(m_Hitinfo.hit.point.x, m_Hitinfo.hit.point.y + obj.transform.localScale.y / 2, m_Hitinfo.hit.point.z);
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Target = obj;
                isflying = true;
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
        StartPos = transform.position;

        // プログラム開始までの待機時間
        //yield return new WaitForSeconds(1.5f);

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
        while (m_BrotherStateManager.GetState() == BrotherState.THROW)//!transform.GetComponent<Brother>()._isFloor)//elapse_time < flightDuration)
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
        StartPos = transform.position;

        // プログラム開始までの待機時間
        //yield return new WaitForSeconds(1.5f);

        //再バウンド先
        m_Target.transform.position += new Vector3(
            (m_Target.transform.position.x - Player.transform.position.x) / count,
            0,
            (m_Target.transform.position.z - Player.transform.position.z) / count);

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
        while (m_BrotherStateManager.GetState()== BrotherState.THROW)//!transform.GetComponent<Brother>()._isFloor)//elapse_time < flightDuration)
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
    
    //簡易的衝撃波
    IEnumerator ShockWave()
    {
        GameObject shockwave = (GameObject)Instantiate(m_ShockWave, EndPos, Quaternion.identity);
        if (StartPos.y - EndPos.y >= 1.0f)
        {
            while (scale < StartPos.y - EndPos.y)
            {
                scale += 1.0f;// * Time.deltaTime;
                shockwave.transform.localScale = new Vector3(scale, shockwave.transform.localScale.y, scale);
                yield return null;
            }
        }
        //yield return new WaitForSeconds(0.1f);
        Destroy(shockwave);
        scale = 0.0f;
        m_BrotherStateManager.SetState(BrotherState.BACK);
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
        m_EnemyList.Clear();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && m_BrotherStateManager.GetState() == BrotherState.THROW &&isflying)
        {
            EndPos = transform.position;
            StartCoroutine(ShockWave());

            //GetComponent<Rigidbody>().useGravity = true;
        }
        if (collision.gameObject.tag == "Enemy" && m_BrotherStateManager.GetState() == BrotherState.THROW)
        {
            if (collision.gameObject.GetComponent<EnemyBase>().GetEnemyState() != EnemyBase.EnemyState.GET)
            {
                _enemyHit = true;
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
                m_EnemyList.Add(collision.gameObject);
                collision.gameObject.SendMessage("ChangeState", 3, SendMessageOptions.DontRequireReceiver);
            }          
        }
    }
}
