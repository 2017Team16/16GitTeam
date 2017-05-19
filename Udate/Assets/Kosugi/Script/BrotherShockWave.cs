﻿using UnityEngine;
using System.Collections;

public class BrotherShockWave : MonoBehaviour {

    //親(弟)が持っている弟管理クラス
    private BrotherStateManager m_BrotherStateManager;

    // Use this for initialization
    void Start () {
        m_BrotherStateManager = GameObject.Find("Brother").GetComponent<BrotherStateManager>();
    }
	
	// Update is called once per frame
	void Update () {

	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Enemy" && m_BrotherStateManager.GetState() == BrotherState.THROW)
        {
            print("!");
            other.gameObject.SendMessage("ChangeState", 3, SendMessageOptions.DontRequireReceiver);
        }
    }
}