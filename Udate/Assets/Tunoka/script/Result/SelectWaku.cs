using UnityEngine;
using System.Collections;

public class SelectWaku : MonoBehaviour {

    [SerializeField, Header("SE用弟")]
    public AudioSource _audio;
    public AudioClip _clip01;
    public AudioClip _clip02;

    public SceneChanger _SceneChanger;
    public float SelectNum = 0;
    // Use this for initialization
    void Start () {
        SelectNum = 0;
        transform.localPosition = new Vector3(76, -160, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (SelectNum < 0) return;
        if (SelectNum == 0 && Input.GetButtonDown("XboxB"))
        {
            _audio.PlayOneShot(_clip02);
            SelectNum = -1;
            _SceneChanger.FadeOut("TtesTitle");
        }
        if (SelectNum == 1 && Input.GetButtonDown("XboxB"))
        {
            _audio.PlayOneShot(_clip02);
            SelectNum = -1;
            _SceneChanger.FadeOut("TtestScene01");
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localPosition = new Vector3(76, -160, 0);
            if (SelectNum != 0)
            {
                _audio.PlayOneShot(_clip01);
            }
            SelectNum = 0;

        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localPosition = new Vector3(286, -160, 0);
            if (SelectNum != 1)
            {
                _audio.PlayOneShot(_clip01);
            }
            SelectNum = 1;
        }
    }
}
