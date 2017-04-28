using UnityEngine;
using System.Collections;

public class BrotherBack : MonoBehaviour
{
    private Transform Player;
    //移動速度
    public float _speed = 5;

    public bool _isBack = false;
    //public bool _isMove = false;

    //弟管理クラス
    private BrotherStateManager m_BrotherStateManager;

    void Start()
    {
        Player = GameObject.Find("Player").transform;

        m_BrotherStateManager = GetComponent<BrotherStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)&&!_isMove)
        //{
        //    StartCoroutine(GoBack()); 
        //}
    }

    public void Move()
    {
        StartCoroutine(GoBack());
    }

    IEnumerator GoBack()
    {
        yield return new WaitForSeconds(0.1f);

        //_isMove = true;
        while (!_isBack)//elapse_time < flightDuration)
        {
            //方向
            Vector3 _direction = Player.position - transform.position;
            //単位化（距離要素を取り除く）
            _direction = _direction.normalized;
            transform.position = transform.position + (_direction * _speed * Time.deltaTime);
            //プレイヤーの方を向く
            transform.LookAt(Player);

            yield return null;

            //ダッシュ用
            if(Input.GetKeyDown(KeyCode.B))
            {

            }
        }
        m_BrotherStateManager.SetState(BrotherState.NORMAL);
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == ("Player") && m_BrotherStateManager.GetState() == BrotherState.BACK)
        {
            _isBack = true;
        }
    }
}
