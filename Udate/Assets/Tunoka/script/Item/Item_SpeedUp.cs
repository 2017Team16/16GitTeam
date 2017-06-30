using UnityEngine;
using System.Collections;

public class Item_SpeedUp : MonoBehaviour {
    [SerializeField, Header("SE用")]
    public AudioSource _audio;
    public AudioClip _clip01;

    [SerializeField, Header("上がる値")]
    private float _SpeedUp = 1;

    [SerializeField, Header("回転速度")]
    private float _speed = 1;

    private Transform _ime;
    // Use this for initialization
    void Start()
    {
        _ime = transform.FindChild("Item-Heel").gameObject.transform;
        
        _audio = GameObject.Find("SE").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
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
            collider.transform.GetComponent<OlderBrotherHamster>().DefaultSpeedWalk();
            transform.GetComponent<ItemGetEffect>().MoveCor_(GameObject.FindGameObjectWithTag("Player").transform.position);
        }

    }
}
