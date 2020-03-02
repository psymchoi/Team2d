using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 캐릭터 클릭 시 일어날 이벤트 관련 
public class CharController : MonoBehaviour
{
    // 캐릭터 종류
    public enum eCharacterNum
    {
        One,        // 갈색
        Two,        // 초록
        Three,      // 파랑
        Four,       // 회색
        Five,       // 빨강
        Six,        // 미남
    }   
    public eCharacterNum eCharNum;
    // 캐릭터 종류

    // 캐릭터 애니메이션 상태
    public enum eAniState
    {
        Idle,
        Run,
        Attack,
        Death,
    }
    eAniState eAState;
    // 캐릭터 애니메이션 상태

    public Vector3 m_originPos;     // 이 스크립트가 붙은 객체의 원래 위치
    //public int m_charActiveNum;     // InGameManager 하위에 몇 번째가 켜져있나
    public int m_charSlotNum;       // 현재 캐릭터가 위치한 슬롯 넘버

    float startPosX;                // 마우스 x좌표
    float startPosY;                // 마우스 y좌표
    bool isBeginHeld = false;       // 터치가 일어났는지 여부
    bool isSell = false;            // 팔 수 있는지 없는지 판단여부
    bool isActive = true;           // 자리이동이 가능한지 판단여부
    int m_movSlotNum = 0;           // 위치를 이동시킬 슬롯 넘버

    // 캐릭터 능력치 관련
    public int m_level;     
    public float m_hp;
    public float m_curHp;
    public float m_atk;
    public int m_atkRange;               // 해당 객체 공격범위
    public float m_def;
    public int m_movSpeed;          // 캐릭터 이동 속도
    float dist;                     // 가장 가까운 적과의 거리

    public float a_upHp;
    public float a_upAtk;
    public int a_upMSpeed;
    // 캐릭터 능력치 관련

    InGameManager theInGame;
    MyCardInfo theCardInfo;
    CharacterStat theCStat;

    Animator m_aniCtrl;

    GameObject m_targetEnemy;

    void Start()
    {
        m_originPos = transform.position;

        theInGame = FindObjectOfType<InGameManager>();
        theCardInfo = FindObjectOfType<MyCardInfo>();
        theCStat = FindObjectOfType<CharacterStat>();

        m_aniCtrl = GetComponent<Animator>();

        // 해당 유형의 비활성화 된 객체도 가져온다
        // theCharSlot = Resources.FindObjectsOfTypeAll<CharacterSlot>();
        // 해당 유형의 비활성화 된 객체도 가져온다


        theInGame.m_myCard.Add(this.gameObject);
        // InGameManager 에서 몇 번째꺼가 켜져있나
        //m_charActiveNum = (int)eCharNum * 9 + m_charSlotNum;
        // InGameManager 에서 몇 번째꺼가 켜져있나





        dist = 1000;

        // 스폰 시 최초 한 번 실행 될 effect
        EffectActive.EffectInstance.CharacterSpawn(this.transform);
        // 스폰 시 최초 한 번 실행 될 effect

    }

    void Update()
    {
        if (InGameManager.InGameInstance.m_curGameState
          == InGameManager.eGameState.ReadyForPlay)
        {
            eAState = eAniState.Idle;

            if (isBeginHeld == true)
            {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);
                
                //----- Ray를 쏘아 태그를 판별해서 이벤트를 발생시킬 부분 -----
                Vector2 touchPos = new Vector2(mousePos.x, mousePos.y);
                Ray2D ray = new Ray2D(touchPos, Vector2.zero);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                
                // 캐릭터 팔기
                if (hit.collider.tag == "Sell")
                    isSell = true;
                else
                    isSell = false;
                // 캐릭터 팔기

                // 캐릭터 슬롯 이동
                if (hit.collider.tag == "MoveCharacter")
                {
                    // Debug.Log("Move Char");
                    isActive = false;
                     m_movSlotNum = (int)hit.collider.gameObject.GetComponent<CharacterSlot>().eNum;
                }
                else
                    isActive = true;
                // 캐릭터 슬롯 이동
                
                //----- Ray를 쏘아 태그를 판별해서 이벤트를 발생시킬 부분 -----
            }
        }
        else if(theInGame.m_curGameState == InGameManager.eGameState.WaitPlayTime)
        {
             for (int n = 0; n < theInGame.m_isCharSlotOn.Length; n++)
                 theInGame.m_cardZone[n].gameObject.SetActive(false);
             
             // 판매 UI Off
             theCardInfo.SellOff();
             theCardInfo.OffInfoPanel();
             // 판매 UI Off
             
             this.transform.position = m_originPos;            
        }

