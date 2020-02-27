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
    public GameObject[] m_CharSlot;

    RectTransform m_rectTransform;
    CanvasGroup m_canvasGroup;
    
    CardBuyList theBuyList;

    void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_canvasGroup = GetComponent<CanvasGroup>();

        theBuyList = FindObjectOfType<CardBuyList>();
    }
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(InGameManager.InGameInstance.m_curGameState
            == InGameManager.eGameState.ReadyForPlay)
        {
            Debug.Log("OnBeginDrag");
            m_canvasGroup.alpha = .6f;
            m_canvasGroup.blocksRaycasts = false;

            for (int n = 0; n < m_CharSlot.Length; n++)
            {
                if(m_CharSlot[n].GetComponent<CharacterSlot>().m_isSlotOn == false)
                    m_CharSlot[n].SetActive(true);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (InGameManager.InGameInstance.m_curGameState
           == InGameManager.eGameState.ReadyForPlay)
        {
            if (this.transform.GetComponent<Image>().sprite != null)
            {
                m_rectTransform.anchoredPosition += eventData.delta / m_canvas.scaleFactor;
            }

            Debug.Log("OnDrag");
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
            m_canvasGroup.alpha = 1f;
            m_canvasGroup.blocksRaycasts = true;

            for (int n = 0; n < m_CharSlot.Length; n++)
                m_CharSlot[n].SetActive(false);

            Debug.Log("OnEndDrag");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (InGameManager.InGameInstance.m_curGameState
           == InGameManager.eGameState.ReadyForPlay)
        {
            if (theBuyList.m_isEmpty[(int)m_edragDropNum] == true)
                return;

            for (int n = 0; n < 9; n++)
            {
                m_CharSlot[n].GetComponent<CharacterSlot>().m_slotNum
                    = int.Parse(this.transform.GetComponent<Image>().sprite.name);
                
                m_CharSlot[n].GetComponent<CharacterSlot>().m_invenNum
                    = (int)m_edragDropNum;


            }
            Debug.Log("On Pointer Down");
        }
    }
    
}
