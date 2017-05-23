using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyspawnerEvent : MonoBehaviour {



    [SerializeField, Header("スポーンするまでの時間")]
    private float _SpawnerTime = 25;
    private float _Timer;
    [SerializeField, Header("スポーンする最大数")]
    private float _SpawnerMaxNum = 5;
    [SerializeField, Header("一回に出す数")]
    private float _SpawnerNum = 1;

    [SerializeField, Header("スポーンさせない距離")]
    private float _Distance = 5 ;


    [SerializeField, Header("出てくるタイプ")]
    private Type _EnemyType = Type.Tackle;
    private int _EnemyTypeNum = 0;

    private EmergencePoint _EmergencePoint;

    enum Type
    {
        // タックル
        Tackle,
        // タックル
        Shot, 
        // 巡回
        Patrol,
        //逃げる
        Escape
    }



    // Use this for initialization
    void Start()
    {
        _Timer = 0;
        _EmergencePoint = transform.GetComponent<EmergencePoint>();
        switch (_EnemyType)
        {
            case Type.Tackle: _EnemyTypeNum = 0; break;
            case Type.Shot: _EnemyTypeNum = 1; break;
            case Type.Patrol: _EnemyTypeNum = 2; break;
            case Type.Escape: _EnemyTypeNum = 3; break;
        }
    }
	// Update is called once per frame
	void Update () {
        _Timer += Time.deltaTime;

        if (_Timer >= _SpawnerTime)//指定の時間を過ぎたら
        {
            int EnemyCount = 0;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))//今いる敵をすべて集める
            {
                if (obj.transform.GetComponent<EnemyType>().GetEType() == _EnemyType.ToString())//敵のタイプが一致するのを数える
                {
                    EnemyCount++;
                }
            }
            if (_SpawnerNum + EnemyCount <= _SpawnerMaxNum)//新しく出る数　+　今いる数　が　外に出れる最大数以下だったら
            {
                _EmergencePoint.spawner(_EnemyTypeNum, _SpawnerNum, _Distance);//敵を生成するようにEmergencePointを呼び出す
            }
            else if(EnemyCount < _SpawnerMaxNum)//今いる敵　が　外に出れる最大数以下だったら
            {
                _EmergencePoint.spawner(_EnemyTypeNum, _SpawnerMaxNum - EnemyCount, _Distance);//敵を生成するようにEmergencePointを呼び出す
            }

            _Timer = 0;

        }

	}
}
