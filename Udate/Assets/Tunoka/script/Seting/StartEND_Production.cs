using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StartEND_Production : MonoBehaviour {


    [SerializeField]
    Sprite[] numberSprites = new Sprite[10];

    [SerializeField]
    Image imagesStart;
    [SerializeField]
    Image imagesEnd;
    [SerializeField]
    GameObject imagesStartObj;
    [SerializeField]
    GameObject imagesEndObj;
    [SerializeField]
    GameObject PauseObj;

    private bool _Start_On = false;
    private bool _End_On = false;

    [SerializeField]
    private float _timer = 0;
    private int _Count = 0;
    // Use this for initialization
    void Start () {
        _Start_On = true;

        _Count = 1;
        _timer = 0;
        imagesStartObj.SetActive(true);
        imagesEndObj.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (_End_On == false && _Start_On == false)
        {
            imagesStartObj.SetActive(false);
        }
      
        if (_Start_On == true)
        {
            Start_Production();
        }

        
    }
    void Start_Production()
    {
        Time.timeScale = 0;
        _timer++;
        if (_timer >= 30 * 1)
        {
            _timer = 0;
            _Count--;
        }
        if (_Count >= 1)
        {
            imagesStart.sprite = numberSprites[4];
        }
        if (_Count <= 0)
        {
            Time.timeScale = 1;
            imagesStart.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);

            imagesStart.color -= new Color(0, 0, 0f,0.05f);
            imagesStart.sprite = numberSprites[5];
            if (_timer > 20)
            {
                PauseObj.SetActive(true);
                _Start_On = false;
            }
        }
      
    }

    public void End_Production( int Time)
    {
        _End_On = true;
        imagesEndObj.SetActive(true);

        imagesEndObj.GetComponent<Animator>().Play("ProductionIme01", 0, 0);
        imagesEnd.sprite = numberSprites[Time];
    }
    public void End_ProductionOff()
    {
        _End_On = false;
        imagesEndObj.SetActive(false);
        imagesEnd.sprite = numberSprites[3];
    }

}
