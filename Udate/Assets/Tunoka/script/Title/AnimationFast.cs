using UnityEngine;
using System.Collections;


public class AnimationFast : MonoBehaviour {

    [SerializeField, Header("メインコントローラー")]
    private TitleAnimController _TitleAnimController;

    // Use this for initialization
    void Start () {
        iTween.ScaleTo(gameObject, iTween.Hash("x", 1, "y", 1, "delay", 3f, "time", 3));
        //DelayMethodを6秒後に呼び出す
        Invoke("DelayMethod", 6f);
    }
 

    void Update () {

    }
    void DelayMethod()
    {
        _TitleAnimController.NextAnimationstep();
    }
}
