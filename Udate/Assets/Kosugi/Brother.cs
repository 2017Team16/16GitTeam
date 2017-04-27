﻿using UnityEngine;
using System.Collections;     

public class Brother : MonoBehaviour {

    private Transform Player;
    public bool _isFloor;

    //弟管理クラス
    private BrotherStateManager m_BrotherStateManager;
    
    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player").transform;

        m_BrotherStateManager = GetComponent<BrotherStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.FindChild("Point").gameObject.transform.position;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_BrotherStateManager.SetState(BrotherState.THROW);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _isFloor = true;
        }
    }
}
