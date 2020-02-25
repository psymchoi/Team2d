using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            // 인벤토리 위에다 드래그앤 드롭 할 시 원래위치로
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
                = new Vector2(0, 0);
            // 인벤토리 위에다 드래그앤 드롭 할 시 원래위치로
        }
    }
}
