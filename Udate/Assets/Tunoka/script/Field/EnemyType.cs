using UnityEngine;
using System.Collections;

public class EnemyType : MonoBehaviour {

    enum Type
    {
        // タックル
        Tackle,
        // タックル
        Shot,
        //逃げる
        Escape
    }

    [SerializeField, Header("自分のタイプ")]
    private Type _EnemyType = Type.Tackle;
    private string _Type;//ダミー用エネミータイプ

    // Use this for initialization
    void Start () {
        _Type = _EnemyType.ToString();

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public string GetEType()
    {
        return _Type;
    }

}
