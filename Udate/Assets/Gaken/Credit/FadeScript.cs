using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FadeScript : MonoBehaviour {

    public float speed = 0.01f;          //透明化の速さ
    private float alpha;                 //A値を操作するための変数
    private float red, green, blue;      //RGBを操作するための変数

    // Use this for initialization
    void Start () {
        //Panelの色を取得
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void FadeOut()
    {
        GetComponent<Image>().color = new Color(red, green, blue, alpha);
        alpha += speed;
    }

    public float Alpha
    {
        get
        {
            return alpha;
        }
    }
}
