using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropImg : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public enum eDragDropNum
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
    }

    [SerializeField] Canvas m_canvas;
    public eDragDropNum m_edragDropNum;
    //public GameObject[] m_CharSlot;

    RectTransform m_rectTransform;
    CanvasGroup m_canvasGroup;

    InGameManager theInGame;
    CardBuyList theBuyList;

    void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_canvasGroup = GetComponent<CanvasGroup>();

        theInGame = FindObjectOfType<InGameManager>();
        theBuyList = FindObjectOfType<CardBuyList>();
    }
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(InGameManager.InGameInstance.m_curGameState
            == InGameManager.eGameState.ReadyForPlay)
        {
            // Debug.Log("OnBeginDrag");

            if (theBuyList.m_isEmpty[(int)m_edragDropNum] == true)
                return;

            m_canvasGroup.alpha = .6f;
            m_canvasGroup.blocksRaycasts = false;

            for (int n = 0; n < theInGame.m_isCharSlotOn.Length; n++)
            {
                if (theInGame.m_isCharSlotOn[n] == true)
                    theInGame.m_cardZone[n].SetActive(true);
            }
        }
        else
        {
            m_rectTransform.anchoredPosition = new Vector2(0, 0);
            m_canvasGroup.alpha = 1f;
            m_canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (InGameManager.InGameInstance.m_curGameState
           == InGameManager.eGameState.ReadyForPlay)
        {
            // Debug.Log("OnDrag");

            if (this.transform.GetComponent<Image>().sprite != null)
            {
                m_rectTransform.anchoredPosition += eventData.delta / m_canvas.scaleFactor;
            }
        }
        else
        {
            m_rectTransform.anchoredPosition = new Vector2(0, 0);
            m_canvasGroup.alpha = 1f;
            m_canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (InGameManager.InGameInstance.m_curGameState
           == InGameManager.eGameState.ReadyForPlay)
        {
            // Debug.Log("OnEndDrag");

            m_canvasGroup.alpha = 1f;
            m_canvasGroup.blocksRaycasts = true;

            for (int n = 0; n < theInGame.m_isCharSlotOn.Length; n++)
                theInGame.m_cardZone[n].gameObject.SetActive(false);
        }
        else
        {
            m_rectTransform.anchoredPosition = new Vector2(0, 0);
            m_canvasGroup.alpha = 1f;
            m_canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (InGameManager.InGameInstance.m_curGameState
           == InGameManager.eGameState.ReadyForPlay)
        {
            if (theBuyList.m_isEmpty[(int)m_edragDropNum] == true)
                return;

            // Debug.Log("On Pointer Down");

            // 카드 종류, 설치 위치
            theInGame.m_dragCardKind = int.Parse(this.transform.GetComponent<Image>().sprite.name);
            theInGame.m_slotNum = (int)m_edragDropNum;
            // 카드 종류, 설치 위치
        }
        else
        {
            m_rectTransform.anchoredPosition = new Vector2(0, 0);
            m_canvasGroup.alpha = 1f;
            m_canvasGroup.blocksRaycasts = true;
        }
    }
    
}
