using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    SoundManager theSound;

    // Start is called before the first frame update
    void Start()
    {
        theSound = FindObjectOfType<SoundManager>();

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
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        for (int n = 0; n < transform.Find("MyCardSlot").childCount; n++)
        {
            // 이미지를 다시 열고, 버튼기능 다시 하게 한다.
            for(int m = 0; m < 5; m ++)
            {
                transform.Find("MyCardSlot").
                    transform.GetChild(n).
                    transform.GetChild(m).gameObject.SetActive(true);
            }

            transform.Find("MyCardSlot").
            transform.GetChild(n).
            GetComponent<Button>().enabled = true;
            // 이미지를 다시 열고, 버튼기능 다시 하게 한다.

        }

        // 카드를 새로고침 해준다.
        transform.Find("MyCardSlot").
                GetComponent<MyCardInfo>().RefreshCard();
        // 카드를 새로고침 해준다. 
    }
}
