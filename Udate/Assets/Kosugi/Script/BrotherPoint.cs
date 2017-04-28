using UnityEngine;
using System.Collections;

public class BrotherPoint : MonoBehaviour {

    [SerializeField, TooltipAttribute("弟用の座標オブジェクト")]
    private Transform BrotherPosition;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = BrotherPosition.position;
    }
}
