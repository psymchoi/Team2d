using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour, IDropHandler
{
    public enum eSlot
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
    }

    public eSlot eNum;
    
    InGameManager theInGame;
    CardBuyList theBuyList;

    void Start()
    {
        theInGame = FindObjectOfType<InGameManager>();
        theBuyList = FindObjectOfType<CardBuyList>();
    }

    void Update()
    {
        if (InGameManager.InGameInstance.m_curGameState
           == InGameManager.eGameState.ReadyForPlay)
        {

        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (InGameManager.InGameInstance.m_curGameState
           == InGameManager.eGameState.ReadyForPlay)
        {
            // Debug.Log("OnDrop Field");
            if (eventData.pointerDrag != null)
            {
                int n = theInGame.m_dragCardKind * 9 + (int)eNum;
                if (theInGame.m_isActiveMyCard[(int)eNum] == false && 
                    theInGame.m_isCharSlotOn[(int)eNum] == true)
                {
                    // 인벤토리에 있는 이미지를 누른 경우
                    // 해당 칸에 미리 배치해 놓은 캐릭터 SetActive(true)
                    theInGame.m_isActiveMyCard[n] = true;                            // 캐릭터 on
                    theInGame.transform.GetChild(n).gameObject.SetActive(true);      // 미리배치해둔 캐릭터 On
                    theInGame.transform.GetChild(n).
                        GetComponent<CharController>().m_charSlotNum = (int)eNum;
                    // Debug.Log("(int)eNum : " + (int)eNum);
                    // 해당 칸에 미리 배치해 놓은 캐릭터 SetActive(true)

                    //----- 이미지를 원래 인벤토리 자리로 -----
                    eventData.pointerDrag.GetComponent<Image>().sprite = default;
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                             = new Vector2(0, 0);

                    // 드래그해온 인벤토리 번호를 빈상태로
                    theBuyList.m_InvenNum[theInGame.m_slotNum] = 0;
                    // 드래그해온 인벤토리 번호를 빈상태로

                    theInGame.m_isCharSlotOn[(int)eNum] = false;        // 해당 번째 자리는 차있다.

                    //----- 이미지를 원래 인벤토리 자리로 -----
                    //break;                    
                }
                else
                {
                    Debug.Log("Is already spawn in there");
                    //break;
                }
                
            }
        }

    }
}
