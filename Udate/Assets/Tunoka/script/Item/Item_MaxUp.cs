﻿using UnityEngine;
using System.Collections;

public class Item_MaxUp : MonoBehaviour {


    [SerializeField, Header("回復値")]
    private float _Heel = 2;

    [SerializeField, Header("回転速度")]
    private float _speed = 1;

    private Transform _ime;
    // Use this for initialization
    void Start ()
    {
        _ime = transform.FindChild("Item-MaxUp").gameObject.transform;
        _ime.eulerAngles = new Vector3(0f, 270, 180f);
    }

    // Update is called once per frame
    void Update()
    {
        _ime.eulerAngles += new Vector3(0f, _speed, 0f);
        if (_ime.eulerAngles.y >= 90 && _ime.eulerAngles.y <= 270)
        {
            _ime.eulerAngles = new Vector3(0f, 270, 180f);
        }


    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            collider.transform.GetComponent<OlderBrotherHamster>().AddMaxLife();
            Destroy(transform.gameObject);
        }

    }

}