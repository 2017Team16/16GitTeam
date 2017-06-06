using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 p = Camera.main.transform.position;
        p.x = transform.position.x;
        p.y = transform.position.y;
        transform.LookAt(p);
    }
}
