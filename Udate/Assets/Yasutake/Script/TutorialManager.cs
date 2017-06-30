using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    public GameObject player;
    private TutorialPlayer pSrc;
    private int m_Number;
    private Vector3 pmaePos;
    private float pMoveDistance;
    private float manualTime;
    private int nextNum;
    private EnemyBase tutorial05enemy;
    private int check;
    private bool isskip = false;

    public Sprite[] tutorialUIs;
    public GameObject tutorialObj;
    public Image tutorial;
    public Image tutorialBack;
    public GameObject sankaku;
    public GameObject uiBack;
    public SceneChanger sc;
    private Vector2 onPosition;
    private Vector2 onScale;
    private Vector2 offPosition = new Vector2(-200, -150);
    private Vector2 offScale = new Vector2(0.5f,0.5f);

    public Vector2 hpBackPosition;
    public Vector2 hpBackScale;
    public Vector2 specialBackPosition;
    public Vector2 specialBackScale;
    public Vector2 timerBackPosition;
    public Vector2 timerBackScale;
    public Vector2 scoreBackPosition;
    public Vector2 scoreBackScale;


    public GameObject[] spownPoints;
    public GameObject tutorialEnemy;

    [SerializeField, Header("出現用Particle(Dustsmoke)")]
    private GameObject _Enemyspawner;

    public TutorialPose pose;
    private float maeTime;
    private bool isOff;

    // Use this for initialization
    void Start () {
        pSrc = player.GetComponent<TutorialPlayer>();

        m_Number = 1;
        pmaePos = player.transform.position;
        pmaePos.y = 0;
        pMoveDistance = 0;
        manualTime = 1.0f;
        tutorial05enemy = null;
        check = 0;
        isskip = false;
        onPosition = tutorialObj.GetComponent<RectTransform>().anchoredPosition;
        onScale = tutorialObj.GetComponent<RectTransform>().localScale;
        uiBack.SetActive(false);

        isOff = false;
        maeTime = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (pose.IsPose())
        {
            Time.timeScale = 0;
            maeTime = 0;
            return;
        }
        else if(maeTime == 0)
        {
            maeTime = 1;
            return;
        }
        switch (m_Number)
        {
            case 1: WindowOn(); Tutorial01();break;
            case 2: Tutorial02(); break;
            case 4: Tutorial04(); break;
            case 5: Tutorial05(); break;
            case 6: Tutorial06(); break;
            case 8: Tutorial08(); break;
            case 9: Tutorial09(); break;
            case 10: Tutorial10(); break;
            case 11: Tutorial11(); break;
            case 12: Tutorial12(); break;
            case 13: Tutorial13(); break;
            case 14: Tutorial14(); break;
            case 15: Tutorial15(); break;
            case 17: Tutorial17(); break;
            case 19: Tutorial19(); break;
            case 22: Tutorial22(); break;
            case 24: Tutorial24(); break;
            case 27: Tutorial27(); break;
            case 3:
            case 7:
            case 16:
            case 18:
            case 20:
            case 21:
            case 23:
            case 25:
            case 26: TutorialN(); break;
        }

        //if (Input.GetButtonDown("XboxStart"))
        //{
        //    pose.SetActive(true);
        //}
        //if (isskip)
        //{
        //    skip.SetActive(true);
        //    if (Input.GetButtonDown("XboxA"))
        //    {
        //        isskip = false;
        //    }
        //    if (Input.GetButtonDown("XboxB"))
        //    {
        //        sc.FadeOut();
        //    }
        //}
        //else
        //{
        //    skip.SetActive(false);
        //}
    }

    private void WindowOn()
    {
        isOff = false;
        Time.timeScale = 0;
        tutorialObj.GetComponent<RectTransform>().anchoredPosition = onPosition;
        tutorialObj.GetComponent<RectTransform>().localScale = onScale;
        tutorialBack.color = new Color(1, 1, 1, 1);
        sankaku.SetActive(true);
    }
    private void WindowOff()
    {
        if (Input.GetButtonDown("XboxB"))
        {
            isOff = true;
            Time.timeScale = 1;
            tutorialObj.GetComponent<RectTransform>().anchoredPosition = offPosition;
            tutorialObj.GetComponent<RectTransform>().localScale = offScale;
            tutorialBack.color = new Color(1, 1, 1, 0.7f);
            sankaku.SetActive(false);
        }
    }
    private void NextPage()
    {
        m_Number++;
        tutorial.sprite = tutorialUIs[m_Number - 1];
    }
    private void WindouOffNow()
    {
        if (isOff)
        {
            Time.timeScale = 1;
        }
    }

    private void TutorialN()
    {
        if (Input.GetButtonDown("XboxB"))
        {
            NextPage();
        }
    }

    private void Tutorial01()
    {
        if (Input.GetButtonDown("XboxB"))
        {
            NextPage();
        }
    }
    private void Tutorial02()
    {
        WindowOff();
        WindouOffNow();
        Vector3 ppos = player.transform.position;
        ppos.y = 0;
        pMoveDistance += Mathf.Abs(Vector3.Distance(ppos, pmaePos));
        if (pMoveDistance > 20.0f)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
            WindowOn();
        }
        pmaePos = ppos;
    }
    private void Tutorial04()
    {
        WindowOff();
        WindouOffNow();
        int EnemyCount = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (obj.transform.GetComponent<EnemyType>().GetEType() == "Tackle")
            {
                EnemyCount++;
                tutorial05enemy = obj.transform.GetComponent<EnemyBase>();
            }
        }

        if (EnemyCount >= 1)
        {
            NextPage();
            WindowOn();
        }
        else if (GameDatas.isBrotherFlying && check ==0)
        {
            GameObject wreckClone = (GameObject)Instantiate
          (_Enemyspawner, spownPoints[0].transform.position,
           Quaternion.identity);

            wreckClone.GetComponent<Enemyspawner>().OccurrenceSetting(tutorialEnemy, 1);
            check++;
        }
    }

    private void Tutorial05()
    {
        WindowOff();
        WindouOffNow();
        if (tutorial05enemy.GetEnemyState() == EnemyBase.EnemyState.SUTAN)
        {
            NextPage();
            WindowOn();
        }
    }

    private void Tutorial06()
    {
        WindowOff();
        WindouOffNow();
        if (pSrc.GetEnemyCount() >= 1)
        {
            NextPage();
            WindowOn();
            GameObject wreckClone = (GameObject)Instantiate
         (_Enemyspawner, spownPoints[3].transform.position,
          Quaternion.identity);

            wreckClone.GetComponent<Enemyspawner>().OccurrenceSetting(tutorialEnemy, 1);
        }
    }

    private void Tutorial08()
    {
        WindowOff();
        WindouOffNow();
        if (tutorial05enemy.GetEnemyState() == EnemyBase.EnemyState.SUTAN
            || manualTime <= 0.0f)
        {
            NextPage();
            WindowOn();
            manualTime = 3.0f;
        }
        if (tutorial05enemy.GetEnemyState() == EnemyBase.EnemyState.RECOVERY)
        {
            manualTime -= Time.deltaTime;
        }
    }

    private void Tutorial09()
    {
        if (Input.GetButtonDown("XboxB"))
        {
            NextPage();
            uiBack.SetActive(true);
            uiBack.GetComponent<RectTransform>().anchoredPosition = specialBackPosition;
            uiBack.GetComponent<RectTransform>().localScale = specialBackScale;
        }
    }
    
    private void Tutorial10()
    {
        WindowOff();
        WindouOffNow();
        if (GameDatas.isSpecialAttack)
        {
            m_Number++;
        }
    }

    private void Tutorial11()
    {
        if(manualTime == 0.0f)
        {
            if (Input.GetButtonDown("XboxB"))
            {
                manualTime = 1.0f;
                NextPage();
                uiBack.SetActive(false);
            }
            return;
        }
        manualTime -= Time.deltaTime;
        if (manualTime < 0.0f)
        {
            tutorial.sprite = tutorialUIs[m_Number - 1];
            manualTime = 0.0f;
            WindowOn();
        }
    }

    private void Tutorial12()
    {
        WindowOff();
        WindouOffNow();
        int EnemyCount = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (obj.transform.GetComponent<EnemyType>().GetEType() == "Tackle")
            {
                EnemyCount++;
                tutorial05enemy = obj.transform.GetComponent<EnemyBase>();
            }
        }
        if (EnemyCount <= 1)
        {
            manualTime -= Time.deltaTime;
            if (manualTime <= 0.0f)
            {
                NextPage();
                uiBack.SetActive(true);
                uiBack.GetComponent<RectTransform>().anchoredPosition = scoreBackPosition;
                uiBack.GetComponent<RectTransform>().localScale = scoreBackScale;
                WindowOn();
                manualTime = 1.0f;
            }
        }
        
    }

    private void Tutorial13()
    {
        if (Input.GetButtonDown("XboxB"))
        {
            NextPage();
            uiBack.GetComponent<RectTransform>().anchoredPosition = timerBackPosition;
            uiBack.GetComponent<RectTransform>().localScale = timerBackScale;
        }
    }

    private void Tutorial14()
    {
        if (Input.GetButtonDown("XboxB"))
        {
            NextPage();
            uiBack.GetComponent<RectTransform>().anchoredPosition = hpBackPosition;
            uiBack.GetComponent<RectTransform>().localScale = hpBackScale;
        }
    }

    private void Tutorial15()
    {
        if (Input.GetButtonDown("XboxB"))
        {
            NextPage();
            uiBack.SetActive(false);
        }
    }

    private void Tutorial17()
    {
        WindowOff();
        WindouOffNow();
        if (player.transform.position.y > 7.0f)
        {
            NextPage();
            WindowOn();
            for (int i = 0; i < spownPoints.Length; i++)
            {
                GameObject wreckClone = (GameObject)Instantiate
                  (_Enemyspawner, spownPoints[i].transform.position,
                  Quaternion.identity);

                wreckClone.GetComponent<Enemyspawner>().OccurrenceSetting(tutorialEnemy, 1);
            }
        }
    }

    private void Tutorial19()
    {
        WindowOff();
        WindouOffNow();

        if (pSrc.GetEnemyCount() > 0)
        {
            NextPage();
            WindowOn();
        }
    }

    private void Tutorial22()
    {
        WindowOff();
        WindouOffNow();
        int EnemyCount = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (obj.transform.GetComponent<EnemyType>().GetEType() == "Tackle")
            {
                EnemyCount++;
            }
        }

        if (EnemyCount == 0)
        {
            manualTime -= Time.deltaTime;
            if (manualTime <= 0.0f)
            {
                m_Number++;
                tutorial.sprite = tutorialUIs[m_Number - 1];
                WindowOn();
                manualTime = 3.0f;
            }
        }
    }

    private void Tutorial24()
    {
        WindowOff();
        WindouOffNow();
        if (Time.timeScale == 0) return;
        if (Input.GetButtonDown("XboxA"))
        {
            check++;
        }
        if (check >= 2)
        {
            manualTime -= Time.deltaTime;
            if (manualTime <= 0.0f)
            {
                NextPage();
                WindowOn();
                manualTime = 1.0f;
            }
        }
    }

    private void Tutorial27()
    {
        if (Input.GetButtonDown("XboxB"))
        {
            sc.FadeOut();
            m_Number++;
            Time.timeScale = 1;
        }
    }

    public int GetTutorialNumber()
    {
        return m_Number;
    }
}
