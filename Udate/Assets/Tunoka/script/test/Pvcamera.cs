using UnityEngine;
using System.Collections;

public class Pvcamera : MonoBehaviour {

    public GameObject camera;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.T))
        {
            print("TTT");
            iTween.MoveTo (camera, iTween.Hash ("x", transform.position.x, "z", transform.position.z - 1, "y", transform.position.y, "time", 3));
            camera.transform.parent = transform;
        }
	
	}
}
