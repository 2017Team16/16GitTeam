using UnityEngine;
using System.Collections;

public class PlayerSoul : MonoBehaviour {

    public float speed;
    public float range;

    private Vector3 origin;

	// Use this for initialization
	void Start () {
        origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = origin + Vector3.right * Mathf.Sin(Time.time * speed) * range;
        origin += Vector3.up / 15.0f;
	}
}
