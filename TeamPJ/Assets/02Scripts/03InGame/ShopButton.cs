using System.Collections;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 자식 객체에 있는 모든 카드 정보를 새로고침 해준다.
    /// </summary>
    public void RefreshCard()
    {
        for (int n = 0; n < transform.Find("MyCardSlot").childCount; n++)
        {
            // 카드를 새로고침 해준다.
            transform.Find("MyCardSlot").
            transform.GetChild(n).GetComponent<MyCardInfo>().RefreshCard();
            // 카드를 새로고침 해준다.
        }
    }
}
