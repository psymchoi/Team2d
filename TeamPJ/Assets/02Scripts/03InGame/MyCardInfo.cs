using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCardInfo : MonoBehaviour
{
    public Sprite[] m_myCardImg;
    public string[] m_myCardTxt;

    InGameManager theInGame;
    CardBuyList theBuyList;

    // 이 카드의 정보  (0 ~ n번 닌자 중 랜덤하게 하나 추출넘버)
    int m_CardNum;          // 번호
    int m_CardCost;         // 가격
    // 이 카드의 정보  (0 ~ n번 닌자 중 랜덤하게 하나 추출넘버)

    // Start is called before the first frame update
    void Start()
    {
        m_CardNum = Random.Range(0, m_myCardImg.Length);

        switch(m_CardNum)
        {
            case 0:
                m_CardCost = 50;
                break;
            case 1:
                m_CardCost = 40;
                break;
            case 2:
                m_CardCost = 60;
                break;
            case 3:
                m_CardCost = 50;
                break;
            case 4:
                m_CardCost = 30;
                break;
            case 5:
                m_CardCost = 50;
                break;
            default:
                Debug.Log("Can't Find m_CardNum");
                break;
        }

        this.transform.GetChild(0).GetComponent<Image>().sprite = m_myCardImg[m_CardNum];
        this.transform.GetChild(1).GetComponent<Text>().text = m_myCardTxt[m_CardNum];

        GetComponent<Button>().onClick.AddListener(delegate { Method_Card(m_CardNum); });

        theInGame = FindObjectOfType<InGameManager>();
        theBuyList = FindObjectOfType<CardBuyList>();
    }

    void Method_Card(int cardNum)
    {
        // 해당 넘버 카드가 눌렸을 때 반응하게 될 함수
        if(theBuyList.m_isEmpty[theBuyList.m_isEmpty.Length - 1] == true)
        {
            if (theInGame.m_Money - m_CardCost >= 0)
            {
                // 이미지를 가리고, 버튼기능을 못하게 한다.
                this.transform.GetChild(0).gameObject.SetActive(false);
                this.transform.GetChild(1).gameObject.SetActive(false);
                this.GetComponent<Button>().enabled = false;
                // 이미지를 가리고, 버튼기능을 못하게 한다.

                theInGame.m_Money -= m_CardCost;
                theBuyList.IncludeContents(m_CardNum);      // 내가 산 카드목록을 채워주기 위한 함수.
            }
            else
            {
                Debug.Log("Not Enough Money");
            }
        }
        else
        {
            Debug.Log("Inventory is Full");
        }
        // 해당 넘버 카드가 눌렸을 때 반응하게 될 함수
    }

    /// <summary>
    /// 새로 고침 버튼 눌렀을 때 일어나는 이벤트.
    /// </summary>
    public void RefreshCard()
    {
        m_CardNum = Random.Range(0, m_myCardImg.Length);

        this.transform.GetChild(0).GetComponent<Image>().sprite = m_myCardImg[m_CardNum];
        this.transform.GetChild(1).GetComponent<Text>().text = m_myCardTxt[m_CardNum];
    }
}
