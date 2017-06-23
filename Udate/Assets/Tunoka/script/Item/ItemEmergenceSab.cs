using UnityEngine;
using System.Collections;

public class ItemEmergenceSab : MonoBehaviour {

    private ItemspawnerEvent _ItemspawnerEvent;
    private GameObject _ChildName;

    // Use this for initialization
    void Start () {
        _ItemspawnerEvent = transform.root.gameObject.GetComponentInChildren<ItemspawnerEvent>();
        _ChildName = transform.gameObject;

    }
	
	// Update is called once per frame
	void Update () {
        if (transform.childCount <= 0 && _ChildName != transform.gameObject)
        {
            _ItemspawnerEvent.ItemSpesNot(transform.gameObject);
            _ChildName = transform.gameObject;
        }
	}
    public void setChild(GameObject set)
    {
        _ChildName = set;
        set.transform.parent = transform.transform;
    }
}
