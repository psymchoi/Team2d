using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBuyList : MonoBehaviour
{
    public Sprite[] m_myCardImg;
    public bool[] m_isEmpty;    

    // Start is called before the first frame update
    void Start()
    {
        m_isEmpty = new bool[this.transform.childCount];
        for(int n = 0; n < this.transform.childCount; n++)
        {
            m_isEmpty[n] = true;          // 다 비어있는 상태로
        }
    }

    void Update()
    {
        if(InGameManager.InGameInstance.m_curGameState
            == InGameManager.eGameState.Play)
        {
            for (int n = 0; n < this.transform.childCount; n++)
            {
                if (this.transform.GetChild(n).
                        transform.GetChild(0).
                        GetComponent<Image>().sprite == default)
                {
                    m_isEmpty[n] = true;
                }
            }
        }
    }

    public void IncludeContents(int buyNum)
    {
        int n = 0;
        for (n = 0; n < this.transform.childCount; n++)
        {
            if(m_isEmpty[n] == true)
            {
                // 이미지 삽입
                this.transform.GetChild(n).
                    transform.GetChild(0).
                    GetComponent<Image>().sprite = m_myCardImg[buyNum];
                // 이미지 삽입
                
                m_isEmpty[n] = false;

                break;
            }
        }
        
        if(n == this.transform.childCount)
        {
            Debug.Log("BuyList is Full");
        }
    }
}
