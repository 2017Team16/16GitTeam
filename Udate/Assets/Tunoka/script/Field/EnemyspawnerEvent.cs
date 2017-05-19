using UnityEngine;
using System.Collections;

public class EnemyspawnerEvent : MonoBehaviour {



    [SerializeField, Header("スポーンするまでの時間")]
    private float _SpawnerTime = 25;
    private float _Timer;
    [SerializeField, Header("スポーンする数")]
    private float _SpawnerNum = 5;

    [SerializeField, Header("スポーンさせない距離")]
    private float _Distance = 5 ;

    [SerializeField, Header("Fieldにいる敵(確認用)")]
    private GameObject[] _CountEnemys ;


    [SerializeField, Header("出てくるタイプ")]
    private Type _EnemyType = Type.Tackle;


    enum Type
    {
        // タックル
        Tackle,
        // タックル
        Shot,
        //逃げる
        Escape
    }

    public GameObject _Player;


    // Use this for initialization
    void Start () {
        _Timer = 0;


    }
	
	// Update is called once per frame
	void Update () {
        _Timer += Time.deltaTime;

        if (_Timer >= _SpawnerTime)
        {
           

           
        }

        print(Vector3.Distance(_Player.transform.position, transform.position));
	}
}
