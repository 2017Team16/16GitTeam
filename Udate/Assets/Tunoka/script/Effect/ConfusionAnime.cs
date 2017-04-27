using UnityEngine;
using System.Collections;

public class ConfusionAnime : MonoBehaviour {

    private GameObject _Center;

    [SerializeField, Header("混乱状態の有無")]
    private bool _stunTr;
    [SerializeField, Header("星の回転スピード")]
    private float _speed = 5;

    [SerializeField, Header("自分の親")]
    private GameObject _root;
    private Enemy _Enemy;


    // Use this for initialization
    void Start () {
        if (transform.root.gameObject != null)
        {
            _root = transform.root.gameObject;
            _Enemy = _root.GetComponent<Enemy>();
        }
        _stunTr = false;
        _Center = transform.FindChild("StarCenter").gameObject;
        }
	
	// Update is called once per frame
	void Update () {
        if (_root == null) return;
        transform.position = _root.transform.position;
        transform.eulerAngles = new Vector3(0, 0, 0);
        
        if (_Enemy.GetEnemyState().ToString() == "SUTAN")//スタン状態の確認
        {
            _stunTr = true;
        }
        else
        {
            _stunTr = false;
        }

        _Center.SetActive(_stunTr);//スタン状態の有無で表示する

        if (_stunTr)//スタン状態だったら動かす
        {
            _Center.transform.eulerAngles += new Vector3(Mathf.Sin(Time.time)*0.5f, _speed, 0);
        }

    }
}
