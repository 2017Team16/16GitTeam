using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrotherStateManager : MonoBehaviour
{

    [SerializeField, TooltipAttribute("最初の状態")]
    private BrotherState m_StartState = BrotherState.NORMAL;
    [SerializeField, TooltipAttribute("１つ前の状態")]
    private BrotherState m_BeforeBrosState;
    [SerializeField, TooltipAttribute("現在の状態")]
    private BrotherState m_BrosState;
    
    //各状態中の処理はこの配列へ格納
    private Dictionary<BrotherState, MonoBehaviour> m_Moves;

    // Use this for initialization
    void Start()
    {
        m_BeforeBrosState = BrotherState.NONE;
        m_BrosState = m_StartState;
        m_Moves = new Dictionary<BrotherState, MonoBehaviour>()
        {
            {BrotherState.WAIT, GetComponent<BrotherWait>() },
            {BrotherState.NORMAL, GetComponent<Brother>() },
            {BrotherState.THROW, GetComponent<BrotherThrow>()},
            //{BrotherState.CANNON_BLOCK, GetComponent<CannonBlockMove>() },
            //{BrotherState.STAGE_CLEAR, GetComponent<StageClearMove>() },
            //{BrotherState.STAGE_FINAL_CLEAR, GetComponent<StageFinalClearMove>() }
        };
    }

    // Update is called once per frame
    void Update()
    {
        //現在の状態のみを実行
        Action(m_BrosState);
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
        if ( m_BeforeBrosState == BrotherState.NORMAL
            && m_BrosState == BrotherState.THROW)
        {
            m_Moves[BrotherState.NORMAL].GetComponent<Brother>()._isFloor = false;
            m_Moves[BrotherState.THROW].GetComponent<BrotherThrow>().m_Target.GetComponent<Renderer>().enabled = true;
            m_Moves[BrotherState.THROW].GetComponent<BrotherThrow>()._count = 2.0f;
            m_Moves[BrotherState.THROW].GetComponent<BrotherThrow>().ThrowStart();

            //バグ用調整
            m_Moves[BrotherState.THROW].GetComponent<BrotherThrow>()._enemyHit = false;
        }
        //投げ→着地
        if (m_BeforeBrosState == BrotherState.THROW
            && m_BrosState == BrotherState.WAIT)
        {
            m_Moves[BrotherState.THROW].GetComponent<BrotherThrow>().m_Target.GetComponent<Renderer>().enabled = false;
            m_Moves[BrotherState.THROW].GetComponent<BrotherThrow>().IsTriggerOff();
            m_Moves[BrotherState.WAIT].GetComponent<BrotherWait>()._isBack = false;
            //m_Moves[BrotherState.WAIT].GetComponent<BrotherWait>()._isMove = false;

            m_Moves[BrotherState.WAIT].GetComponent<BrotherWait>().Move();
        }
        //ANY→通常への変更時
        //if (m_BrosState == BrotherState.NORMAL)
        //{
        //    m_Moves[BrotherState.NORMAL].GetComponent<Brother>()._isFloor = false;
        //}
    }
    public BrotherState GetState()
    {
        return m_BrosState;
    }
}
