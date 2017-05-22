using UnityEngine;
using System.Collections;

public class ItemspawnerEvent : MonoBehaviour {


    [SerializeField, Header("ノーマル回復Item時間")]
    private GameObject _HeelItem01;
    [SerializeField, Header("出現頻度")]
    private float Item01Time = 60;
    private float Item01Dummy; 

    private float _countTime;
    void Start () {
        _countTime = 0;
        Item01Dummy = Item01Time;

    }
	
	// Update is called once per frame
	void Update () {
        _countTime += Time.deltaTime;

   
        //Item01の生成処理
        if ((int)_countTime == Item01Dummy)//連続で動かないようにダミーを置く
        {
            Item01Dummy += Item01Time;//ダミーを次の時間帯に進める
            GameObject wreckClone = (GameObject)Instantiate
             (_HeelItem01,
              transform.FindChild((Random.Range(1,4)).ToString()).gameObject.transform.position,
              Quaternion.identity);
        }



    }
}
