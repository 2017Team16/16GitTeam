using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Resultback : MonoBehaviour {
    [SerializeField, Header("選ばれたStage ")]
    private int StageNum;
    [SerializeField, Header("画像リスト")]
    private Sprite[] _ImeS;
    // Use this for initialization
    void Start () {
        StageNum = StageSelectController.getStageNum();
        if (StageNum == 0) StageNum = 1;
        transform.GetComponent<Image>().sprite = _ImeS[StageNum-1];
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
