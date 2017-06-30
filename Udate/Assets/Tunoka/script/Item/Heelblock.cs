using UnityEngine;
using System.Collections;

public class Heelblock : MonoBehaviour {

    [SerializeField, Header("SE用")]
    public AudioSource _audio;
    public AudioClip _clip01;

    [SerializeField, Header("回復値")]
    private float _Heel = 1;

    [SerializeField, Header("回転速度")]
    private float _speed = 1;

    private Transform _ime;

    // Use this for initialization
    void Start ()
    {
        //_ime = transform.FindChild("Item-Heel").gameObject.transform;
        //_ime.eulerAngles = new Vector3(0f, 270, 180f);
        _audio = GameObject.Find("SE").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //_ime.eulerAngles += new Vector3(0f, _speed, 0f);
        //if (_ime.eulerAngles.y >= 90 && _ime.eulerAngles.y <= 270)
        //{
        //    _ime.eulerAngles = new Vector3(0f, 270, 180f);
        //}


    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            _audio.PlayOneShot(_clip01);
            collider.transform.GetComponent<OlderBrotherHamster>().AddLife((int)2);

            transform.GetComponent<ItemGetEffect>().MoveCor_(transform.GetComponent<ItemGetEffect>().GetCanvas().transform.FindChild("Hp").position);
        }

    }

}
