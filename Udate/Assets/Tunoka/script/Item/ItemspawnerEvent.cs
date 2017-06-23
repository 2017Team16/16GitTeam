using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemspawnerEvent : MonoBehaviour {

    [SerializeField, Header("出ているアイテムList")]
    private List<int> _SetList = new List<int>();

    [SerializeField, Header("出ているアイテムList2")]
    List<int> _DummyNum = new List<int>();
    [SerializeField, Header("アイテムボックス")]
    private GameObject[] _ItemS;
    [SerializeField, Header("出現頻度")]
    private float[] _ItemSTime;


    [SerializeField, Header("アイテム内の時間")]
    private float _countTime;

    void Start () {
        _countTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        _countTime += Time.deltaTime;

        for (int i = 0 ; i < _ItemS.Length; i++)
        {
            if ((int)_ItemSTime[i] == (int)_countTime)//連続で動かないようにダミーを置く
            {
                _ItemSTime[i] += _ItemSTime[i];//ダミーを次の時間帯に進める

                int rand = ItemPosSet();


                if (rand < 0) return;//置く場所がなかった
                GameObject wreckClone = (GameObject)Instantiate
                 (_ItemS[i],
                  transform.FindChild((rand).ToString()).gameObject.transform.position,
                  Quaternion.identity);
                transform.FindChild((rand).ToString()).transform.GetComponent<ItemEmergenceSab>().setChild(wreckClone);
                ItemSpesSet(transform.FindChild((rand).ToString()).gameObject);
            }
        }
    }
    public void ItemSpesSet(GameObject set)
    {
        _SetList.Add(int.Parse(set.name));
        _SetList.Sort();
    }
    public void ItemSpesNot(GameObject Out)
    {
        for (int i = 0; i < _SetList.Count; i++)
        {
            if (_SetList[i].ToString() == Out.name)
            {
                _SetList.RemoveAt(i);
                _SetList.Sort();
            }
        }
    }
    int ItemPosSet()
    {
        if (transform.childCount <= _SetList.Count) { return -1; }
        _DummyNum.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            _DummyNum.Add(i);
           
        }
        if (_SetList.Count != 0)
        {
            for (int j = _SetList.Count; j > 0; j--)
            {
                _DummyNum.RemoveAt(_SetList[j - 1]);
            }
        }
        return _DummyNum[Random.Range(0, _DummyNum.Count)];
        
    }
}
