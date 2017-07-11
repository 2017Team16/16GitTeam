using UnityEngine;
using System.Collections;

public class TutorialPlayerUI : MonoBehaviour {

    private TutorialManager tMane;
    public int[] myActiveNumber;
    public int defaultRenNumber;
    public int renNumber;

	// Use this for initialization
	void Start () {
        tMane = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
	}
	
	// Update is called once per frame
	void Update () {
        for(int i = 0; i < myActiveNumber.Length; i++)
        {
            if(tMane.GetTutorialNumber() == myActiveNumber[i])
            {
                transform.SetSiblingIndex(renNumber);
                return;
            }
            else
            {
                transform.SetSiblingIndex(defaultRenNumber);
            }
        }
	}
}
