using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{

    public GameObject player;
    private TutorialAnitest pSrc;
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
    public float sankakuColor;
    public GameObject uiBack;
    public SceneChanger sc;
    private Vector2 onPosition;
    private Vector2 onScale;
    private Vector2 offPosition = new Vector2(-200, -150);
    private Vector2 offScale = new Vector2(0.5f, 0.5f);

    public Vector2 hpBackPosition;
    public Vector2 hpBackScale;
    public Vector2 specialBackPosition;
    public Vector2 specialBackScale;
    public Vector2 timerBackPosition;
    public Vector2 timerBackScale;
    public Vector2 scoreBackPosition;
    public Vector2 scoreBackScale;

    public Vector2[] backPositions;
    public Vector2[] backScales;

    public GameObject[] spownPoints;
    public GameObject tutorialEnemy;

    [SerializeField, Header("出現用Particle(Dustsmoke)")]
    private GameObject _Enemyspawner;

    public TutorialPose pose;
    private float maeTime;
    private bool isOff;

    // Use this for initialization
    void Start()
    {
        pSrc = player.GetComponent<TutorialAnitest>();

        m_Number = 0;
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
    void Update()
    {
        Debug.Log(Time.timeScale);
        if (pose.IsPose())
        {
            Time.timeScale = 0;
            maeTime = 0;
            return;
        }
        else if (maeTime == 0)
        {
            maeTime = 1;
            return;
        }
        switch (m_Number)
        {
            case 0: WindowOn(); break;
            case 1: Tutorial01(); break;
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
    }

    private void WindowOn()
    {
        m_Number++;
        tutorial.sprite = tutorialUIs[m_Number - 1];
        isOff = false;
        Time.timeScale = 0;
        tutorialObj.GetComponent<RectTransform>().anchoredPosition = onPosition;
        tutorialObj.GetComponent<RectTransform>().localScale = onScale;
        tutorialBack.color = new Color(1, 1, 1, 1);
        sankaku.SetActive(true);
        sankaku.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        uiBack.SetActive(true);
    }
    private void WindowOff()
    {
        if (Input.GetButtonDown("XboxB"))
        {
            sankaku.GetComponent<Image>().color = new Color(sankakuColor, sankakuColor, sankakuColor, 1);
        }
        if (Input.GetButtonUp("XboxB") && sankaku.GetComponent<Image>().color.r == sankakuColor)
        {
            isOff = true;
            Time.timeScale = 1;
            tutorialObj.GetComponent<RectTransform>().anchoredPosition = offPosition;
            tutorialObj.GetComponent<RectTransform>().localScale = offScale;
            tutorialBack.color = new Color(1, 1, 1, 0.7f);
            sankaku.SetActive(false);
            uiBack.SetActive(false);
        }
    }
    private void NextPage()
    {
        if (Input.GetButtonDown("XboxB"))
        {
            sankaku.GetComponent<Image>().color = new Color(sankakuColor, sankakuColor, sankakuColor, 1);
        }
        if (Input.GetButtonUp("XboxB") && sankaku.GetComponent<Image>().color.r == sankakuColor)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
            sankaku.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
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
            NextPage();
    }

    private void Tutorial01()
    {
            NextPage();
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
            WindowOn();
        }
        else if (GameDatas.isBrotherFlying && check == 0)
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
            WindowOn();
        }
    }

    private void Tutorial06()
    {
        WindowOff();
        WindouOffNow();
        if (pSrc.GetEnemyCount() >= 1)
        {
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
        NextPage();
        if (m_Number ==10)
        {

        }
    }

    private void Tutorial10()
    {
        WindowOff();
        if (GameDatas.isSpecialAttack)
        {
            m_Number++;
            return;
        }
        WindouOffNow();
    }

    private void Tutorial11()
    {
        if (manualTime == 0.0f)
        {
            NextPage();
            if (m_Number == 12)
            {
                manualTime = 1.0f;

            }
            return;
        }

        manualTime -= 1 / 1 * Time.deltaTime;
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

                WindowOn();
                manualTime = 1.0f;
            }
        }

    }

    private void Tutorial13()
    {
        NextPage();
        if (m_Number == 14)
        {

        }
    }

    private void Tutorial14()
    {
        NextPage();
        if (m_Number == 15)
        {

        }
    }

    private void Tutorial15()
    {
        NextPage();
        if (m_Number == 16)
        {

        }
    }

    private void Tutorial17()
    {
        WindowOff();
        WindouOffNow();
        if (player.transform.position.y > 7.0f)
        {
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
