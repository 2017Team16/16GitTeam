using UnityEngine;
using System.Collections;

public class SelectWaku : MonoBehaviour {

    public SceneChanger _SceneChanger;
    public float SelectNum = 0;
    // Use this for initialization
    void Start () {
        SelectNum = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (SelectNum == 0 && Input.GetButtonDown("XboxB"))
        {
            _SceneChanger.FadeOut("TtesTitle");
        }
        if (SelectNum == 1 && Input.GetButtonDown("XboxB"))
        {
            _SceneChanger.FadeOut(" TtestScene01");
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localPosition = new Vector3(76, -160, 0);
            SelectNum = 0;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localPosition = new Vector3(286, -160, 0);
            SelectNum = 1;
        }
    }
}
