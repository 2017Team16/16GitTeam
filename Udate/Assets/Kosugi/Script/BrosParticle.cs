using UnityEngine;
using System.Collections;

public class BrosParticle : MonoBehaviour {

    /*--外部設定オブジェクト--*/
    [Header("弟管理クラス")]
    private BrotherStateManager m_BrotherStateManager;

    [SerializeField, Header("弟アニメーションオブジェクト")]
    private GameObject m_BrosAnimation;


    /*------内部設定変数------*/
    [Header("パーティクルシステム")]
    private ParticleSystem m_Particle;

    // Use this for initialization
    void Start () {
        m_BrotherStateManager = GameObject.Find("Brother").GetComponent<BrotherStateManager>();

        m_Particle = GetComponent<ParticleSystem>();

        GetComponent<ParticleSystem>().Stop();
	}
	
	// Update is called once per frame
	void Update () {
        if (GameDatas.isBrotherSpecialMove)
            m_Particle.Simulate(Time.unscaledDeltaTime, true, false);
        else
            m_Particle.Simulate(Time.deltaTime, true, false);

        transform.position = m_BrosAnimation.transform.position;
    }
}
