using UnityEngine;
using System.Collections;

public class SousaSankaku : MonoBehaviour {

    public float speed;//往復速度
    public float range;//移動範囲

    private Vector3 origin;//起点（移動の中心）
    private float myTime = 0.0f;
    
    // Use this for initialization
    void Start()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        myTime += 1 / 60.0f;
        transform.position =
                origin + Vector3.up * Mathf.Sin(myTime * speed) * range;
    }
}
