using UnityEngine;
using System.Collections;

public class AnimationSecond : MonoBehaviour {

    [SerializeField, Header("メインコントローラー")]
    private TitleAnimController _TitleAnimController;

    // Use this for initialization
    void Start () {
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1, "y", 1,  "time", 3));
        //DelayMethodを3秒後に呼び出す
        Invoke("DelayMethod", 3f);
    }
    void DelayMethod()
    {
        //点滅をスタート

    }
    // Update is called once per frame
    void Update () {
	
	}
}
