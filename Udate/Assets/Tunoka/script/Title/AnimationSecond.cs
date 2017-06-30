using UnityEngine;
using System.Collections;

public class AnimationSecond : MonoBehaviour {

    [SerializeField, Header("メインコントローラー")]
    private TitleAnimController _TitleAnimController;

    [SerializeField, Header("リーダーかどうか")]
    private bool _Animereader = true;

    // Use this for initialization
    void Start () {

        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 0.9f, "time", 0.5f, "onupdate", "SetValue"));
        if (_Animereader == true)
        {
            Invoke("DelayMethod", 3f);
        }
    }
    void SetValue(float alpha)
    {
        transform.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(225, 255, 255, alpha);
    }
    void DelayMethod()
    {
        _TitleAnimController.NextAnimationstep();
    }
    // Update is called once per frame
    void Update () {
	
	}
}
