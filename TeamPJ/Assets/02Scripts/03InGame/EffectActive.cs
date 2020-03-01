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

   
    /// <summary>
    /// 캐릭터가 스폰될때 이펙트 효과
    /// </summary>
    /// <param name="pos"> 이펙트 효과위치 </param>
    public void CharacterSpawn(Transform pos)
    {
        m_effect[0].transform.position = pos.position;
        m_effect[0].Play();
    }

    /// <summary>
    /// 적 공격시 나타날 기본 이펙트 효과
    /// </summary>
    /// <param name="pos"> 이펙트 효과 위치 </param>
    public void CharacterAttack(Transform pos)
    {
        GameObject eff = Instantiate(m_effect[1].gameObject);
        eff.transform.parent = this.transform;
        eff.transform.position = pos.position;
        eff.GetComponent<ParticleSystem>().Play();

        Destroy(eff, 2);
    }
}
