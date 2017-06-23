using UnityEngine;
using System.Collections;

public class DeathStar : MonoBehaviour {

    /*--外部設定オブジェクト--*/
    [SerializeField, Header("弟オブジェクト")]
    private GameObject m_Brother;


    /*------外部設定変数------*/
    [SerializeField, Header("星の回転スピード")]
    private float _speed = 5;


    /*------内部設定変数------*/
    [Header("中心設定用")]
    private GameObject _Center;

    [SerializeField, Header("デバッグ用")]
    private bool debug;

    // Use this for initialization
    void Start()
    {
        _Center = transform.FindChild("StarCenter").gameObject;

        _Center.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Brother == null) return;
        transform.position = transform.root.position;

        if (m_Brother.GetComponent<BrotherStateManager>().GetState() == BrotherState.DEATH||debug)
        {
            _Center.gameObject.SetActive(true);
            _Center.transform.eulerAngles += new Vector3(0, _speed, 0);
        }
    }
}