using UnityEngine;
using System.Collections;

public class GameMainRule : MonoBehaviour
{

    private OlderBrotherHamster _Player;//プレイヤーの　HPがある場所
    [SerializeField, Header("プレイヤーのHP(確認用)")]
    private float _playerHp;
    [SerializeField, Header("ゲームオーバーのシーンの名前を指定")]
    private string _GameOveraSene;
    [SerializeField, Header("ゲームクリアのシーンの名前を指定")]
    private string _GameClearSene;

    private bool lordCheck;//複数シーンチェンジを行わないようにするもの

    void Start()
    {
        _Player = GameObject.Find("Player").GetComponent<OlderBrotherHamster>();
        _playerHp = _Player.m_Life;
        lordCheck = true;

    }

    // Update is called once per frame
    void Update()
    {
        _playerHp = _Player.m_Life;
        if (_playerHp <= 0)//ゲーム終了条件
        {
            GameClear();
        }
    }

    public void GameClear()//ゲームクリア条件を満たしたら呼び出す
    {
        if (lordCheck == false) return;

        transform.GetComponent<SceneChanger>().FadeOut(_GameClearSene);
        lordCheck = false;
    }
    
}
