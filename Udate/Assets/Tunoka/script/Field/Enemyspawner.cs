using UnityEngine;
using System.Collections;

public class Enemyspawner : MonoBehaviour {

    private GameObject _Type ;
    private float _amount = 1;

   

    private ParticleSystem _particleSystem;

    void Start () {
        _particleSystem = transform.GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!_particleSystem.IsAlive())//Particleが終わったら敵を生成
        {
            EnemyOccurrence();
            Destroy(gameObject);
        }
      
    }
    void EnemyOccurrence()
    {
        for (int i = 0; i < _amount; i ++)
        {
            GameObject wreckClone = (GameObject)Instantiate(_Type, transform.position, Quaternion.identity);
        }
    }
    public void OccurrenceSetting(GameObject tupe , float amount)
    {
        _Type = tupe;
        _amount = amount;
    }

}
