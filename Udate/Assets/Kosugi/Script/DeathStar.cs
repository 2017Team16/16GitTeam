using UnityEngine;
using System.Collections;

public class DeathStar : MonoBehaviour {

    private GameObject _Center;

    [SerializeField, Header("星の回転スピード")]
    private float _speed = 5;

    [SerializeField, Header("弟オブジェクト")]
    private GameObject m_Brother;

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
        //transform.eulerAngles = new Vector3(0, 0, 0);

        if (m_Brother.GetComponent<BrotherStateManager>().GetState() == BrotherState.DEATH||debug)
        {
            _Center.gameObject.SetActive(true);
            _Center.transform.eulerAngles += new Vector3(/*Mathf.Sin(Time.time) * 0.5f*/0, _speed, 0);
        }

    }
}