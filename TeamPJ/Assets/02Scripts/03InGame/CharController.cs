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
    public int m_charSlotNum;       // 현재 캐릭터가 위치한 슬롯 넘버
    public int m_movSpeed;          // 캐릭터 이동 속도

    float startPosX;                // 마우스 x좌표
    float startPosY;                // 마우스 y좌표
    bool isBeginHeld = false;       // 터치가 일어났는지 여부
    bool isSell = false;            // 팔 수 있는지 없는지 판단여부
    bool isActive = true;           // 자리이동이 가능한지 판단여부
    int m_movSlotNum = 0;
    float m_atkRange;
    float dist;
    
    InGameManager theInGame;
    MyCardInfo theCardInfo;

    Animator m_aniCtrl;

    Transform m_targetEnemy;

    void Start()
    {
        m_originPos = transform.position;

        theInGame = FindObjectOfType<InGameManager>();
        theCardInfo = FindObjectOfType<MyCardInfo>();

        m_aniCtrl = GetComponent<Animator>();

        // 해당 유형의 비활성화 된 객체도 가져온다
        // theCharSlot = Resources.FindObjectsOfTypeAll<CharacterSlot>();
        // 해당 유형의 비활성화 된 객체도 가져온다

        // 캐릭터 종류별 공격범위
        switch(eCharNum)
        {
            case eCharacterNum.Two:
                m_atkRange = 2.0f;
                break;
            case eCharacterNum.Three:
                m_atkRange = 2.0f;
                break;
            default:
                m_atkRange = 2.0f;
                break;
        }
        // 캐릭터 종류별 공격범위

        dist = Vector2.Distance(transform.position, StageManager.StageInstance.m_tfEnemy[0].position);
        m_targetEnemy = StageManager.StageInstance.m_tfEnemy[0];

        EffectActive.EffectInstance.CharacterSpawn(this.transform);
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
                
                // 레이를 쏘아 태그를 판별해서 이벤트를 발생시킬 부분
                Vector2 touchPos = new Vector2(mousePos.x, mousePos.y);
                Ray2D ray = new Ray2D(touchPos, Vector2.zero);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                
                if (hit.collider.tag == "Sell")
                    isSell = true;
                else
                    isSell = false;

                if (hit.collider.tag == "MoveCharacter")
                {
                    isActive = false;
                     m_movSlotNum = (int)hit.collider.gameObject.GetComponent<CharacterSlot>().eNum;
                }
                else
                    isActive = true;
                
                // 레이를 쏘아 태그를 판별해서 이벤트를 발생시킬 부분
            }
        }

        CharacterAniState();
    }

    public void CharacterAniState()
    {
        if (theInGame.m_curGameState == InGameManager.eGameState.Play)
        {
            switch (eAState)
            {
                case eAniState.Idle:

                    foreach (Transform taggedEnemy in StageManager.StageInstance.m_tfEnemy)
                    {
                        if(taggedEnemy == null ||
                            taggedEnemy.GetComponent<EnemyController>().m_isDeath == true)
                        {
                            m_targetEnemy = null;
                            continue;
                        }

                        Vector2 objPos = taggedEnemy.position;
                        if (Vector2.Distance(transform.position, objPos) < dist)
                        {
                            dist = Vector2.Distance(transform.position, objPos);
                            m_targetEnemy = taggedEnemy;
                        }
                    }

                    if (m_targetEnemy == null)
                        m_aniCtrl.SetTrigger("EndPlay");
                    else
                        eAState = eAniState.Run;

                    break;
                case eAniState.Run:
               
                    // 적과 관련
                    foreach(Transform taggedEnemy in StageManager.StageInstance.m_tfEnemy)
                    {
                        if (taggedEnemy == null)
                        {
                            m_targetEnemy = null;
                            continue;
                        }

                        Vector2 objPos = taggedEnemy.position;
                        if(Vector2.Distance(transform.position, objPos) < dist)
                        {
                            dist = Vector2.Distance(transform.position, objPos);
                            m_targetEnemy = taggedEnemy;
                        }
                    }
                    
                    if (m_targetEnemy == null)
                        eAState = eAniState.Idle;

                    float mov = m_movSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, m_targetEnemy.position, mov);
                    if (Vector2.Distance(transform.position, m_targetEnemy.position) <= m_atkRange)
                    {
                        dist = 1000;
                        eAState = eAniState.Attack;
                    }
                    // 적과 관련

                    break;
                case eAniState.Attack:
                
                    if(m_targetEnemy.GetComponent<EnemyController>().m_isDeath == true)
                    {
                        eAState = eAniState.Idle;       // 현재 캐릭터를 Idle 상태로
                        m_targetEnemy = null;           // 죽였다면 타겟이 없는걸로
                    }
                
                    break;
                case eAniState.Death:
                    m_aniCtrl.SetTrigger("Death");
                    break;
            }
            m_aniCtrl.SetInteger("AniState", (int)eAState);

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
                theCardInfo.SellOn((int)eCharNum);
                // 판매 UI On

                // 인벤토리 UI Off
                InGameManager.InGameInstance.OffShopUI();
                // 인벤토리 UI Off
                
                // 9칸짜리 캐릭터 슬롯 On
                for (int n = 0; n < theInGame.m_isCharSlotOn.Length; n++)
                {
                    if (theInGame.m_isCharSlotOn[n] == true)
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


            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(worldPos.x, worldPos.y);
            Ray2D ray = new Ray2D(touchPos, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        
            // 캐릭터를 팔았을 경우
            Debug.Log(hit.collider.tag.ToString());
            if (isSell == true)
            {
                Debug.Log("sell");

                InGameManager.InGameInstance.m_Money += theCardInfo.sellCost;   // 일정량의 돈을 돌려받는다.
                theInGame.m_isCharSlotOn[m_charSlotNum] = true;                 // 캐릭터가 있던 슬롯 위치를 열어준다.
                this.gameObject.SetActive(false);                               // 객체를 꺼준다.
            }

            for (int n = 0; n < theInGame.m_isCharSlotOn.Length; n++)
                theInGame.m_cardZone[n].gameObject.SetActive(false);
            // 캐릭터를 팔았을 경우

            // 캐릭터 위치를 바꿀 경우
            if(isActive == false)
            {
                int n = theInGame.m_dragCardKind * 9 + m_movSlotNum;
                int m = theInGame.m_dragCardKind * 9 + m_charSlotNum;
                Debug.Log("n : " + n);
                if (theInGame.m_isActiveMyCard[n] == false && 
                    theInGame.m_isCharSlotOn[m_movSlotNum] == true)
                {
                    Debug.Log("Character Move");

                    theInGame.m_isActiveMyCard[m] = false;              // 옮기기 전의 캐릭터 off
                    theInGame.m_isCharSlotOn[m_movSlotNum] = false;     // 옮긴 후 자리는 차지하는 공간으로
                    theInGame.m_isCharSlotOn[m_charSlotNum] = true;     // 옮기기 전 자리는 빈 상태로

                    // 해당 칸에 미리 배치해 놓은 캐릭터 SetActive(true)
                    theInGame.m_isActiveMyCard[n] = true;                            // 캐릭터 on
                    theInGame.transform.GetChild(n).gameObject.SetActive(true);      // 미리배치해둔 캐릭터 On
                    theInGame.transform.GetChild(n).
                        GetComponent<CharController>().m_charSlotNum = m_movSlotNum;
                    // 해당 칸에 미리 배치해 놓은 캐릭터 SetActive(true)
                
                    this.gameObject.SetActive(false);
                }
            }
            // 캐릭터 위치를 바꿀 경우

            // 판매 UI Off
            theCardInfo.SellOff();
            // 판매 UI Off

            isBeginHeld = false;
            isSell = false;
        }
    }
    
}