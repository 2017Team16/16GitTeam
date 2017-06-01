using UnityEngine;
using System.Collections;

public class AnimationStartTimer : MonoBehaviour {



    [SerializeField, Header("自分の始まる番号")]
    private int _Startnum = 2;
    private TitleAnimController _MainController;//アニメーションのメインコントローラーを取得
    private GameObject _MoveObj;
    // Use this for initialization
    void Start () {
        _MainController = GameObject.Find("TitleController").GetComponent<TitleAnimController>();
        _MoveObj = transform.FindChild("MoveObjs").gameObject;
        _MoveObj.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (_MainController.GetAnimationStep() == _Startnum)
        {
            _MoveObj.SetActive(true);
        }
        else if(_MainController.GetAnimationStep() <= 0)
        { _MoveObj.SetActive(false);
        }
	}
}

