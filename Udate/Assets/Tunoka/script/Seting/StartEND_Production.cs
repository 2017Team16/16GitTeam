using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StartEND_Production : MonoBehaviour {

    [SerializeField, Header("SE用弟")]
    public AudioSource _audio;
    public AudioClip _clip01;
    public AudioClip _clip02;
    public AudioClip _clip03;
    [SerializeField]
    Sprite[] numberSprites = new Sprite[10];

    [SerializeField]
    Image images;
    [SerializeField]
    GameObject imagesObj;

    private bool _Start_On = false;
    private bool _End_On = false;

    private float _timer = 0;
    private int _Count = 0;
    // Use this for initialization
    void Start () {
        _Start_On = true;


        _Count = 1;
        _timer = 0;
        Time.timeScale = 0;
        imagesObj.GetComponent<Animator>().Play("ProductionIme00", 0, 0);
        imagesObj.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if (_End_On == false && _Start_On == false)
        {
            imagesObj.SetActive(false);
        }
      
        if (_Start_On == true)
        {
            Start_Production();
        }

        
    }
    void Start_Production()
    {
        _timer++;
        print(_timer);
        if (_timer >= 60 * 1)
        {
            print(_Count);
            _timer = 0;
            _Count--;
        }
        if (_Count >= 1)
        {
            images.sprite = numberSprites[4];
            imagesObj.GetComponent<Animator>().Play("ProductionIme01", 0, 0);
        }
        if (_Count <= 0)
        {
            Time.timeScale = 1;
            images.sprite = numberSprites[5];
            if (_timer > 20)
            {
                _Start_On = false;
            }
        }
      
    }

    public void End_Production( int Time)
    {
        _End_On = true;
        imagesObj.SetActive(true);

        imagesObj.GetComponent<Animator>().Play("ProductionIme01", 0, 0);
        images.sprite = numberSprites[Time];
    }

}
