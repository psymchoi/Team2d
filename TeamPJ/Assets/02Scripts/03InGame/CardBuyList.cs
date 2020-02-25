using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBuyList : MonoBehaviour
{
    public Sprite[] m_myCardImg;
    public bool[] m_isEmpty;
    public int m_emptyNum;     // 현재 비워져 있는 첫 인벤토리 슬롯번호.
    

    // Start is called before the first frame update
    void Start()
    {
        m_isEmpty = new bool[this.transform.childCount];
        for(int n = 0; n < this.transform.childCount; n++)
        {
            m_isEmpty[n] = true;          // 다 비어있는 상태로
        }
    }
    
    public void IncludeContents(int buyNum)
    {
        int n = 0;
        for (n = 0; n < this.transform.childCount; n++)
        {
            if(m_isEmpty[n] == true)
            {
                this.transform.GetChild(n).
                    transform.GetChild(0).
                    GetComponent<Image>().sprite = m_myCardImg[buyNum];

                m_isEmpty[n] = false;
                m_emptyNum = n;
                break;
            }
        }
        
        if(n == this.transform.childCount)
        {
            Debug.Log("BuyList is Full");
        }
    }
}
