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
    /*--外部設定オブジェクト--*/
    [SerializeField, Header("兄の上指定用オブジェクト")]
    private Transform BrotherPosition;

    [SerializeField, Header("プレイヤーオブジェクト")]
    private GameObject Player;

    [SerializeField, Header("ターゲットプレハブ")]
    private GameObject m_Target;
    [SerializeField, Header("ターゲット生成用オブジェクト")]
    private GameObject m_TargetCreate;

    [SerializeField, Header("衝撃波プレハブ")]
    private GameObject m_ShockWave;
    [SerializeField, Header("衝撃波パーティクルオブジェクト")]
    private GameObject particle;


    /*------外部設定変数------*/
    [SerializeField, Header("ターゲット移動速度")]
    private float _targetSpeed = 5.0f;

    [SerializeField, Header("投げる速度")]
    private float _speed = 1.0f;


    /*------内部設定変数------*/
    [HideInInspector, Header("ターゲット")]
    public GameObject Target;

    [Header("投げる角度")]
    private float _flyingAngle = 45.0f;
    [Header("重力")]
    private float _gravity = 9.8f;
    [HideInInspector, Header("距離減衰用変数")]
    public float _count = 2.0f;

    [Header("二回目以降のバウンドか判定用")]
    private bool second = false;
    [Header("最初のバウンドか判定用")]
    private bool noFirst = false;

    [Header("投げ開始地点の座標")]
    private Vector3 StartPos;
    [Header("投げ終了地点の座標")]
    private Vector3 EndPos;

    [Header("衝撃波のスケール")]
    private float _scale = 0.0f;

    [Header("弟の前ベクトル")]
    private Vector3 front = Vector3.zero;
    [Header("弟の上ベクトル")]
    private Vector3 up = Vector3.zero;
    [Header("壁の前ベクトル")]
    private Vector3 wallFront = Vector3.zero;
    [Header("床の下ベクトル")]
    private Vector3 floorBottom = Vector3.zero;

    [Header("サウンド")]
    private AudioSource m_Audio;


    [Header("弟管理クラス")]
    private BrotherStateManager m_BrotherStateManager;

    void Awake()
    {
        m_Audio = GetComponent<AudioSource>();
        m_BrotherStateManager = GetComponent<BrotherStateManager>();
    }

    void Start()
    {

    }

    void Update()
    {
        front = new Vector3(transform.forward.x, 0, transform.forward.z);
        up = new Vector3(front.x, transform.position.y, front.z);
    }

    public void ThrowStart()
    {
        m_TargetCreate.transform.position = new Vector3(Player.transform.position.x, 15, Player.transform.position.z);
        StartCoroutine(TargetMove());
    }

    //ターゲット
    IEnumerator TargetMove()
    {
        Target = Instantiate(m_Target);
        Vector3 pScale = GetComponent<AnimationControl>().m_Anim.transform.localScale;
        Vector3 nScale = GetComponent<AnimationControl>().m_Anim.transform.localScale;
        nScale.x *= -1;
        while (true)
        {
            if (Player.transform.Find("Ani").localScale.x >= 0)
            {
                GetComponent<AnimationControl>().m_Anim.transform.localScale = pScale;
            }
            else
            {
                GetComponent<AnimationControl>().m_Anim.transform.localScale = nScale;
            }

            float dx = Input.GetAxis("BrosHorizontal")* _targetSpeed;
            float dz = Input.GetAxis("BrosVertical")* _targetSpeed;
            m_TargetCreate.GetComponent<Rigidbody>().velocity = new Vector3(dx, 0, dz);
            
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
                if (hit.transform.gameObject.transform.position.y > transform.position.y)
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

            if ((transform.position.y - Target.transform.position.y) / 3>1)
            {
                Target.transform.FindChild("cursor").transform.localScale
                    = new Vector3((transform.position.y - Target.transform.position.y) / 3, (transform.position.y - Target.transform.position.y) / 3, 1);
            }
            else
                Target.transform.FindChild("cursor").transform.localScale= new Vector3(1, 1, 1);

            if (GameDatas.isBrotherFlying)
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

            yield return null;
        }
    }

    //投げ
    IEnumerator SimulateProjectile()
    {
        GetComponent<AnimationControl>().m_Anim.SetBool("rotate", true);

        second = false;

        StartPos = transform.position;

        Vector3 targetPos = Target.transform.position;

        // プログラム開始までの待機時間
        //yield return new WaitForSeconds(1.5f);

        //再バウンド先
        if (noFirst)
        {
            targetPos += new Vector3(
                (targetPos.x - Player.transform.position.x) / _count,
                0,
                (targetPos.z - Player.transform.position.z) / _count);
        }

        // 投げるオブジェクトの開始位置
        //Projectile.position = transform.position + new Vector3(0, 0.0f, 0);
        transform.position = transform.position + new Vector3(0, 0.0f, 0);

        // 投げるオブジェクトからターゲットまでの距離を計算
        float _targetDistance = Vector3.Distance(transform.position, targetPos);

        // 指定した角度でオブジェクトをターゲットまで投げる時の速度を計算
        float projectile_Velocity = _targetDistance / (Mathf.Sin(2 * _flyingAngle * Mathf.Deg2Rad) / (_gravity * _speed));

        // X軸とY軸での速度をそれぞれ計算
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(_flyingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(_flyingAngle * Mathf.Deg2Rad);

        // 滞空時間を計算
        float flightDuration = _targetDistance / Vx;

        // ターゲットまで投げる時のオブジェクトの回転度合い
        transform.rotation = Quaternion.LookRotation(targetPos - transform.position);

        // 放物線の計算
        float elapse_time = 0;
        while (m_BrotherStateManager.GetState() == BrotherState.THROW)
        {
            transform.Translate(0, (Vy - (_gravity * _speed * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            Debug.DrawRay(transform.position, new Vector3(front.x, transform.position.y, front.z) * 10, Color.black, 0.1f, true);
            if (second)
                yield return SimulateProjectile();

            yield return null;
        }
    }

    //衝撃波
    IEnumerator ShockWave()
    {
        GameObject shockwave = (GameObject)Instantiate(m_ShockWave, EndPos, Quaternion.identity);
        if (StartPos.y - EndPos.y > 2.1f)
        {
            m_Audio.PlayOneShot(m_BrotherStateManager.m_SE[3]);

            particle.GetComponent<ParticleSystem>().startLifetime = 0.04f * (StartPos.y - EndPos.y);
            particle.transform.position = EndPos;
            particle.GetComponent<ParticleSystem>().Play();
            while (_scale < StartPos.y - EndPos.y)
            {
                _scale += 0.5f;
                shockwave.transform.localScale = new Vector3(_scale, shockwave.transform.localScale.y, _scale);
                yield return null;
            }
        }
        else
        {
            m_Audio.PlayOneShot(m_BrotherStateManager.m_SE[2]);
        }
        particle.GetComponent<ParticleSystem>().Stop();
        Destroy(shockwave);
        _scale = 0.0f;

        //speed175...scale9:lifetime0.25=1:0.028
    }

    public void OnCollisionEnter(Collision collision)
    {
        //床判定(判定甘いので要調整)
        if (collision.gameObject.tag == "Floor" && m_BrotherStateManager.GetState() == BrotherState.THROW && GameDatas.isBrotherFlying)
        {
            _count = 2.0f;
            second = false;
            noFirst = false;

            if (transform.position.y >= collision.transform.position.y)
            {
                m_BrotherStateManager.SetState(BrotherState.BACK);
                //GetComponent<AnimationControl>().m_Anim.SetTrigger("land");
                GetComponent<AnimationControl>().m_Anim.SetBool("rotate", false);
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

                //Target.transform.position = new Vector3(reflectPos.x, Target.transform.position.y, reflectPos.z);

                m_Audio.PlayOneShot(m_BrotherStateManager.m_SE[1]);
            }
        }

        //敵判定(バウンド)
        if (collision.gameObject.tag == "Enemy" && m_BrotherStateManager.GetState() == BrotherState.THROW)
        {
            if (collision.gameObject.GetComponent<EnemyBase>().GetEnemyState() != EnemyBase.EnemyState.SUTAN
                && collision.gameObject.GetComponent<EnemyBase>().GetEnemyState() != EnemyBase.EnemyState.GET)
            {
                if (noFirst)
                    _count *= 2.0f;
                second = true;

                collision.gameObject.SendMessage("ChangeState", 3, SendMessageOptions.DontRequireReceiver);
                if (!noFirst)
                    noFirst = true;

                m_Audio.PlayOneShot(m_BrotherStateManager.m_SE[1]);
            }
        }

        //壁判定(跳ね返り)
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

            m_Audio.PlayOneShot(m_BrotherStateManager.m_SE[1]);
        }
    }
}