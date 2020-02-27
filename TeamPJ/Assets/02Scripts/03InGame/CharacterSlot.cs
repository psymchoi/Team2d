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
    public int m_slotNum;
    public int m_invenNum;
    public bool m_isSlotOn = false;


    InGameManager theInGameManager;
    CardBuyList theBuyList;

    void Start()
    {
        theInGameManager = FindObjectOfType<InGameManager>();
        theBuyList = FindObjectOfType<CardBuyList>();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (InGameManager.InGameInstance.m_curGameState
           == InGameManager.eGameState.ReadyForPlay)
        {
            Debug.Log("OnDrop");
            if (eventData.pointerDrag != null)
            {
                for (int n = m_slotNum * 9 + (int)eNum ; n < (m_slotNum + 1) * 9; n++)
                {
                    if (theInGameManager.m_isActiveMyCard[n] == false && m_isSlotOn == false)
                    {
                        // 미리 배치해 놓은 캐릭터 SetActive(true)
                        theInGameManager.m_isActiveMyCard[n] = true;
                        theInGameManager.transform.GetChild(n).gameObject.SetActive(true);
                        theInGameManager.transform.GetChild(n).
                            GetComponent<CharController>().m_charSlotNum = (int)eNum;
                        // 미리 배치해 놓은 캐릭터 SetActive(true)

                        // 이미지를 원래 인벤토리 자리로
                        eventData.pointerDrag.GetComponent<Image>().sprite = default;
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                                 = new Vector2(0, 0);

                        theBuyList.m_InvenNum[m_invenNum] = 0;
                        m_slotNum = 0;
                        m_isSlotOn = true;
                        // 이미지를 원래 인벤토리 자리로
                        break;
                    }
                    else
                    {
                        Debug.Log("Is already spawn in there");
                        break;
                    }
                }
            }
        }

    }
}
