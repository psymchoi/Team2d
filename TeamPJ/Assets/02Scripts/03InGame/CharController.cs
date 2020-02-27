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
    
    MyCardInfo theCardInfo;


    void Start()
    {
        m_originPos = transform.position;

        theCardInfo = FindObjectOfType<MyCardInfo>();
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
            }
        }
    }

    void OnMouseDown()
    {
        if (InGameManager.InGameInstance.m_curGameState
            == InGameManager.eGameState.ReadyForPlay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // 판매 UI On
                theCardInfo.SellOn((int)eCharNum);
                // 판매 UI On

                InGameManager.InGameInstance.OffShopUI();
                
                Vector3 mousePos;
                mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);

                startPosX = mousePos.x - this.transform.localPosition.x;
                startPosY = mousePos.y - this.transform.localPosition.y;

                isBeginHeld = true;
            }
        }
    }

    void OnMouseUp()
    {
        Debug.Log("Mouse Up");
        
        transform.position = m_originPos;

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(pos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // 팔렸을 경우
        if (hit.collider.tag == "Sell")
        {
            Debug.Log("sell");
            this.gameObject.SetActive(false);
            InGameManager.InGameInstance.m_Money += theCardInfo.sellCost;
        }
        // 팔렸을 경우

        // 판매 UI Off
        theCardInfo.SellOff();
        // 판매 UI Off

        isBeginHeld = false;

       
    }
    
}