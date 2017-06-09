using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Rayの当たり判定用
struct RayHitInfo
{
    public RaycastHit hit;
    //当たったか？
    public bool isHit;
};
public class BrotherThrow : MonoBehaviour
{
    /*--オブジェクト指定--*/
    [SerializeField, TooltipAttribute("ポジション用オブジェクト")]
    private Transform BrotherPosition;

    [SerializeField, TooltipAttribute("プレイヤーオブジェクト")]
    public GameObject Player;

    [SerializeField, TooltipAttribute("ターゲットプレハブ")]
    private GameObject m_Target;
    [SerializeField, TooltipAttribute("ターゲット生成用オブジェクト")]
    public GameObject m_TargetCreate;
    [SerializeField, TooltipAttribute("衝撃波プレハブ")]
    private GameObject m_ShockWave;
    /*--------------------*/

    [HideInInspector, TooltipAttribute("ターゲット")]
    public GameObject Target;

    [SerializeField, TooltipAttribute("ターゲット移動速度")]
    private float _targetSpeed = 0.1f;

    [SerializeField, TooltipAttribute("投げる角度")]
    public float _firingAngle = 45.0f;
    [HideInInspector, TooltipAttribute("重力(変更禁止)")]
    private float _gravity = 9.8f;
    [HideInInspector, TooltipAttribute("エネミーに当たったかどうか")]
    public bool _enemyHit = false;
    [HideInInspector, TooltipAttribute("距離減衰用変数")]
    public float _count = 2.0f;

    //衝撃波用
    private float _scale = 0.0f;

    //投げ開始地点Y座標
    private Vector3 StartPos;
    //投げ着地地点Y座標
    private Vector3 EndPos;

    //投げる距離保存用
    private float _targetDistance;
    //当たった敵保存用
    //private List<GameObject> m_EnemyList;

    //弟の前ベクトル
    Vector3 front = Vector3.zero;
    //弟の上ベクトル
    Vector3 up = Vector3.zero;
    //壁の前向きベクトル
    Vector3 wallFront = Vector3.zero;
    //床の下向きベクトル
    Vector3 floorBottom = Vector3.zero;

    private bool next = false;
    private bool noFirst = false;

    //弟管理クラス
    private BrotherStateManager m_BrotherStateManager;

    void Awake()
    {

    }

    void Start()
    {
        //m_EnemyList = new List<GameObject>();
        m_BrotherStateManager = GetComponent<BrotherStateManager>();
    }

    void Update()
    {
        front = new Vector3(transform.forward.x, 0, transform.forward.z);
        up = new Vector3(front.x, transform.position.y, front.z);
    }

    public void ThrowStart()
    {
        m_TargetCreate.transform.position = new Vector3(0.0f, m_TargetCreate.transform.position.y, 0.0f);
        StartCoroutine(TargetMove());
    }
    
