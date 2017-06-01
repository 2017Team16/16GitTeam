﻿using UnityEngine;
using System.Collections;

public class BrotherBack : MonoBehaviour
{
    [SerializeField, TooltipAttribute("プレイヤーオブジェクト")]
    private GameObject Player;

    //移動速度
    public float _speed = 5;

    //public bool _isBack = false;
    //public bool _isMove = false;

    NavMeshAgent m_Nav;

    //弟管理クラス
    private BrotherStateManager m_BrotherStateManager;

    void Start()
    {
        m_Nav = GetComponent<NavMeshAgent>();

        m_BrotherStateManager = GetComponent<BrotherStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_BrotherStateManager.GetState() == BrotherState.BACK)
        {
            m_Nav.destination = Player.transform.position;
            //if (_isBack)
            //    m_BrotherStateManager.SetState(BrotherState.NORMAL);
        }
    }

    public void Move()
    {
        StartCoroutine(GoBack());
    }

    IEnumerator GoBack()
    {
        yield return new WaitForSeconds(0.1f);

        yield return null;

        //_isMove = true;
        //while (!_isBack)//elapse_time < flightDuration)
        //{
        //    //方向
        //    Vector3 _direction = Player.position - transform.position;
        //    //単位化（距離要素を取り除く）
        //    _direction = _direction.normalized;
        //    transform.position = transform.position + (_direction * _speed * Time.deltaTime);
        //    //プレイヤーの方を向く
        //    transform.LookAt(Player);

        //    yield return null;

        //    //ダッシュ用
        //    if (Input.GetKeyDown(KeyCode.B))
        //    {

        //    }
        //}
        //m_BrotherStateManager.SetState(BrotherState.NORMAL);
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == ("Player") && m_BrotherStateManager.GetState() == BrotherState.BACK)
        {
            m_BrotherStateManager.SetState(BrotherState.NORMAL);
        }
    }
}
