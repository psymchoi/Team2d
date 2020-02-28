using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectActive : MonoBehaviour
{
    public static EffectActive EffectInstance;

    public ParticleSystem[] m_effect;

    void Start()
    {
        EffectInstance = this;
    }

    public void CharacterSpawn(Transform pos)
    {
        m_effect[0].transform.position = pos.position;
        m_effect[0].Play();
    }
}
