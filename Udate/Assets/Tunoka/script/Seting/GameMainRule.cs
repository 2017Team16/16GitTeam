using UnityEngine;
using System.Collections;

public class GameMainRule : MonoBehaviour
{


    [SerializeField, Header("プレイヤーのHP(確認用)")]
    private float _playerHp;
    [SerializeField, Header("ゲームオーバーのシーンの名前を指定")]
    private string _GameOveraSene;
    [SerializeField, Header("ゲームクリアのシーンの名前を指定")]
    private string _GameClearSene;

    private bool lordCheck;//複数シーンチェンジを行わないようにするもの

    void Start()
    {
        //仮のHP
        _playerHp = 10;
        lordCheck = true;

    }

    // Update is called once per frame
    void Update()
    {
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
