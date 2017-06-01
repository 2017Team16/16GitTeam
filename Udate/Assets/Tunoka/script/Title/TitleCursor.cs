using UnityEngine;
using System.Collections;

public class TitleCursor : MonoBehaviour {


    public TitleController _Tcontroller;
    private float _CursorNum;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        _CursorNum = _Tcontroller._CursorNum;

        if (_CursorNum == 0)
        {
            transform.localPosition = new Vector3(-130, -30, 0);
        }
        else if (_CursorNum == 1)
        {
            transform.localPosition = new Vector3(-130, -110, 0);

        }
    }
}
