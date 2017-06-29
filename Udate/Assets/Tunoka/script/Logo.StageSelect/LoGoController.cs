using UnityEngine;
using System.Collections;

public class LoGoController : MonoBehaviour {

    private SceneChanger SChang;
    // Use this for initialization
    void Start () {

        SChang = transform.GetComponent<SceneChanger>();
        Invoke("FadeIn", 4);

        Cursor.visible = false;
    }
    void FadeIn() { 
        SChang.FadeOut(); 
    }
    // Update is called once 
    void Update ()
    {
    }
}
