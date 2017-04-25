using UnityEngine;
using System.Collections;

public class EmergencePoint : MonoBehaviour {

    [SerializeField, Header("敵の種類入れ")]
    private GameObject[] _EnemyType;
    [SerializeField, Header("出る敵の種類(_EnemyTypeの配列から選ぶ)")]
    public int _Type = 0;
    [SerializeField, Header("出る敵の数")]
    public float _Amount = 1;
    [SerializeField, Header("出る場所(子にある名前と同じ数値)")]
    public float _Pointnum = 1;

    [SerializeField, Header("出現用Particle(Dustsmoke)")]
    public GameObject _Enemyspawner ;

    void Start () {
	
	}
	
	void Update () {
        //デバック用=======================================================================
        if (Input.GetKeyDown(KeyCode.O))
        {
            spawner(0,1,1);
        }
        //=================================================================================

    }
    void spawner(int type , float amount , float pointnum)//出る敵の種類 出る敵の数 出る場所
    {
        _Type     = type;
        _Amount   = amount;
        _Pointnum = pointnum;


        if (transform.childCount < _Pointnum || _Pointnum <= 0)//ポインち場所
        {
            print("そのポイントは存在しないよ"); 
            return;
        }

        GameObject wreckClone = (GameObject)Instantiate
           (_Enemyspawner, 
            transform.FindChild(_Pointnum.ToString()).gameObject.transform.position, 
            Quaternion.identity);
        wreckClone.GetComponent<Enemyspawner>().OccurrenceSetting(_EnemyType[_Type], _Amount);
    }
}
