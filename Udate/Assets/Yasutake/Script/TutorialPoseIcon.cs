using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialPoseIcon : MonoBehaviour {
    [SerializeField, Header("ResultEvent")]
    private TutorialPose _Pause;

    [SerializeField, Header("画像リスト")]
    private Sprite[] _ImeS;

    [SerializeField, Header("表示番号")]
    public int _ListNum = 0;

    [SerializeField, Header("自分の番号")]
    private int MyNum = 0;
    [SerializeField, Header("サイズ変更が必要かどうか")]
    private bool _ChangeSizeTr;
    [SerializeField, Header("変更サイズ")]
    private Vector3 _ChangeSize;
    private Vector3 _StateSize;
    void Start()
    {
        _StateSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {




        _ListNum = _Pause._ListNum;
        if (_ListNum >= 3)
        {
            print("サイズ外を指定外か終了しています");
            return;
        }




        if (_ListNum == MyNum)
        {
            if (_ChangeSizeTr == true)//サイズ調整が必要な画像
            {
                transform.localScale = _ChangeSize;
            }
            transform.GetComponent<Image>().sprite = _ImeS[1];
        }
        else
        {
            transform.localScale = _StateSize;
            transform.GetComponent<Image>().sprite = _ImeS[0];
        }
    }
}
