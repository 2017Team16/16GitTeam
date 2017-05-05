using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {


    private TitleAnimController _TAnim;
    private SceneChanger SChang;

    // Use this for initialization
    void Start () {
        _TAnim = transform.GetComponent<TitleAnimController>();
        SChang = transform.GetComponent<SceneChanger>();
    }
	
	// Update is called once per frame
	void Update () {
        print(_TAnim.GetAnimeFlag());
        if (_TAnim.GetAnimeFlag() && Input.GetKeyDown(KeyCode.Space))
        {
            print("シーンチェンジだよ");
            SChang.FadeOut();
        }
    }

    
}
