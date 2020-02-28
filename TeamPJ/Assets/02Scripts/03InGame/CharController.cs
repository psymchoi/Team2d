using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 캐릭터 클릭 시 일어날 이벤트 관련 
public class CharController : MonoBehaviour
{
    public enum eCharacterNum
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
    }
    
    public eCharacterNum eCharNum;

    public Vector3 m_originPos;
    public int m_charSlotNum;

    float startPosX;
    float startPosY;
    bool isBeginHeld = false;
    bool isSell = false;

    InGameManager theInGame;
    MyCardInfo theCardInfo;

    void Start()
    {
        m_originPos = transform.position;

        theInGame = FindObjectOfType<InGameManager>();
        theCardInfo = FindObjectOfType<MyCardInfo>();

        // 해당 유형의 비활성화 된 객체도 가져온다
        // theCharSlot = Resources.FindObjectsOfTypeAll<CharacterSlot>();
        // 해당 유형의 비활성화 된 객체도 가져온다
    }

    void Update()
    {
        if (InGameManager.InGameInstance.m_curGameState
            == InGameManager.eGameState.ReadyForPlay)
        {
            if (isBeginHeld == true)
            {
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                this.gameObject.transform.localPosition = new Vector3(mousePos.x - startPosX, mousePos.y - startPosY, 0);

                // 레이를 쏴서 Sell되는 Image에 맞는지 안맞지 여부
                Vector2 touchPos = new Vector2(mousePos.x, mousePos.y);
                Ray2D ray = new Ray2D(touchPos, Vector2.zero);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                
                if (hit.collider.tag == "Sell")
                    isSell = true;
                else
                    isSell = false;
                // 레이를 쏴서 Sell되는 Image에 맞는지 안맞지 여부
            }
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
        Debug.Log("Mouse Up Char");
        
        transform.position = m_originPos;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 touchPos = new Vector2(worldPos.x, worldPos.y);
        Ray2D ray = new Ray2D(touchPos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);


        // 팔렸을 경우
        Debug.Log(hit.collider.tag.ToString());
        if (isSell == true)
        {
            Debug.Log("sell");

            InGameManager.InGameInstance.m_Money += theCardInfo.sellCost;
            theInGame.m_isCharSlotOn[m_charSlotNum] = true;
            this.gameObject.SetActive(false);
        }

        for (int n = 0; n < theInGame.m_isCharSlotOn.Length; n++)
            theInGame.m_cardZone[n].gameObject.SetActive(false);
        // 팔렸을 경우

        // 판매 UI Off
        theCardInfo.SellOff();
        // 판매 UI Off

        isBeginHeld = false;
        isSell = false;
    }
    
}