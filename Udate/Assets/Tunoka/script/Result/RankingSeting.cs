using UnityEngine;
using System.Collections;
using System.IO; 
using System; 
using System.Text;


public class RankingSeting : MonoBehaviour {
    private string guitxt = "";
    // Use this for initialization
    void Start () {
        ReadFile();
    }
	
	// Update is called once per frame
	void Update () {
        print(guitxt);
    }
    void ReadFile()
    {
        // FileReadTest.txtファイルを読み込む
        FileInfo fi = new FileInfo(Application.dataPath + "/Tunoka/Resources/" + "FileReadTest.txt");
        try
        {
            // 一行毎読み込み
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                guitxt = sr.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            // 改行コード
            guitxt += SetDefaultText();
        }
    }

    // 改行コード処理
    string SetDefaultText()
    {
        return "C#あ\n";
    }
}
