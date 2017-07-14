using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedUpGauge : MonoBehaviour {

    private OlderBrotherHamster player;
    private float limit;
    private Image myImage;
    private Image backImage;
    private RectTransform mytrans;
    private RectTransform backtrans;
    private float xpos;

    // Use this for initialization
    void Start () {
        player = transform.parent.transform.parent.GetComponent<OlderBrotherHamster>();
        limit = 0.0f;
        myImage = transform.GetComponent<Image>();
        backImage = transform.parent.FindChild("SpeedUpGaugeFrame").transform.GetComponent<Image>();
        mytrans = gameObject.GetComponent<RectTransform>();
        backtrans = transform.parent.FindChild("SpeedUpGaugeFrame").GetComponent<RectTransform>();
        xpos = mytrans.localPosition.x;

    }
	
	// Update is called once per frame
	void Update () {
        if(player == null)return;
        limit = player.GetSpeedUpTime();

        if(limit == 0.0f)
        {
            myImage.enabled = false;
            backImage.enabled = false;
        }
        else
        {
            myImage.enabled = true;
            backImage.enabled = true;
            
            myImage.fillAmount = limit;

            if (player.GetPlayerLR())
            {
                mytrans.localPosition = new Vector2(-xpos,0);
                backtrans.localPosition = new Vector2(-xpos, 0);
            }
            else
            {
                mytrans.localPosition = new Vector2(xpos, 0);
                backtrans.localPosition = new Vector2(xpos, 0);
            }
        }
    }
}
