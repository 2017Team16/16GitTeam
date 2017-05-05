using UnityEngine;
using System.Collections;

public class FallingText : MonoBehaviour {

    private Vector3 _TargetPos;//自分が最終的に到達する地点
    [SerializeField, Header("止まるまでの時間")]
    private float _FallingTime = 4;
    [SerializeField, Header("始まるまでのずれ")]
    private float _delayTime = 0;

    // Use this for initialization
    void Start () {
        _TargetPos = transform.position;//始まりの場所を到着地点にする
        transform.position = transform.FindChild("StartPos").transform.position;//自分の子にある始まりのPositionに移動
        Invoke("Bounce", _delayTime);
    }
	
	// Update is called once per frame
	void Update () {
        	
	}
    void Bounce()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", _TargetPos.y, "time", _FallingTime, "EaseType", iTween.EaseType.easeOutBounce));
    }
}
