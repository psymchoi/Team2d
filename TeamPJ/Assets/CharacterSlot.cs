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

    InGameManager theInGameManager;
    DragDropImg theDragImg;
    CardBuyList theCardBuyList;

    void Start()
    {
        theInGameManager = FindObjectOfType<InGameManager>();
        theDragImg = FindObjectOfType<DragDropImg>();
        theCardBuyList = FindObjectOfType<CardBuyList>();
    }
    

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            for (int n = m_slotNum * 9 + (int)eNum ; n < (m_slotNum + 1) * 9; n++)
            {
                if(theInGameManager.m_isActiveMyCard[n] == false)
                {
                    // 미리 배치해 놓은 캐릭터 SetActive(true)
                    theInGameManager.m_isActiveMyCard[n] = true;
                    theInGameManager.transform.GetChild(n).gameObject.SetActive(true);
                    // 미리 배치해 놓은 캐릭터 SetActive(true)

                    // 이미지를 원래 인벤토리 자리로
                    eventData.pointerDrag.GetComponent<Image>().sprite = default;
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                             = new Vector2(0, 0);
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
