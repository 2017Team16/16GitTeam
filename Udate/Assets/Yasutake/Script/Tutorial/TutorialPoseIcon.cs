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

    [SerializeField, Header("往復速度")]
    private float speed = 5;
    [SerializeField, Header("移動範囲")]
    private float range = 5;

    private Vector3 origin;//起点（移動の中心）
    private float myTime = 0.0f;

    void Start()
    {
        _StateSize = transform.localScale;

        origin = transform.position;
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
            myTime += 1 / 60.0f;
            
        }
        else
        {
            transform.localScale = _StateSize;
            transform.GetComponent<Image>().sprite = _ImeS[0];
            myTime = 0.0f;
        }
        transform.position =
                        origin + Vector3.up * Mathf.Sin(myTime * speed) * range;

    }
}
