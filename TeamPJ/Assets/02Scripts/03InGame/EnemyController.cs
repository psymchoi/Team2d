using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum eAniState
    {
        Idle,
        Run,
        Attack,
        Death,
    }
    eAniState eAState;

    public bool m_isDeath = false;

    // 적 관련 변수
    [SerializeField] float m_hp;
    [SerializeField] float m_curHp;
    [SerializeField] float m_mp;                  // 캐릭터 마나
    [SerializeField] float m_curMp;              // 캐릭터 현재 마나
    [SerializeField] float m_maxMp;             // 꽉찬 마나
    public float m_atk;
    public float m_skillAtk;                           // 스킬 공격력
    [SerializeField] float m_def;
    [SerializeField] float m_magicDef;             // 마법 방어력
    [SerializeField] float m_atkSpeed;          // 공격속도
    [SerializeField] float m_atkRange;               // 해당 객체 공격범위
    public float m_stgAtkPtg;                      // 치명타 확률
    public float m_stgAtk;                          // 치명타 공격력
    [SerializeField] float m_movSpeed;           // 캐릭터 이동 속도
    [SerializeField] int m_price;                    // 처치했을 시 유저에게 줄 골드

    float dist;                     // 가장 가까운 적과의 거리
    float m_atkSpd;             // 공격속도 계산용

    GameObject m_targetChar;
    // 적 관련 변수

    Animator m_aniCtrl;

    InGameManager theInGame;

    // Start is called before the first frame update
    void Start()
    {
        m_aniCtrl = GetComponent<Animator>();

        theInGame = FindObjectOfType<InGameManager>();

        dist = 1000;
    }

    void Update()
    {
        if(theInGame.m_curGameState == InGameManager.eGameState.ReadyForPlay)
        {
            eAState = eAniState.Idle;

            m_hp = 170 * (theInGame.m_curStage * 1.1f);
            m_curHp = m_hp;
            m_atk = 17 * (theInGame.m_curStage * 1.1f);
        }

        EnemyAniState();
    }

    public void EnemyAniState()
    {
        if(theInGame.m_curGameState == InGameManager.eGameState.Play)
        {
            switch(eAState)
            {
                case eAniState.Idle:
                    foreach (GameObject taggedChar in theInGame.m_myCard)
                    {
                        if (taggedChar == null ||
                            taggedChar.GetComponent<CharController>().m_isDeath == true)
                        {
                            continue;
                        }

                        Vector2 objPos = taggedChar.transform.position;
                        if (Vector2.Distance(transform.position, objPos) < dist)
                        {
                            dist = Vector2.Distance(transform.position, objPos);
                            m_targetChar = taggedChar;
                        }
                    }

                    if (m_targetChar == null)
                    {
                        InGameManager.InGameInstance.m_isFail = true;

                        m_aniCtrl.SetTrigger("EndPlay");
                        dist = 1000;
                    }
                    else
                    {
                        if (Vector2.Distance(transform.position, m_targetChar.transform.position) <= m_atkRange)
                        {
                            m_atkSpd += Time.deltaTime / m_atkSpeed;
                            Debug.Log("Enemy m_atkSpd : " + m_atkSpd);
                            if (m_atkSpd >= 1.0f)
                            {
                                m_atkSpd = 0;
                                eAState = eAniState.Attack;
                            }
                        }
                        else
                        {
                            eAState = eAniState.Run;
                        }
                    }

                    break;
                case eAniState.Run:
                    //----- 적과 관련 ------
                    // 객체가 없거나 죽어있는 녀석을 제외하고, 가까이 있는 적을 찾아낸다.
                    foreach (GameObject taggedChar in theInGame.m_myCard)
                    {
                        if (taggedChar == null ||
                            taggedChar.GetComponent<CharController>().m_isDeath == true)
                        {
                            continue;
                        }

                        Vector2 objPos = taggedChar.transform.position;
                        if (Vector2.Distance(transform.position, objPos) < dist)
                        {
                            dist = Vector2.Distance(transform.position, objPos);
                            m_targetChar = taggedChar;
                        }
                    }
                    // 객체가 없거나 죽어있는 녀석을 제외하고, 가까이 있는 적을 찾아낸다.

                    if (m_targetChar == null)
                    {
                        eAState = eAniState.Idle;
                        dist = 1000;
                        break;
                    }

                    float mov = m_movSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, m_targetChar.transform.position, mov);
                    if (Vector2.Distance(transform.position, m_targetChar.transform.position) <= m_atkRange)
                    {
                        // eAState = eAniState.Attack;         // 내 위치와 적 위치가 공격범위 안쪽이면 공격!
                        eAState = eAniState.Idle;
                    }
                    //----- 적과 관련 -----

                    break;
                case eAniState.Attack:

                    if (m_targetChar.GetComponent<CharController>().m_isDeath == true)
                    {
                        eAState = eAniState.Idle;       // 현재 캐릭터를 Idle 상태로
                        dist = 1000;                    // dist를 초기화
                        m_targetChar = null;           // 죽였다면 타겟이 없는걸로
                    }

                    break;
                case eAniState.Death:
                    m_aniCtrl.SetTrigger("Death");
                    break;
            }

            m_aniCtrl.SetInteger("AniState", (int)eAState);
            if (eAState == eAniState.Attack)
                eAState = eAniState.Idle;

            // mp++
            if (m_curMp >= m_maxMp)
            {
                Debug.Log("Max Mp");
                m_curMp = m_maxMp;
                return;
            }

            m_curMp += Time.deltaTime * 1.5f;
            //Debug.Log("m_curMp : " + m_curMp); 
            // mp++
        }
    }

    /// <summary>
    /// 유저 캐릭터가 적을 쳤을 때 일어나는 일반공격 함수
    /// </summary>
    /// <param name="a_charAtk"> 유저 캐릭터 일반공격력 </param>
    /// <returns></returns>
    public float Hit(float a_charAtk)
    {
        if (m_isDeath == true)
            return 0;

        EffectActive.EffectInstance.CharacterAttack(this.transform);

        // 치명타 확률
        int stgAtkPtg = Random.Range(1, 101);
        bool isAtkPtg = false;

        if (stgAtkPtg <= 25)
            isAtkPtg = true;
        // 치명타 확률

        // 유저 캐릭터 공격력
        float playerAtk = a_charAtk;
        float a_dmg;

        if (isAtkPtg == true)
        {
            playerAtk = playerAtk * m_stgAtk;
            Debug.Log("Strong EnemyAtk : " + playerAtk);
            // 치명타 이펙트
            // 치명타 이펙트
        }
        else
        {
            Debug.Log("EnemyAtk : " + playerAtk);
        }

        if (m_def >= playerAtk)
            a_dmg = 1;
        else
            a_dmg = playerAtk - m_def;

        m_curHp -= a_dmg;

        if(m_curHp <= 0)
        {
            m_isDeath = true;
            m_aniCtrl.SetTrigger("Death");

            theInGame.m_money += m_price;

            Invoke("VanishEmy", 3.0f);
        }
        // 유저 캐릭터 공격력

        Debug.Log("------------------------------- ");
        Debug.Log("Enemy Damaged : " + a_dmg);
        Debug.Log("Enemy CurHp : " + m_curHp);
        Debug.Log("------------------------------- ");

        return a_dmg;
    }

    public void VanishEmy()
    {
        Destroy(this.gameObject);
    }


}
