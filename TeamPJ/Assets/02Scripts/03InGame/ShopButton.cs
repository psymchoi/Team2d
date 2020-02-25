﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 새로 고침 버튼
        this.transform.Find("RefreshBtn").
                    GetComponent<Button>().onClick.AddListener
                    (delegate { RefreshCard(); });
        // 새로 고침 버튼
        

        // 업그레이드 버튼

        // 업그레이드 버튼
    }
    
    /// <summary>
    /// 자식 객체에 있는 모든 카드 정보를 새로고침 해준다.
    /// </summary>
    public void RefreshCard()
    {
        for (int n = 0; n < transform.Find("MyCardSlot").childCount; n++)
        {
            // 이미지를 다시 열고, 버튼기능 다시 하게 한다.
            transform.Find("MyCardSlot").
            transform.GetChild(n).
            transform.GetChild(0).gameObject.SetActive(true);

            transform.Find("MyCardSlot").
            transform.GetChild(n).
            transform.GetChild(1).gameObject.SetActive(true);

            transform.Find("MyCardSlot").
            transform.GetChild(n).
            GetComponent<Button>().enabled = true;
            // 이미지를 다시 열고, 버튼기능 다시 하게 한다.

            // 카드를 새로고침 해준다.
            transform.Find("MyCardSlot").
            transform.GetChild(n).
            GetComponent<MyCardInfo>().RefreshCard();
            // 카드를 새로고침 해준다. 
        }
    }
}
