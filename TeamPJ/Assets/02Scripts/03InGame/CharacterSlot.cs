﻿using System.Collections;
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
    public bool m_isSlotOn = true;


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
            Debug.Log("OnDrop");
            if (eventData.pointerDrag != null)
            {
                for (int n = theInGame.m_dragCardKind * 9 + (int)eNum ; n < (theInGame.m_dragCardKind + 1) * 9; n++)
                {
                    if (theInGame.m_isActiveMyCard[(int)eNum] == false && theInGame.m_isCharSlotOn[(int)eNum] == true)
                    {
                        // 미리 배치해 놓은 캐릭터 SetActive(true)
                        theInGame.m_isActiveMyCard[n] = true;                            // 캐릭터 on
                        theInGame.transform.GetChild(n).gameObject.SetActive(true);      // 미리배치해둔 캐릭터 On
                        theInGame.transform.GetChild(n).
                            GetComponent<CharController>().m_charSlotNum = (int)eNum;
                        // 미리 배치해 놓은 캐릭터 SetActive(true)

                        // 이미지를 원래 인벤토리 자리로
                        eventData.pointerDrag.GetComponent<Image>().sprite = default;
                        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                                 = new Vector2(0, 0);

                        theBuyList.m_InvenNum[theInGame.m_slotNum] = 0;
                        theInGame.m_isCharSlotOn[(int)eNum] = m_isSlotOn = false;
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
