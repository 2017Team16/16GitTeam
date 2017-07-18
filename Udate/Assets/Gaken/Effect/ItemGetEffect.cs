using UnityEngine;
using System.Collections;

public class ItemGetEffect : MonoBehaviour
{
    private Canvas _canvas;                 //行きたいUI部分

    [SerializeField] private float _tweenTime;           //遷移時間
    [SerializeField] private AnimationCurve _tweenCurve; //アニメーション曲線
    [SerializeField] private GameObject _particle;

    private bool isTweening = false;                    //遷移中か?

    // Use this for initialization
    void Start()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Canvas GetCanvas()
    {
        return _canvas;
    }

    //行きたいところへ移動
    public Coroutine MoveCor_(Vector3 target)
    {
        return StartCoroutine(MoveCor(target));
    }

    IEnumerator MoveCor(Vector3 target)
    {
        if (isTweening) yield break;
        isTweening = true;

        var startTime = Time.time;
        var initPos = transform.position;

        while (Time.time - startTime < _tweenTime)
        {

            float rate = (Time.time - startTime) / _tweenTime;
            var curvedRate = _tweenCurve.Evaluate(rate);
            transform.position = Vector3.Lerp(initPos, target, curvedRate);

            yield return new WaitForEndOfFrame();
        }

        transform.position = target;
        Instantiate(_particle, transform.position, Quaternion.identity);

        isTweening = false;

        if (_particle.GetComponent<ParticleSystem>().isStopped == true)
        {
            Destroy(transform.gameObject);
        }

    }
}
