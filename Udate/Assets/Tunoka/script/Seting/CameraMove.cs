using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {


    [SerializeField, Header("カメラがプレイヤーを追尾するならオン")]
    private bool _MoveOn = false;
    [SerializeField, Header("プレイヤー以外を追尾するならここに追尾先を入れる")]
    private GameObject _MoveTarget ;


    // Use this for initialization
    void Start () {
        if (_MoveTarget == null)
        {
            _MoveTarget = GameObject.FindWithTag("Player");
        }
	
	}
	
	// Update is called once per frame
	void Update () {

        if (_MoveOn == false || _MoveTarget == null) return;
        transform.position = _MoveTarget.transform.position;
       
    }


}
