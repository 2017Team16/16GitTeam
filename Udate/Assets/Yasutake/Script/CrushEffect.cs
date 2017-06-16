using UnityEngine;
using System.Collections;

public class CrushEffect : MonoBehaviour {

    private ParticleSystem m_Particle;
    public int particleStopNum = 0;

	// Use this for initialization
	void Start () {
        m_Particle = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Crush(int no)
    {
        if (particleStopNum == no) return;
        m_Particle.Emit(1);
    }
}
