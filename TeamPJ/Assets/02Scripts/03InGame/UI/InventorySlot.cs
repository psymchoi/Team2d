using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 떨어뜨릴 때 인벤토리 범위라면 인벤토리에 다시금 올려놓게하는 스크립트
public class InventorySlot : MonoBehaviour, IPointerDownHandler, IDropHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        InGameManager.InGameInstance.OffShopUI();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop InvenSlot");
        if (eventData.pointerDrag != null)
        {           
            // 인벤토리 위에다 드래그앤 드롭 할 시 원래위치로
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                = new Vector2(0, 0);
            // 인벤토리 위에다 드래그앤 드롭 할 시 원래위치로          
        }
    }
}
