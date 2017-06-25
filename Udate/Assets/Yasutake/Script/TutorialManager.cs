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
    private bool isskip = false;

    public Sprite[] tutorialUIs;
    public Image tutorial;
    public GameObject timer;
    public GameObject score;
    public GameObject special;
    public GameObject hp;
    public GameObject skip;
    public SceneChanger sc;

    public GameObject[] spownPoints;
    public GameObject tutorialEnemy;

    [SerializeField, Header("出現用Particle(Dustsmoke)")]
    private GameObject _Enemyspawner;

    // Use this for initialization
    void Start () {
        pSrc = player.GetComponent<TutorialPlayer>();

        m_Number = 1;
        pmaePos = player.transform.position;
        pmaePos.y = 0;
        pMoveDistance = 0;
        manualTime = 3.0f;
        timer.SetActive(false);
        score.SetActive(false);
        special.SetActive(false);
        hp.SetActive(false);
        skip.SetActive(false);
        tutorial05enemy = null;
        isskip = false;
    }
	
	// Update is called once per frame
	void Update () {
        switch (m_Number)
        {
            case 1: Tutorial01();break;
            case 2: Tutorial02(); break;
            case 3: Tutorial03();break;
            case 4: Tutorial04(); break;
            case 5: Tutorial05(); break;
            case 6: Tutorial06(); break;
            case 7: Tutorial07(); break;
            case 8: Tutorial08(); break;
            case 9: Tutorial09(); break;
            case 10: Tutorial10(); break;
            case 11: Tutorial11(); break;
            case 12: Tutorial12(); break;
            case 13: Tutorial13(); break;
            case 14: Tutorial14(); break;
            case 15: Tutorial15(); break;
            case 16: Tutorial16(); break;
            case 17: Tutorial17(); break;
            case 18: Tutorial18(); break;
            case 19: Tutorial19(); break;
            case 20: Tutorial20(); break;
            case 21: Tutorial21(); break;
            case 22: Tutorial22(); break;
            case 23: Tutorial23(); break;
            case 24: Tutorial24(); break;
        }

        if (Input.GetButtonDown("XboxStart"))
        {
            isskip = !isskip;
        }
        if (isskip)
        {
            skip.SetActive(true);
            if (Input.GetButtonDown("XboxA"))
            {
                isskip = false;
            }
            if (Input.GetButtonDown("XboxB"))
            {
                sc.FadeOut();
            }
        }
        else
        {
            skip.SetActive(false);
        }
	}

    private void TutorialAuto(int n)
    {
        manualTime -= Time.deltaTime;
        if (manualTime <= 0.0f)
        {
            m_Number = n;
            tutorial.sprite = tutorialUIs[m_Number - 1];
            manualTime = 4.0f;
        }
    }
    private void Tutorial01()
    {
        TutorialAuto(2);
        pmaePos = player.transform.position;
        pmaePos.y = 0;
    }
    private void Tutorial02()
    {
        Vector3 ppos = player.transform.position;
        ppos.y = 0;
        pMoveDistance += Mathf.Abs(Vector3.Distance(ppos, pmaePos));
        if (pMoveDistance > 20.0f)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
        }
        pmaePos = ppos;
    }
    private void Tutorial03()
    {
        if (GameDatas.isBrotherFlying)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];

            GameObject wreckClone = (GameObject)Instantiate
           (_Enemyspawner,spownPoints[0].transform.position,
            Quaternion.identity);

            wreckClone.GetComponent<Enemyspawner>().OccurrenceSetting(tutorialEnemy, 1);
        }

    }
    private void Tutorial04()
    {
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
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
        }
        
    }

    private void Tutorial05()
    {
        if (tutorial05enemy.GetEnemyState() == EnemyBase.EnemyState.SUTAN)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
        }
    }

    private void Tutorial06()
    {
        if(pSrc.GetEnemyCount() >= 1)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
        }
    }

    private void Tutorial07()
    {
        if (pSrc.GetEnemyCount() == 0)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
            score.SetActive(true);
        }
    }

    private void Tutorial08()
    {
        TutorialAuto(9);
        if(manualTime == 4.0f)
        {
            score.SetActive(false);
            timer.SetActive(true);
        }
    }

    private void Tutorial09()
    {
        TutorialAuto(10);
        if (manualTime == 4.0f)
        {
            timer.SetActive(false);
            hp.SetActive(true);
        }
    }

    private void Tutorial10()
    {
        TutorialAuto(11);
        if (manualTime == 4.0f)
        {
            hp.SetActive(false);
        }
    }

    private void Tutorial11()
    {
        TutorialAuto(12);
    }

    private void Tutorial12()
    {
        if (player.transform.position.y > 7.0f)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
            for(int i = 0; i < spownPoints.Length; i++)
            {
                GameObject wreckClone = (GameObject)Instantiate
                  (_Enemyspawner, spownPoints[i].transform.position,
                  Quaternion.identity);

                wreckClone.GetComponent<Enemyspawner>().OccurrenceSetting(tutorialEnemy, 1);
            }
        }
    }

    private void Tutorial13()
    {
        int EnemyCount = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (obj.transform.GetComponent<EnemyType>().GetEType() == "Tackle")
            {
                EnemyCount++;
            }
        }

        if (EnemyCount >= 4)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
        }

    }

    private void Tutorial14()
    {
        if (pSrc.GetEnemyCount() >= 1)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
        }
    }

    private void Tutorial15()
    {
        if (pSrc.GetEnemyCount() >= 4)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
            GameObject wreckClone = (GameObject)Instantiate
                  (_Enemyspawner, spownPoints[0].transform.position,
                  Quaternion.identity);

            wreckClone.GetComponent<Enemyspawner>().OccurrenceSetting(tutorialEnemy, 1);
        }
    }

    private void Tutorial16()
    {
        int EnemyCount = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (obj.transform.GetComponent<EnemyType>().GetEType() == "Tackle")
            {
                EnemyCount++;
            }
        }

        if (EnemyCount >= 5)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
        }

    }

    private void Tutorial17()
    {
        if (pSrc.GetEnemyCount() <= 2)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
        }
    }

    private void Tutorial18()
    {
        TutorialAuto(19);
        if (manualTime == 4.0f)
        {
            special.SetActive(true);
        }
    }

    private void Tutorial19()
    {
        if (Input.GetButtonDown("XboxR1"))
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
        }
    }

    private void Tutorial20()
    {
        int EnemyCount = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (obj.transform.GetComponent<EnemyType>().GetEType() == "Tackle")
            {
                EnemyCount++;
            }
        }

        if (EnemyCount <= 4)
        {
            m_Number++;
            tutorial.sprite = tutorialUIs[m_Number - 1];
            special.SetActive(false);
        }
    }

    private void Tutorial21()
    {
        TutorialAuto(22);
    }

    private void Tutorial22()
    {
        TutorialAuto(23);
    }

    private void Tutorial23()
    {
        TutorialAuto(24);
    }

    private void Tutorial24()
    {
        manualTime -= Time.deltaTime;
        if (manualTime <= 0.0f)
        {
            sc.FadeOut();
        }
    }

    public int GetTutorialNumber()
    {
        return m_Number;
    }
}
