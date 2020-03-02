using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator m_aniCtrl;

    public bool m_isDeath = false;

    // 적 관련 변수
    public float m_hp;
    public float m_curHp;
    public float m_atk;
    public int m_def;
    public int m_price;
    // 적 관련 변수

    InGameManager theInGame;

    // Start is called before the first frame update
    void Start()
    {
        m_aniCtrl = GetComponent<Animator>();

        theInGame = FindObjectOfType<InGameManager>();

    }

    void Update()
    {
        if(theInGame.m_curGameState == InGameManager.eGameState.ReadyForPlay)
        {
            m_hp = 200 * (theInGame.m_curStage * 1.5f);
            m_curHp = m_hp;
        }
    }

    public float Hit(float a_charAtk)
    {
        if (m_isDeath == true)
            return 0;

        EffectActive.EffectInstance.CharacterAttack(this.transform);
        
        float playerAtk = a_charAtk;
        float a_dmg;

        if (m_def >= playerAtk)
            a_dmg = 1;
        else
            a_dmg = playerAtk - m_def;

        m_curHp -= a_dmg;

        if(m_curHp <= 0)
        {
            m_isDeath = true;
            m_aniCtrl.SetBool("Dead", true);

            theInGame.m_money += m_price;

            Invoke("VanishChar", 3.0f);
        }

        Debug.Log("a_dmg : " + a_dmg);
        Debug.Log("m_curHp : " + m_curHp);

        return a_dmg;
    }

    public void VanishChar()
    {
        Destroy(this.gameObject);
    }


}
