using UnityEngine;
using System.Collections;

public class BrosParticle : MonoBehaviour {

    /*------外部設定------*/
    [SerializeField, Header("弟アニメーションオブジェクト(シーンから)")]
    private GameObject m_BrosAnimation;


    /*------内部設定------*/
    [Header("弟管理クラス")]
    private BrotherStateManager m_BrotherStateManager;

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
