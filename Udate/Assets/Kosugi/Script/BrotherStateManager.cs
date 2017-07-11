using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrotherStateManager : MonoBehaviour
{
    /*------内部設定(外部からは弄らないこと)------*/
    [SerializeField, Header("最初の状態")]
    private BrotherState m_StartState = BrotherState.NORMAL;
    [SerializeField, Header("１つ前の状態")]
    private BrotherState m_BeforeBrosState;
    [SerializeField, Header("現在の状態")]
    private BrotherState m_BrosState;
    //各状態中の処理はこの配列へ格納
    private Dictionary<BrotherState, MonoBehaviour> m_Moves;


    /*------外部設定------*/
    [Header("SE")]
    public AudioClip[] m_SE;

    // Use this for initialization
    void Start()
    {
        m_BeforeBrosState = BrotherState.NONE;
        m_BrosState = m_StartState;
        m_Moves = new Dictionary<BrotherState, MonoBehaviour>()
        {
            {BrotherState.BACK, GetComponent<BrotherBack>() },
            {BrotherState.NORMAL, GetComponent<Brother>() },
            {BrotherState.THROW, GetComponent<BrotherThrow>()},
            {BrotherState.SPECIAL, GetComponent<BrotherSpecial>() },
            //{BrotherState.STAGE_CLEAR, GetComponent<StageClearMove>() },
            //{BrotherState.STAGE_FINAL_CLEAR, GetComponent<StageFinalClearMove>() }
        };
    }

    // Update is called once per frame
    void Update()
    {
        print(GameDatas.isBrotherSpecialMove);
        //現在の状態のみを実行
        Action(m_BrosState);

        GetComponent<MeshRenderer>().enabled = false;

        if (!GameDatas.isPlayerLive)
            SetState(BrotherState.DEATH);
    }
    /// <summary>
    /// 指定した状態のみを有効にする
    /// </summary>
    void Action(BrotherState state)
    {
        foreach (KeyValuePair<BrotherState, MonoBehaviour> move in m_Moves)
        {
            if (state == move.Key)
                //move.Valueからだと変更できないみたい
                m_Moves[move.Key].enabled = true;
            else
                m_Moves[move.Key].enabled = false;
        }
    }

    /// <summary>
    /// 状態を変更する
    /// </summary>
    public void SetState(BrotherState state)
    {
        m_BeforeBrosState = m_BrosState;
        m_BrosState = state;

        //同じ状態への変更は行わない
        if (m_BeforeBrosState == m_BrosState) return;

        // 通常→投げ
        if (m_BeforeBrosState == BrotherState.NORMAL &&
             m_BrosState == BrotherState.THROW)
        {
            m_Moves[BrotherState.THROW].GetComponent<BrotherThrow>()._count = 2.0f;
            m_Moves[BrotherState.THROW].GetComponent<BrotherThrow>().ThrowStart();
        }
        //通常→必殺技or投げ→必殺技
        if (m_BeforeBrosState == BrotherState.NORMAL &&
            m_BrosState == BrotherState.SPECIAL)
        {
            m_Moves[BrotherState.SPECIAL].GetComponent<BrotherSpecial>().SpecialSet();
        }
        //
        if (m_BeforeBrosState == BrotherState.THROW && m_BrosState == BrotherState.SPECIAL)
        {
            Destroy(m_Moves[BrotherState.THROW].GetComponent<BrotherThrow>().Target);
            m_Moves[BrotherState.SPECIAL].GetComponent<BrotherSpecial>().SpecialSet();
        }
        //必殺技→通常
        if (m_BeforeBrosState == BrotherState.SPECIAL &&
             m_BrosState == BrotherState.NORMAL)
        {
            m_Moves[BrotherState.SPECIAL].GetComponent<BrotherSpecial>().m_Particle.GetComponent<ParticleSystem>().Stop();

            m_Moves[BrotherState.SPECIAL].GetComponent<BrotherSpecial>()._hit = false;
            m_Moves[BrotherState.SPECIAL].GetComponent<BrotherSpecial>().IsTriggerOff();
        }
        //投げ→着地
        if (m_BeforeBrosState == BrotherState.THROW &&
             m_BrosState == BrotherState.BACK)
        {
            Destroy(m_Moves[BrotherState.THROW].GetComponent<BrotherThrow>().Target);

            m_Moves[BrotherState.BACK].GetComponent<BrotherBack>().Move();
        }
        //着地→通常
        if (m_BeforeBrosState == BrotherState.BACK &&
            m_BrosState == BrotherState.NORMAL)
        {
            m_Moves[BrotherState.BACK].GetComponent<NavMeshAgent>().enabled = false;
        }

        //ANY→通常への変更時
        if (m_BrosState == BrotherState.NORMAL)
        {
            GetComponent<AnimationControl>().m_Anim.GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<AnimationControl>().m_Anim.SetTrigger("wait");
        }

        if(m_BeforeBrosState==BrotherState.BACK&&
            m_BrosState==BrotherState.DEATH)
        {
            GetComponent<NavMeshAgent>().Stop();
        }

        if(m_BeforeBrosState==BrotherState.BACK)
        {
            GetComponent<AnimationControl>().isClimb = false;
        }
    }
    public BrotherState GetState()
    {
        return m_BrosState;
    }
}