        CharacterAniState();
    }

    public void CharStatReset()
    {
        // 캐릭터 능력치 상태
        int a_lvl = m_level;
        while (a_lvl > 1)
        {
            m_hp = m_hp * a_upHp;
            m_atk = m_atk * a_upAtk;
            m_def += 1;
            m_movSpeed += a_upMSpeed;
            this.transform.localScale
                    = new Vector3(this.transform.localScale.x + 0.04f, this.transform.localScale.y + 0.04f, 1);

            a_lvl--;
        }
        m_curHp = m_hp;
        // 캐릭터 능력치 상태
    }

    /// <summary>
    /// 캐릭터 애니메이션 부분
    /// </summary>
    public void CharacterAniState()
    {
        if (theInGame.m_curGameState == InGameManager.eGameState.Play)
        {
            switch (eAState)
            {
                case eAniState.Idle:
                    // 객체가 없거나 죽어있는 녀석을 제외하고, 가까이 있는 적을 찾아낸다.
                    foreach (GameObject taggedEnemy in StageManager.StageInstance.m_tfEnemy)
                    {
                        if(taggedEnemy == null ||
                            taggedEnemy.GetComponent<EnemyController>().m_isDeath == true)
                        {
                            continue;
                        }

                        Vector2 objPos = taggedEnemy.transform.position;
                        if (Vector2.Distance(transform.position, objPos) < dist)
                        {
                            dist = Vector2.Distance(transform.position, objPos);
                            m_targetEnemy = taggedEnemy;
                        }
                    }
                    // 객체가 없거나 죽어있는 녀석을 제외하고, 가까이 있는 적을 찾아낸다.

                    if (m_targetEnemy == null)
                    {
                        InGameManager.InGameInstance.m_isClear = true;

                        m_aniCtrl.SetTrigger("EndPlay");
                        dist = 1000;
                    }
                    else
                    {
                        eAState = eAniState.Run;
                    }

                    break;
                case eAniState.Run:
                    //----- 적과 관련 ------
                    // 객체가 없거나 죽어있는 녀석을 제외하고, 가까이 있는 적을 찾아낸다.
                    foreach(GameObject taggedEnemy in StageManager.StageInstance.m_tfEnemy)
                    {
                        if (taggedEnemy == null ||
                            taggedEnemy.GetComponent<EnemyController>().m_isDeath == true)
                        {
                            continue;
                        }

                        Vector2 objPos = taggedEnemy.transform.position;
                        if(Vector2.Distance(transform.position, objPos) < dist)
                        {
                            dist = Vector2.Distance(transform.position, objPos);
                            m_targetEnemy = taggedEnemy;
                        }
                    }
                    // 객체가 없거나 죽어있는 녀석을 제외하고, 가까이 있는 적을 찾아낸다.
                    
                    if (m_targetEnemy == null)
                    {
                        eAState = eAniState.Idle;
                        dist = 1000;
                        break;
                    }

                    float mov = m_movSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, m_targetEnemy.transform.position, mov);
                    if (Vector2.Distance(transform.position, m_targetEnemy.transform.position) <= m_atkRange)
                    {
                        eAState = eAniState.Attack;         // 내 위치와 적 위치가 공격범위 안쪽이면 공격!
                    }
                    //----- 적과 관련 -----

                    break;
                case eAniState.Attack:
                
                    if(m_targetEnemy.GetComponent<EnemyController>().m_isDeath == true)
                    {
                        eAState = eAniState.Idle;       // 현재 캐릭터를 Idle 상태로
                        dist = 1000;                    // dist를 초기화
                        m_targetEnemy = null;           // 죽였다면 타겟이 없는걸로
                    }
                
                    break;
                case eAniState.Death:
                    m_aniCtrl.SetTrigger("Death");
                    break;
            }
            m_aniCtrl.SetInteger("AniState", (int)eAState);

            // Debug.Log(m_targetEnemy);

        }
    }

    void OnMouseDown()
    {
        Debug.Log("Mouse Down Char");
        if (InGameManager.InGameInstance.m_curGameState
            == InGameManager.eGameState.ReadyForPlay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 해당 캐릭터종류 번호 담기
                theInGame.m_dragCardKind = (int)eCharNum;
                // 해당 캐릭터종류 번호 담기
                
                // 판매 UI On
                theCardInfo.SellOn((int)eCharNum, m_level);
                // 판매 UI On

                // 인벤토리 UI Off
                InGameManager.InGameInstance.OffShopUI();
                // 인벤토리 UI Off
                
                // 9칸짜리 캐릭터 슬롯 On
                for (int n = 0; n < theInGame.m_isCharSlotOn.Length; n++)
                {
                    if (n != m_charSlotNum)
                    {
                        theInGame.m_cardZone[n].gameObject.SetActive(true);
                    }
                }
                // 9칸짜리 캐릭터 슬롯 On

                // 마우스를 눌렀을 때 마우스 좌표 얻기
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                startPosX = mousePos.x - this.transform.localPosition.x;
                startPosY = mousePos.y - this.transform.localPosition.y;
                // 마우스를 눌렀을 때 마우스 좌표 얻기

                isBeginHeld = true;
            }
        }
    }

    void OnMouseUp()
    {
        if (InGameManager.InGameInstance.m_curGameState
           == InGameManager.eGameState.ReadyForPlay)
        {
            Debug.Log("Mouse Up Char");

            // 캐릭터를 원래 위치로
            transform.position = m_originPos;
            // 캐릭터를 원래 위치로


            //Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector2 touchPos = new Vector2(worldPos.x, worldPos.y);
            //Ray2D ray = new Ray2D(touchPos, Vector2.zero);
            //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);


            // 캐릭터를 Sell 경우
            Debug.Log("isSell : " + isSell);
            Debug.Log("isActive : " + isActive);
            if (isSell == true)
            {
                Debug.Log("sell");

                InGameManager.InGameInstance.m_money += theCardInfo.sellCost;   // 일정량의 돈을 돌려받는다.
                theInGame.m_isCharSlotOn[m_charSlotNum] = true;                 // 캐릭터가 있던 슬롯 위치를 열어준다.


                int m = theInGame.m_dragCardKind * 9 + m_charSlotNum;
                theInGame.transform.GetChild(m).
                        GetComponent<CharController>().m_level = 1;
                theInGame.transform.GetChild(m).
                    GetComponent<CharController>().m_hp = 200;
                theInGame.transform.GetChild(m).
                    GetComponent<CharController>().m_atk = 100;
                theInGame.transform.GetChild(m).
                    GetComponent<CharController>().m_def = 1;
                theInGame.transform.GetChild(m).
                    GetComponent<CharController>().m_movSpeed = 5;
                theInGame.transform.GetChild(m).localScale
                    = new Vector3(0.17f, 0.17f, 1);

                this.gameObject.SetActive(false);                               // 객체를 꺼준다.

                // 판매 UI Off
                theCardInfo.SellOff();
                theCardInfo.OffInfoPanel();
                // 판매 UI Off

                // 캐릭터 슬롯을 꺼준다
                for (int n = 0; n < theInGame.m_isCharSlotOn.Length; n++)
                    theInGame.m_cardZone[n].gameObject.SetActive(false);
                // 캐릭터 슬롯을 꺼준다

                return;
            }
            // 캐릭터를 Sell 경우


            // 캐릭터 위치를 Change / 캐릭터 등급 업
            if (isActive == false)
            {
                int a_movSlotNum  = theInGame.m_dragCardKind * 9 + m_movSlotNum;
                int a_charSlotNum = theInGame.m_dragCardKind * 9 + m_charSlotNum;
                Debug.Log("n : " + a_movSlotNum);
                if (theInGame.m_isActiveMyCard[a_movSlotNum] == false &&
                    theInGame.m_isCharSlotOn[m_movSlotNum] == true)
                {
                    Debug.Log("Character Move");

                    theInGame.m_isActiveMyCard[a_charSlotNum] = false;          // 옮기기 전의 캐릭터 off
                    theInGame.m_isActiveMyCard[a_movSlotNum] = true;            // 옮긴 후 캐릭터 on
                    theInGame.m_isCharSlotOn[m_charSlotNum] = true;             // 옮기기 전 자리는 빈 상태로
                    theInGame.m_isCharSlotOn[m_movSlotNum] = false;             // 옮긴 후 자리는 차지하는 공간으로

                    // 해당 칸에 미리 배치해 놓은 캐릭터 SetActive(true)
                    theInGame.transform.GetChild(a_movSlotNum).gameObject.SetActive(true);      // 미리배치해둔 캐릭터 On
                    theInGame.transform.GetChild(a_movSlotNum).
                        GetComponent<CharController>().m_charSlotNum = m_movSlotNum;

                    theInGame.transform.GetChild(a_movSlotNum).gameObject.
                                    GetComponent<CharController>().m_level = m_level;
                    theInGame.transform.GetChild(a_movSlotNum).gameObject.
                                    GetComponent<CharController>().m_hp = m_hp;
                    theInGame.transform.GetChild(a_movSlotNum).gameObject.
                                    GetComponent<CharController>().m_atk = m_atk;
                    theInGame.transform.GetChild(a_movSlotNum).gameObject.
                                    GetComponent<CharController>().m_def = m_def;
                    theInGame.transform.GetChild(a_movSlotNum).gameObject.
                                    GetComponent<CharController>().m_movSpeed = m_movSpeed;
                    theInGame.transform.GetChild(a_movSlotNum).localScale
                                             = new Vector3(this.transform.localScale.x,
                                                            this.transform.localScale.y, 1);
                    // 해당 칸에 미리 배치해 놓은 캐릭터 SetActive(true)

                    this.gameObject.SetActive(false);

                    // 판매 UI Off
                    theCardInfo.SellOff();
                    theCardInfo.OffInfoPanel();
                    // 판매 UI Off

                    // 캐릭터 슬롯을 꺼준다
                    for (int n = 0; n < theInGame.m_isCharSlotOn.Length; n++)
                        theInGame.m_cardZone[n].gameObject.SetActive(false);
                    // 캐릭터 슬롯을 꺼준다

                    return;
                }
                else
                {// 등급업인 경우
                    if (theInGame.transform.GetChild(a_movSlotNum).GetComponent<CharController>().eCharNum == eCharNum &&
                        theInGame.transform.GetChild(a_movSlotNum).GetComponent<CharController>().m_level == m_level)
                    {
                        theInGame.m_isActiveMyCard[a_charSlotNum] = false;          // 옮기기 전의 캐릭터 off
                        theInGame.m_isActiveMyCard[a_movSlotNum] = true;            // 옮긴 후 캐릭터 on
                        theInGame.m_isCharSlotOn[m_movSlotNum] = false;             // 옮긴 후 자리는 차지하는 공간으로
                        theInGame.m_isCharSlotOn[m_charSlotNum] = true;             // 옮기기 전 자리는 빈 상태로

                        // 해당 칸에 미리 배치해 놓은 캐릭터 SetActive(true)
                        theInGame.transform.GetChild(a_movSlotNum).gameObject.SetActive(true);      // 미리배치해둔 캐릭터 On
                        theInGame.transform.GetChild(a_movSlotNum).
                            GetComponent<CharController>().m_charSlotNum = m_movSlotNum;
                        // 해당 칸에 미리 배치해 놓은 캐릭터 SetActive(true)

                        // 캐릭터 능력치 Up
                        theInGame.transform.GetChild(a_movSlotNum).gameObject.
                                        GetComponent<CharController>().m_level += 1;
                        theInGame.transform.GetChild(a_movSlotNum).gameObject.
                                        GetComponent<CharController>().m_hp = m_hp * a_upHp;
                        theInGame.transform.GetChild(a_movSlotNum).gameObject.
                                        GetComponent<CharController>().m_atk = m_atk * a_upAtk;
                        theInGame.transform.GetChild(a_movSlotNum).gameObject.
                                        GetComponent<CharController>().m_def += 1;
                        theInGame.transform.GetChild(a_movSlotNum).gameObject.
                                        GetComponent<CharController>().m_movSpeed = m_movSpeed + a_upMSpeed;
                        theInGame.transform.GetChild(a_movSlotNum).localScale
                                        = new Vector3(theInGame.transform.GetChild(a_movSlotNum).localScale.x + 0.04f,
                                                       theInGame.transform.GetChild(a_movSlotNum).localScale.y + 0.04f, 1);
                        // 캐릭터 능력치 Up

                        this.gameObject.SetActive(false);
                        
                        
                        // 판매 UI Off
                        theCardInfo.SellOff();
                        theCardInfo.OffInfoPanel();
                        // 판매 UI Off


                        EffectActive.EffectInstance.CharacterSpawn(theInGame.transform.GetChild(a_movSlotNum).transform);

                        // 캐릭터 슬롯을 꺼준다
                        for (int n = 0; n < theInGame.m_isCharSlotOn.Length; n++)
                            theInGame.m_cardZone[n].gameObject.SetActive(false);
                        // 캐릭터 슬롯을 꺼준다

                        return;
                    }
                    else
                        Debug.Log("Can't Level UP");
                }
            }
            // 캐릭터 위치를 Change / 캐릭터 등급 업


            for (int n = 0; n < theInGame.m_isCharSlotOn.Length; n++)
                theInGame.m_cardZone[n].gameObject.SetActive(false);

            // 판매 UI Off
            theCardInfo.SellOff();
            theCardInfo.OffInfoPanel();
            // 판매 UI Off

            isBeginHeld = false;
            isSell = false;
            isActive = true;



            //this.transform.position = m_originPos;

            // 캐릭터를 Level Up 시킬 경우
            //if (isLevelUp == true)
            //{
            //    int n = theInGame.m_dragCardKind * 9 + m_movSlotNum;
            //    int m = theInGame.m_dragCardKind * 9 + m_charSlotNum;

            //    Debug.Log("Character LevelUp");


            //    theInGame.m_isActiveMyCard[m] = false;              // 옮기기 전의 캐릭터 off
            //    theInGame.m_isCharSlotOn[m_movSlotNum] = false;     // 옮긴 후 자리는 차지하는 공간으로
            //    theInGame.m_isCharSlotOn[m_charSlotNum] = true;     // 옮기기 전 자리는 빈 상태로

            //    // 해당 칸에 미리 배치해 놓은 캐릭터 SetActive(true)
            //    theInGame.m_isActiveMyCard[n] = true;                            // 캐릭터 on
            //    theInGame.transform.GetChild(n).gameObject.SetActive(true);      // 미리배치해둔 캐릭터 On
            //    theInGame.transform.GetChild(n).
            //        GetComponent<CharController>().m_charSlotNum = m_movSlotNum;
            //    // 캐릭터 능력치 Up


            //    theInGame.transform.GetChild(n).
            //        GetComponent<CharController>().m_level += 1;
            //    theInGame.transform.GetChild(n).
            //        GetComponent<CharController>().m_hp = m_hp * a_upHp;
            //    theInGame.transform.GetChild(n).
            //        GetComponent<CharController>().m_atk = m_atk * a_upAtk;
            //    theInGame.transform.GetChild(n).
            //        GetComponent<CharController>().m_def += 1;
            //    theInGame.transform.GetChild(n).
            //        GetComponent<CharController>().m_movSpeed = m_movSpeed + a_upMSpeed;
            //    theInGame.transform.GetChild(n).localScale
            //        = new Vector3(theInGame.transform.GetChild(n).localScale.x + 0.04f,
            //                       theInGame.transform.GetChild(n).localScale.y + 0.04f, 1);
            //    // 캐릭터 능력치 Up
            //    // 해당 칸에 미리 배치해 놓은 캐릭터 SetActive(true)

            //    this.gameObject.SetActive(false);

            //    // 캐릭터를 Level Up 시킬 경우


            //    // 판매 UI Off
            //    theCardInfo.SellOff();
            //    theCardInfo.OffInfoPanel();
            //    // 판매 UI Off

            //    isBeginHeld = false;
            //    isSell = false;

            //    EffectActive.EffectInstance.CharacterSpawn(theInGame.transform.GetChild(n).transform);
            //}
        }
    }

}