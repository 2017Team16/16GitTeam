using UnityEngine;
using System.Collections;

public class EmergencePoint : MonoBehaviour {

    [SerializeField, Header("敵の種類入れ")]
    private GameObject[] _EnemyType;

    private int _Type = 0;//出る敵の種類
    private float _Amount = 1;//出る敵の数
    private float _Pointnum = 1;//"出る場所
    private float _Distance = 5;//スポーンさせない距離

    [SerializeField, Header("出現用Particle(Dustsmoke)")]
    private GameObject _Enemyspawner ;

    public GameObject _Player;
    void Start () {
	
	}
	
	void Update () {

    }
    public void spawner(int type , float amount , float distance)//出る敵の種類 出る敵の数 出る範囲
    {
        _Type     = type;
        _Amount   = amount;
        _Pointnum = Random.Range(1, transform.childCount+1);
        _Distance = distance;


        //プレイヤーが出現ポイント付近にいないか
        if ((Vector3.Distance(_Player.transform.position, transform.FindChild(_Pointnum.ToString()).gameObject.transform.position)) <= _Distance)
        {
            //プレイヤーがいた場合出現ポイントを変更する
            if (transform.childCount <= 1)//ただし他に出現ポイントがなかったらそこで終わる
            {
                return;
            }
            _Pointnum++;//場所を一個ずらす
            if (transform.childCount < _Pointnum )//範囲外になったら最初に戻す
            {
                _Pointnum = 1;
            }
        }
        //敵を出現させる
        GameObject wreckClone = (GameObject)Instantiate
           (_Enemyspawner, 
            transform.FindChild(_Pointnum.ToString()).gameObject.transform.position, 
            Quaternion.identity);
        wreckClone.GetComponent<Enemyspawner>().OccurrenceSetting(_EnemyType[_Type], _Amount);
    }
}
