using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

    private bool _pauseTr;
    [SerializeField]
    private GameObject _MoveObj;
    // Use this for initialization
    void Start () {
        _pauseTr = false;
        _MoveObj.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (_pauseTr == true)
        {
            _MoveObj.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            _MoveObj.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
