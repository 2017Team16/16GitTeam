using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CreditScroller : MonoBehaviour {

    [Header("スクロール速度")]
    public float moveSpeedY = 30.0f;
    [Header("終わり時間線")]
    public float overLine = 1000.0f;
    [SerializeField, Header("フェイドコントローラ")]
    public GameObject fadeObject;
    private FadeScript fadeController;
    private Vector2 position = Vector2.zero;

    private bool isAnyKeyDown;

	// Use this for initialization
	void Start () {
        isAnyKeyDown = false;
        position = transform.position;
        fadeController = fadeObject.GetComponent<FadeScript>();
    }

    // Update is called once per frame
    void Update () {
        position.y += moveSpeedY * Time.deltaTime;
        transform.position = position;

        if (Input.anyKeyDown) isAnyKeyDown = true;

        if (transform.position.y >= overLine || isAnyKeyDown)
        {
            //まずフェイドアウトする
            fadeController.FadeOut();
        }

        //フェイドアウト完了したら遷移へ
        if (fadeController.GetAlpha() >= 1.0f)
        {
            //タイトルシーンを読み込み
            SceneManager.LoadScene(1);
        }

        Debug.Log(fadeController.GetAlpha());
    }

}
