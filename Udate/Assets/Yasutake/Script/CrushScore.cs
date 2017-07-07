using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CrushScore : MonoBehaviour {

    [SerializeField]
    SpriteRenderer[] images = new SpriteRenderer[4];
    [SerializeField]
    Sprite[] numberSprites = new Sprite[10];

    private float alpha = 0.0f;
    private Vector3 def;

    // Use this for initialization
    void Start()
    {
        def = transform.localPosition;
        for(int i = 0; i < images.Length; i++)
        {
            images[i].color = new Color(1, 1, 1, alpha);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (alpha == 0.0f) return;
        alpha -= Time.deltaTime;
        transform.localPosition += Vector3.up * 10 * Time.deltaTime;
        if (alpha <= 0.0f) alpha = 0.0f;
        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = new Color(1, 1, 1, alpha);
        }
    }
    public void SetNumbers(int num)
    {
        int str = num % 10;
        images[0].sprite = numberSprites[str];

        num = num / 10;
        str = num % 10;
        images[1].sprite = numberSprites[str];

        num = num / 10;
        str = num % 10;
        images[2].sprite = numberSprites[str];

        num = num / 10;
        str = num % 10;
        images[3].sprite = numberSprites[str];


        alpha = 1.0f;
        transform.localPosition = def;

        for (int i = images.Length - 1; i > 0; i--)
        {
            if (images[i].sprite.name == "result-score-nb_0")
            {
                images[i].sprite = numberSprites[10];
            }
            else
            {
                return;
            }
        }

    }
}