    IEnumerator TargetMove()
    {
        Target = Instantiate(m_Target);
        Vector3 pScale = GetComponent<AnimationControl>().m_Anim.transform.localScale;
        Vector3 nScale = GetComponent<AnimationControl>().m_Anim.transform.localScale;
        nScale.x *= -1;
        while (true)
        {
            print(Player.transform.localScale.x);
            if (Player.transform.Find("Ani").localScale.x >= 0)
            {
                GetComponent<AnimationControl>().m_Anim.transform.localScale = pScale;
            }
            else
            {
                GetComponent<AnimationControl>().m_Anim.transform.localScale = nScale;
            }

            float dx = Input.GetAxis("BrosHorizontal");
            float dz = Input.GetAxis("BrosVertical");
            m_TargetCreate.transform.Translate(dx * _targetSpeed, 0.0f, dz * _targetSpeed);

            Vector3 rayPos = m_TargetCreate.transform.position;
            Ray ray = new Ray(rayPos, -m_TargetCreate.transform.up);
            RaycastHit hit;
            RaycastHit noHit;
            int layermask = 1 << 8;
            int noHitLayermask = 1 << 9;
            RayHitInfo m_Hitinfo;
            RayHitInfo m_NoHitinfo;
            m_Hitinfo.isHit = Physics.Raycast(ray, out hit, 20.0f, layermask, QueryTriggerInteraction.Ignore);
            m_NoHitinfo.isHit = Physics.Raycast(ray, out noHit, 20.0f, noHitLayermask, QueryTriggerInteraction.Ignore);
            m_Hitinfo.hit = hit;
            m_NoHitinfo.hit = noHit;

            transform.LookAt(Target.transform.position);

            if (m_Hitinfo.isHit)
            {
                if (hit.transform.gameObject.transform.position.y >= transform.position.y)
                {
                    hit.transform.gameObject.layer = 9;
                }
                else
                {
                    Target.transform.position
                        = new Vector3(m_Hitinfo.hit.point.x, m_Hitinfo.hit.point.y + Target.transform.localScale.y / 2, m_Hitinfo.hit.point.z);
                }
            }
            if (m_NoHitinfo.isHit && noHit.transform.gameObject.transform.position.y < transform.position.y)
            {
                noHit.transform.gameObject.layer = 8;
            }

            yield return null;

            //if (Input.GetKeyDown(KeyCode.Space))

            //if (Input.GetButtonDown("XboxL1"))
            if(GameDatas.isBrotherFlying)
            {
                GetComponent<AnimationControl>().m_Anim.GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<AnimationControl>().m_Anim.SetTrigger("fly");

                if (m_NoHitinfo.isHit)
                {
                    noHit.transform.gameObject.layer = 8;
                }

                StartCoroutine(SimulateProjectile());
                

                yield break;
            }
            else if(Input.GetButtonDown("XboxB"))
            {
                Destroy(Target);
                
                m_BrotherStateManager.SetState(BrotherState.NORMAL);

                yield break;
            }
            transform.position = BrotherPosition.position;
        }
    }

