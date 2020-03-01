using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellCharacter : MonoBehaviour
{
    public Vector3 m_CharPosition = Vector3.zero;
    public int m_invenNum;

    CardBuyList theBuyList;
    

    private void Start()
    {
        theBuyList = FindObjectOfType<CardBuyList>();

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_PointerDown);

        EventTrigger.Entry entry_Drag = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_PointerDown);

        EventTrigger.Entry entry_EndDrag = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_PointerDown);
    }


    private void OnPointerDown(PointerEventData data)
    {
        Debug.Log("Pointer Down");
    }

    private void OnDrag(PointerEventData data)
    {
        Debug.Log("Drag");
    }


    //public override void OnDrop(PointerEventData eventData)
    void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrop SellPoint");
        if (eventData.pointerDrag != null)
        {
            theBuyList.m_InvenNum[m_invenNum] = 100;
            eventData.pointerDrag.transform.position = m_CharPosition;
            eventData.pointerDrag.SetActive(false);
        }
    }
    
}
