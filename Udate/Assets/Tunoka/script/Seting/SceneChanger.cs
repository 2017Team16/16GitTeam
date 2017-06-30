using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    [SerializeField, Header("次のシーンの名前")]
    public string _NextSceneName;

    [SerializeField, Header("フェード用の黒い壁")]
    private GameObject _BlackImage;

    [SerializeField, Header("変わるまでの時間")]
    private int _fadeTime = 2;
    bool m_tr = true;

    [SerializeField, Header("音楽")]
    private GameObject[] _SaundObj;

    [SerializeField, Header("FadeInをやるかどうか")]
    private bool FadeOff = false;
    void Awake()
    {
        _BlackImage.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 255);
    }
    void Start()
    {
        if (FadeOff == true) return;
        FadeIn();
    }
    public void HalfFadeIn()
    {
        // SetValue()を毎フレーム呼び出して、１秒間に０から0.5までの値の中間値を渡す
        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 0.9f, "time", _fadeTime, "onupdate", "SetValue"));
    }
   
    public void FadeIn()
    {
        // SetValue()を毎フレーム呼び出して、１秒間に１から０までの値の中間値を渡す
        iTween.ValueTo(gameObject, iTween.Hash("from", 1f, "to", 0f, "time", _fadeTime, "onupdate", "SetValue"));
        m_tr = false;
    }
    
    public void FadeOut()
    {
        m_tr = true;
        // SetValue()を毎フレーム呼び出して、１秒間に０から１までの値の中間値を渡す
        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1f, "time", _fadeTime, "onupdate", "SetValue"));
    }

    public void FadeOut(string name)
    {
        _NextSceneName = name;
        m_tr = true;
        // SetValue()を毎フレーム呼び出して、１秒間に０から１までの値の中間値を渡す
        iTween.ValueTo(gameObject, iTween.Hash("from", 0f, "to", 1f, "time", _fadeTime, "onupdate", "SetValue"));
    }

    public void PauseFadeOut(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    void SetValue(float alpha)
    {

        if (_SaundObj != null)//BGMがあればBGMもフードインアウトする
        {
            float volume = 1 - alpha;
            foreach (GameObject value in _SaundObj)
            {
                value.GetComponent<AudioSource>().volume = volume;
            }
        }

        _BlackImage.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, alpha);
        if (alpha >= 1 && m_tr) SceneChange(_NextSceneName);

    }
    void SceneChange(string name)
    {
        print(name + "にシーンチェンジ");
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
