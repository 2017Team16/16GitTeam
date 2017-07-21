using UnityEngine;
using System.Collections;

public class Sweat : MonoBehaviour {

    private Vector3 move;
    private float time;

	// Use this for initialization
	void Start () {
        int r = Random.Range(0, 2);
        float r2 = Random.Range(1.0f, 2.0f);
        float r3 = Random.Range(10.0f, 80.0f);
        float r4 = Random.Range(0.5f, 1.2f);
        float r5 = Random.Range(0.5f, 1.2f);

        if (r == 0)
        {
            transform.localPosition = new Vector3(1, 1.5f, 0);
            transform.localScale = new Vector3(-1, 1, 1);
            transform.Rotate(new Vector3(0, 0, r3));
            move = new Vector3(r4, -r5, 0);
        }
        else
        {
            transform.localPosition = new Vector3(-1, 1.5f, 0);
            transform.Rotate(new Vector3(0, 0, -r3));
            move = new Vector3(-r4, -r5, 0);
        }
        transform.localScale *= r2;
        time = Time.time;

	}
	
	// Update is called once per frame
	void Update () {
        transform.position += move * Time.deltaTime;
        if(time + 0.5f < Time.time)
        {
            Destroy(gameObject);
        }
	}
}
