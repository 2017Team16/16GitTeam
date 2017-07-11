using UnityEngine;
using System.Collections;

public class DeathStar : MonoBehaviour {

    /*------外部設定------*/
    [Header("星の回転スピード")]
    private float _speed = 5;


    /*------内部設定------*/
    [Header("弟オブジェクト")]
    private GameObject m_Brother;

    [Header("中心設定用")]
    private GameObject _Center;

    // Use this for initialization
    void Awake()
    {
        m_Brother = GameObject.Find("Brother");

        _Center = transform.FindChild("StarCenter").gameObject;

        _Center.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Brother == null) return;
        transform.position = transform.root.position;

        if (m_Brother.GetComponent<BrotherStateManager>().GetState() == BrotherState.DEATH)
        {
            _Center.gameObject.SetActive(true);
            _Center.transform.eulerAngles += new Vector3(0, _speed, 0);
        }
    }
}