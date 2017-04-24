using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //デバック用=======================================================================
        if (Input.GetKey(KeyCode.Space))
        {
            transform.GetComponent<SceneChanger>().FadeOut();
        }
        //=================================================================================
    }
}
