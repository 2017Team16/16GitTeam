using UnityEngine;
using System.Collections;

public class XconTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Horizontal") > 0)
        {
            print("ステックが右に倒された");
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            print("ステックが左に倒された");
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            print("ステックが上に倒された");
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            print("ステックが下に倒された");
        }

        if (Input.GetButton("XboxA"))
        {
            print("Aボタン");
        }
        if (Input.GetButton("XboxB"))
        {
            print("Bボタン");
        }
        if (Input.GetButton("XboxX"))
        {
            print("Cボタン");
        }
        if (Input.GetButton("XboxL1"))
        {
            print("L1ボタン");
        }
    }
}
//Fire1