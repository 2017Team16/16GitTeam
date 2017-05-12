using UnityEngine;
using System.Collections;

public class ResultController : MonoBehaviour {


    [SerializeField, Header("確認用スコア(いじらない)")]
    private float _Score;
    [SerializeField, Header("確認用連続チェイン(いじらない)")]
    private float _Chain;
    [SerializeField, Header("確認用一回の最大数(いじらない)")]
    private float _Maxpush;
    [SerializeField, Header("確認用倒した数(いじらない)")]
    private float _Kill;

    // Use this for initialization
    void Start () {
        _Score = Score.getScore();
        _Chain = Score.getChain();
        _Maxpush = Score.getScore();
        _Kill = Score.getKill();
    }
	
	// Update is called once per frame
	void Update () {

	}
}
