using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBuyList : MonoBehaviour
{
    public Sprite[] m_myCardImg;
    public bool[] m_isEmpty;

    public GameObject[] theDragDrop;

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
                        GetComponent<DragDropImg>().m_invenNum == 0)
                {
                    // 인벤 넘버가 0인 경우에는 빈 공간으로 간주한다.
                    m_isEmpty[n] = true;
                    // 인벤 넘버가 0인 경우에는 빈 공간으로 간주한다.
                }
            }
        }
    }

    /// <summary>
    /// 상점에서 구입한 캐릭터 인벤토리로 옮기는 함수.
    /// </summary>
    /// <param name="buyNum">구매한 캐릭터 번호</param>
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

                // 넘버 부여
                theDragDrop[n].GetComponent<DragDropImg>().m_invenNum = buyNum;
                // 넘버 부여

                m_isEmpty[n] = false;

                break;
            }
        }
        
        if(n == this.transform.childCount)
        {
            Debug.Log("BuyList is Full");
        }
    }


    public void SortInventory()
    {
        // 빈 공간이 있으면 일단 다 앞으로 당긴다.
        int n = 0;
        int m = 1;
        while(n != 7)
        {
            // Debug.Log("n1 : " + n);
            // Debug.Log("m2 : " + m);
            Transform tmp1 = this.transform.GetChild(n).transform.GetChild(0);
            if (tmp1.GetComponent<Image>().sprite == default)
            {
                int nn = n;
                while(m != 7)
                {
                    // Debug.Log("nn : " + nn);
                    // Debug.Log("m : " + m);
                    tmp1 = this.transform.GetChild(nn).transform.GetChild(0);
                    Transform tmp2 = this.transform.GetChild(m).transform.GetChild(0);

                    if (tmp2.GetComponent<Image>().sprite == default)
                    {
                        Debug.Log("m증가");
                        m++;
                        continue;
                    }
                    
                    tmp1.GetComponent<Image>().sprite = m_myCardImg[int.Parse(tmp2.GetComponent<Image>().sprite.name)];
                    tmp2.GetComponent<Image>().sprite = default;
                    
                    tmp1.GetComponent<DragDropImg>().m_invenNum = tmp2.GetComponent<DragDropImg>().m_invenNum;
                    tmp2.GetComponent<DragDropImg>().m_invenNum = 0;

                    nn++;
                    m++;
                }
                break;
            }
            
            n++;
            m = n + 1;
        }
        // 빈 공간이 있으면 일단 다 앞으로 당긴다.

        // 버블정렬
        n = 0;
        m = 1;
        while(n != 7)
        {
            Transform tmp1 = this.transform.GetChild(n).transform.GetChild(0);

            if (tmp1.GetComponent<Image>().sprite == default)
            {
                n++;
                m = n + 1;
                continue;
            }

            int n1 = int.Parse(tmp1.GetComponent<Image>().sprite.name);

            while(m != 7)
            {
                Transform tmp2 = this.transform.GetChild(m).transform.GetChild(0);

                if(tmp2.GetComponent<Image>().sprite == default)
                {
                    m++;
                    continue;
                }

                int n2 = int.Parse(tmp2.GetComponent<Image>().sprite.name);

                // Debug.Log("n11 : " + n1);
                // Debug.Log("n22 : " + n2);

                if(int.Parse(tmp1.GetComponent<Image>().sprite.name) >
                    int.Parse(tmp2.GetComponent<Image>().sprite.name))
                {
                    int tmpSpriteNum = int.Parse(tmp1.GetComponent<Image>().sprite.name);
                    tmp1.GetComponent<Image>().sprite = m_myCardImg[int.Parse(tmp2.GetComponent<Image>().sprite.name)];
                    tmp2.GetComponent<Image>().sprite = m_myCardImg[tmpSpriteNum];

                    int tmpInvenNum = tmp1.GetComponent<DragDropImg>().m_invenNum;
                    tmp1.GetComponent<DragDropImg>().m_invenNum = tmp2.GetComponent<DragDropImg>().m_invenNum;
                    tmp2.GetComponent<DragDropImg>().m_invenNum = tmpInvenNum;
                }
                m++;
            }

            n++;
            m = n + 1;
        }
        // 버블정렬
    }
}