    /// <summary>
    /// 投げ
    /// </summary>
    /// <returns></returns>
    IEnumerator SimulateProjectile()
    {
        next = false;

        StartPos = transform.position;

        // プログラム開始までの待機時間
        //yield return new WaitForSeconds(1.5f);

        //再バウンド先
        if (noFirst)
        {
            Target.transform.position += new Vector3(
                (Target.transform.position.x - Player.transform.position.x) / _count,
                0,
                (Target.transform.position.z - Player.transform.position.z) / _count);
        }

        // 投げるオブジェクトの開始位置
        //Projectile.position = transform.position + new Vector3(0, 0.0f, 0);
        transform.position = transform.position + new Vector3(0, 0.0f, 0);

        // 投げるオブジェクトからターゲットまでの距離を計算
        _targetDistance = Vector3.Distance(transform.position, Target.transform.position);

        // 指定した角度でオブジェクトをターゲットまで投げる時の速度を計算
        float projectile_Velocity = _targetDistance / (Mathf.Sin(2 * _firingAngle * Mathf.Deg2Rad) / _gravity);

        // X軸とY軸での速度をそれぞれ計算
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(_firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(_firingAngle * Mathf.Deg2Rad);

        // 滞空時間を計算
        float flightDuration = _targetDistance / Vx;

        // ターゲットまで投げる時のオブジェクトの回転度合い
        transform.rotation = Quaternion.LookRotation(Target.transform.position - transform.position);

        // 放物線の計算
        float elapse_time = 0;
        while (m_BrotherStateManager.GetState() == BrotherState.THROW)
        {
            transform.Translate(0, (Vy - (_gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            Debug.DrawRay(transform.position, new Vector3(front.x, transform.position.y, front.z) * 10, Color.black, 0.1f, true);
            if (next)
                yield return SimulateProjectile();

            yield return null;
        }
    }

    //衝撃波
    IEnumerator ShockWave()
    {
        GameObject shockwave = (GameObject)Instantiate(m_ShockWave, EndPos, Quaternion.identity);
        if (StartPos.y - EndPos.y > 1.0f)
        {
            while (_scale < StartPos.y - EndPos.y)
            {
                _scale += 1.0f;
                shockwave.transform.localScale = new Vector3(_scale, shockwave.transform.localScale.y, _scale);
                yield return null;
            }
        }
        Destroy(shockwave);
        _scale = 0.0f;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && m_BrotherStateManager.GetState() == BrotherState.THROW && GameDatas.isBrotherFlying)
        {
            _count = 2.0f;
            next = false;
            noFirst = false;

            if (transform.position.y >= collision.transform.position.y)
            {
                m_BrotherStateManager.SetState(BrotherState.BACK);
                GetComponent<AnimationControl>().m_Anim.SetTrigger("land");
                EndPos = transform.position;
                StartCoroutine(ShockWave());
                //m_BrotherStateManager.SetState(BrotherState.BACK);
            }
            else
            {
                //当たった場所
                Vector3 reflectPos = transform.position;

                //Wallレイヤーのオブジェクトに対してRayを当てもろもろの値を取得
                Ray ray = new Ray(transform.position, up);
                RaycastHit hit;
                int layermask = 1 << 8;
                RayHitInfo m_Hitinfo;
                m_Hitinfo.isHit = Physics.Raycast(ray, out hit, 5.0f, layermask, QueryTriggerInteraction.Ignore);
                m_Hitinfo.hit = hit;

                //壁の面に対して垂直なベクトルを生成
                if (m_Hitinfo.isHit)
                {
                    floorBottom = m_Hitinfo.hit.normal.normalized;
                }

                //向きを変更・確定
                if (Vector3.Dot(up, floorBottom) >= 0)
                    transform.forward = Vector3.Reflect(up, -floorBottom);
                else
                    transform.forward = Vector3.Reflect(up, floorBottom);

                Target.transform.position = new Vector3(reflectPos.x, Target.transform.position.y, reflectPos.z);
            }
        }
        if (collision.gameObject.tag == "Enemy" && m_BrotherStateManager.GetState() == BrotherState.THROW)
        {
            if (collision.gameObject.GetComponent<EnemyBase>().GetEnemyState() != EnemyBase.EnemyState.SUTAN
                && collision.gameObject.GetComponent<EnemyBase>().GetEnemyState() != EnemyBase.EnemyState.GET)
            {
                if (noFirst)
                    _count *= 2.0f;
                next = true;

                collision.gameObject.SendMessage("ChangeState", 3, SendMessageOptions.DontRequireReceiver);
                if (!noFirst)
                    noFirst = true;
            }
        }
        //壁の跳ね返り
        if (collision.gameObject.tag == "Wall" && m_BrotherStateManager.GetState() == BrotherState.THROW)
        {
            //Wallレイヤーのオブジェクトに対してRayを当てもろもろの値を取得
            Ray ray = new Ray(transform.position, front);
            RaycastHit hit;
            int layermask = 1 << 10;
            RayHitInfo m_Hitinfo;
            m_Hitinfo.isHit = Physics.Raycast(ray, out hit, 5.0f, layermask, QueryTriggerInteraction.Ignore);
            m_Hitinfo.hit = hit;
            Debug.DrawRay(transform.position, front * 5.0f, Color.black, 0.1f, true);

            //壁の面に対して垂直なベクトルを生成
            if (m_Hitinfo.isHit)
            {
                wallFront= m_Hitinfo.hit.normal.normalized;
            }

            //向きを変更・確定
            if (Vector3.Dot(front, wallFront) >= 0)
                transform.forward = Vector3.Reflect(front, -wallFront);
            else
                transform.forward = Vector3.Reflect(front, wallFront);
        }
    }
}