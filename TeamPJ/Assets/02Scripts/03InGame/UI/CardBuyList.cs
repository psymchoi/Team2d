using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBuyList : MonoBehaviour
{
    public Sprite[] m_myCardImg;
    public bool[] m_isEmpty;            // 인벤토리를 위한 변수
    public int[] m_InvenNum;            // 슬롯이 채워져 있을 시 고유넘버 부여, 0 은 빈공간
    public string[] m_cardKind;            // 인벤토리에 담긴 카드 종류 번호

    public GameObject m_sortingBtn;

    InGameManager theInGame;

    // Start is called before the first frame update
    void Start()
    {
        theInGame = FindObjectOfType<InGameManager>();

        // m_isEmpty = new bool[this.transform.childCount];            // 지금은 7칸
        // m_InvenNum = new int[this.transform.childCount];            // 고유번호도 7개

        //for (int n = 0; n < this.transform.childCount; n++)
        //{
        //    m_isEmpty[n] = true;                // 다 비어있는 상태
        //   //m_InvenNum[n] = 0;                  // 고유번호 0 은 비어있는 상태
        //}

        m_sortingBtn.GetComponent<Button>().onClick.AddListener(delegate { SortInventory(); });
    }

    void Update()
    {
        if(InGameManager.InGameInstance.m_curGameState
            == InGameManager.eGameState.ReadyForPlay)
        {
            for (int n = 0; n < this.transform.childCount; n++)
            {
                if(m_InvenNum[n] == 100)
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
    public void IncludeContents(Sprite slotImg, int buyNum)
    {
        int n = 0;
        for (n = 0; n < this.transform.childCount; n++)
        {
            if(m_isEmpty[n] == true)
            {
                Debug.Log("CardBuyList n : " + n);

                // 이미지 삽입
                this.transform.GetChild(n).
                    transform.GetChild(0).
                    GetComponent<Image>().sprite = slotImg;
                // 이미지 삽입

                // 어떤 카드 종류인지
                m_cardKind[n] = this.transform.GetChild(n).
                    transform.GetChild(0).
                    GetComponent<Image>().sprite.name;
                // 어떤 카드 종류인지

                // 넘버 부여
                m_InvenNum[n] = n;      // 0 ~ 6 
                // 넘버 부여


                m_isEmpty[n] = false;       // 해당 슬롯은 빈상태가 아닌걸로

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
                        // Debug.Log("m증가");
                        m_isEmpty[m] = true;

                        m++;
                        continue;
                    }
                    
                    tmp1.GetComponent<Image>().sprite = m_myCardImg[int.Parse(tmp2.GetComponent<Image>().sprite.name)];
                    tmp2.GetComponent<Image>().sprite = default;
                    
                    m_InvenNum[nn] = m_InvenNum[m];
                    m_InvenNum[m] = 0;

                    m_cardKind[nn] = tmp2.GetComponent<Image>().sprite.name;

                    m_isEmpty[nn] = false;
                    m_isEmpty[m] = true;


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

                    int tmp = m_InvenNum[n];
                    m_InvenNum[n] = m_InvenNum[m];
                    m_InvenNum[m] = tmp;

                    m_cardKind[n] = tmp2.GetComponent<Image>().sprite.name;
                    m_cardKind[m] = tmp1.GetComponent<Image>().sprite.name;

                }
                m++;
            }

            n++;
            m = n + 1;
        }
        // 버블정렬
    }
}
