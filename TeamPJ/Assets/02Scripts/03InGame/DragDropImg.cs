using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropImg : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
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

    RectTransform m_rectTransform;
    CanvasGroup m_canvasGroup;
    
    public GameObject[] m_CharSlot;

    CardBuyList theCardList;

    void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_canvasGroup = GetComponent<CanvasGroup>();

        theCardList = FindObjectOfType<CardBuyList>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        m_canvasGroup.alpha = .6f;
        m_canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        if (this.transform.GetComponent<Image>().sprite != null)
            m_rectTransform.anchoredPosition += eventData.delta / m_canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        m_canvasGroup.alpha = 1f;
        m_canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        
        
        for(int n = 0; n < 9; n++)
        {
            m_CharSlot[n].GetComponent<CharacterSlot>().m_slotNum 
                = int.Parse(this.transform.GetComponent<Image>().sprite.name);
            

            m_CharSlot[n].GetComponent<CharacterSlot>().m_invenNum
                = (int)m_edragDropNum;

            Debug.Log("CharacterSlot m_invenNum : " + m_CharSlot[n].GetComponent<CharacterSlot>().m_invenNum);
        }
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
