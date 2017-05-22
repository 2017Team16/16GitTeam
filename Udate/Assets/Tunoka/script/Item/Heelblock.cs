using UnityEngine;
using System.Collections;

public class Heelblock : MonoBehaviour {


    [SerializeField, Header("回復値")]
    private float _Heel = 1;

    [SerializeField, Header("回転速度")]
    private float _speed = 1;


    // Use this for initialization
    void Start ()
    {
        transform.eulerAngles = new Vector3(0f, 270, 180f);

    }
	
	// Update is called once per frame
	void Update () {
        transform.eulerAngles += new Vector3(0f, _speed, 0f);
        if (transform.eulerAngles.y >= 90 && transform.eulerAngles.y <= 270)
        {
            transform.eulerAngles = new Vector3(0f, 270, 180f);
        }


    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            Destroy(transform.gameObject);
            //collider.transform.GetComponent<OlderBrotherHamster>().AddLife(_Heel);
        }

    }
 

}
