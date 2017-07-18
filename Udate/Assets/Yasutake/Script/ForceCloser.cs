using UnityEngine;
using System.Collections;

public class ForceCloser : MonoBehaviour
{

    // 操作が無い場合に自動で終了するまでの秒数
    static readonly float AutoCloseDuration = 30;

    // 最後に入力があった時間
    float lastInputTime;

    void Awake()
    {
        if (FindObjectsOfType<ForceCloser>().Length > 1)
        {
            // 自分の他に ForceCloser が既に存在する場合は、自分を消す
            Destroy(gameObject);
            return;
        }
        // 全シーンにまたがって生存する(シーン切り替えで破棄されない)ようにする
        DontDestroyOnLoad(gameObject);
        lastInputTime = Time.time;
    }

    void Update()
    {
        lastInputTime += 1 / 60.0f;
        // XBOX コントローラーの BACK ボタンと START ボタン同時押し
        // または
        // キーボードのエスケープキーでゲームを強制終了する
        if ((Input.GetKey(KeyCode.JoystickButton6) && Input.GetKey(KeyCode.JoystickButton7))
        || Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }
        // 何かしらの入力があれば、最後に入力があった時間として現在の時間を記録する
        if (InputAnyKey())
        {
            //lastInputTime = Time.time;
            lastInputTime = 0.0f;
        }
        // 最後に入力があった瞬間からの経過時間が、指定秒数を超えたら、強制終了する
        if (//(Time.time - lastInputTime) >= AutoCloseDuration
            lastInputTime >= AutoCloseDuration)
        {
            Application.Quit();
            return;
        }
    }

    // 何らかの入力があれば true、無ければ false を返却する
    bool InputAnyKey()
    {
        return Input.anyKey ||
        Mathf.Abs(Input.GetAxis("Horizontal")) > 0.3f ||
        Mathf.Abs(Input.GetAxis("Vertical")) > 0.3f ||
        Mathf.Abs(Input.GetAxis("Mouse X")) > 0 ||
        Mathf.Abs(Input.GetAxis("Mouse Y")) > 0 ||
        Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0;
    }
}
