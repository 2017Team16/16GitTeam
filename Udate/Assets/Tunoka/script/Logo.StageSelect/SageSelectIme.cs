using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SageSelectIme : MonoBehaviour {

    private StageSelectController _StageSelectController;
    [SerializeField, Header("自分の番号")]
    private int _MyNumber;
    private int _RootNumber;
    [SerializeField, Header("元の画像")]
    private Sprite _protoIme;
    [SerializeField, Header("変更後の画像")]
    private Sprite _ChangeIme;

    // Use this for initialization
    void Start () {
        _StageSelectController = GameObject.Find("StageSelectController").GetComponent<StageSelectController>();

    }
	
	// Update is called once per frame
	void Update () {
        _RootNumber = _StageSelectController.GetCursorNum();//コントローラーのカーソルのいる番号を持ってくる

        if (_RootNumber == _MyNumber)
        {
            transform.GetComponent<Image>().sprite = _ChangeIme;
            return;
        }
        transform.GetComponent<Image>().sprite = _protoIme;

    }
}
