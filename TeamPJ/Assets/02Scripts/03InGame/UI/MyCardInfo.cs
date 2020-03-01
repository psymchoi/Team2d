using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyCardInfo : MonoBehaviour
{
    public static MyCardInfo CardInfoInstance;

    // 카드의 정보  
    public Sprite[] m_CardImg;      // 캐릭터 이미지
    public string[] m_CardName;     // 캐릭터 이름
    public string[] m_CardInfo;     // 캐릭터 정보
    public int[] m_CardCost;        // 캐릭터 가격
    public int[] m_CardNum;         // 캐릭터 번호
    // 카드의 정보  

    public GameObject[] m_shopCard;
    public GameObject m_infoPanel;
    public Text m_infoTxt;
    public Text m_warningTxt;

    InGameManager theInGame;
    CardBuyList theBuyList;
    SoundManager theSound;
    
    // Start is called before the first frame update
    void Start()
    {
        CardInfoInstance = this;

        m_CardNum = new int[m_shopCard.Length];     // 이 번호는 랜덤하게 가질 이미지번호. 최대개수는 상점목록 개수(지금은 5개)

        for (int n = 0; n < m_shopCard.Length; n++)
        {
            m_CardNum[n] = Random.Range(0, m_CardImg.Length);

            m_shopCard[n].transform.GetChild(0).GetComponent<Image>().sprite = m_CardImg[m_CardNum[n]];             // 카드이미지
            m_shopCard[n].transform.GetChild(1).GetComponent<Text>().text = m_CardName[m_CardNum[n]];               // 카드이름
            m_shopCard[n].transform.GetChild(2).GetComponent<Text>().text = m_CardCost[m_CardNum[n]].ToString();     // 카드가격
        }

        // 이름 / 가격이 "" 일경우에
        bool istrue = true;
        for (int n = 0; n < m_CardName.Length; n++)
        {
            if(m_CardName[n] == "" || 
                m_CardCost[n].ToString() == "" ||
                m_CardInfo[n] == "")
            {
                istrue = false;
                break;
            }
        }


        if (m_CardName.Length != 6 ||
            m_CardCost.Length != 6 ||
            m_CardInfo.Length != 6 ||
            istrue == false)
        {
            m_CardName = new string[6];
            m_CardCost = new int[6];
            m_CardInfo = new string[6];

            m_CardName[0] = "갈색닌자";
            m_CardName[1] = "초록닌자";
            m_CardName[2] = "파랑닌자";
            m_CardName[3] = "회색닌자";
            m_CardName[4] = "빨강닌자";
            m_CardName[5] = "미남닌자";

            m_CardCost[0] = 20;
            m_CardCost[1] = 25;
            m_CardCost[2] = 30;
            m_CardCost[3] = 35;
            m_CardCost[4] = 40;
            m_CardCost[5] = 45;

            for(int n = 0; n < m_shopCard.Length; n++)
            {
                m_shopCard[n].transform.GetChild(1).GetComponent<Text>().text = m_CardName[m_CardNum[n]];
                m_shopCard[n].transform.GetChild(2).GetComponent<Text>().text = m_CardCost[m_CardNum[n]].ToString();

            }

            m_CardInfo[0] = "갈색닌자 이다.";
            m_CardInfo[1] = "초록닌자 이다.";
            m_CardInfo[2] = "파랑닌자 이다.";
            m_CardInfo[3] = "회색닌자 이다.";
            m_CardInfo[4] = "빨강닌자 이다.";
            m_CardInfo[5] = "미남닌자 이다.";
        }
        // 이름 / 가격이 "" 일경우에

       
        m_shopCard[0].transform.GetChild(3).GetComponent<Button>().
                onClick.AddListener(delegate { OnInfoPanel(m_CardNum[0]); });                             // 카드정보
        m_shopCard[1].transform.GetChild(3).GetComponent<Button>().
               onClick.AddListener(delegate { OnInfoPanel(m_CardNum[1]); });                             // 카드정보
        m_shopCard[2].transform.GetChild(3).GetComponent<Button>().
               onClick.AddListener(delegate { OnInfoPanel(m_CardNum[2]); });                             // 카드정보
        m_shopCard[3].transform.GetChild(3).GetComponent<Button>().
               onClick.AddListener(delegate { OnInfoPanel(m_CardNum[3]); });                             // 카드정보
        m_shopCard[4].transform.GetChild(3).GetComponent<Button>().
               onClick.AddListener(delegate { OnInfoPanel(m_CardNum[4]); });                             // 카드정보

        m_shopCard[0].GetComponent<Button>().onClick.AddListener
            (delegate { Method_Card(m_shopCard[0], m_CardNum[0]); });
        m_shopCard[1].GetComponent<Button>().onClick.AddListener
            (delegate { Method_Card(m_shopCard[1], m_CardNum[1]); });
        m_shopCard[2].GetComponent<Button>().onClick.AddListener
            (delegate { Method_Card(m_shopCard[2], m_CardNum[2]); });
        m_shopCard[3].GetComponent<Button>().onClick.AddListener
            (delegate { Method_Card(m_shopCard[3], m_CardNum[3]); });
        m_shopCard[4].GetComponent<Button>().onClick.AddListener
            (delegate { Method_Card(m_shopCard[4], m_CardNum[4]); });

        theInGame = FindObjectOfType<InGameManager>();
        theBuyList = FindObjectOfType<CardBuyList>();
        theSound = FindObjectOfType<SoundManager>();
    }

    /// <summary>
    /// 상점에서 캐릭터 구매 버튼.
    /// </summary>
    /// <param name="cardNum"> 해당 캐릭터 고유넘버 </param>
    void Method_Card(GameObject shop, int cardNum)
    {
        //----- 상점에서 해당 넘버 카드가 눌렸을 때 반응하게 될 함수 -----
        int count = 0;
        for (int n = 0; n < theBuyList.m_isEmpty.Length; n++)
        {
            if (theBuyList.m_isEmpty[n] == false)
            {
                count++;
            }
        }
        // 인벤토리가 꽉 찼다는 메세지
        if(count == 7)
        {
            string warningTxt = "Inventory is Full";
            StartCoroutine(ShowWarningTxt(warningTxt, 2.0f));
            return;
        }
        // 인벤토리가 꽉 찼다는 메세지
        
        for (int n = 0; n < theBuyList.m_isEmpty.Length; n++)
        {
            if (theBuyList.m_isEmpty[n] == true)
            {
                Debug.Log("MyCardInfo n : " + n);
                theSound.PlayEffSound(SoundManager.eEff_Type.Button);

                // 돈이 부족하면 빠꾸
                if (theInGame.m_money - m_CardCost[cardNum] <= 0)
                {
                    string warningTxt = "Not Enough Money";
                    StartCoroutine(ShowWarningTxt(warningTxt, 2.0f));
                    break;
                }
                // 돈이 부족하면 빠꾸

                // 이미지를 가리고, 버튼기능을 못하게 한다.
                shop.transform.GetChild(0).gameObject.SetActive(false);
                shop.transform.GetChild(1).gameObject.SetActive(false);
                shop.transform.GetChild(2).gameObject.SetActive(false);
                shop.transform.GetChild(3).gameObject.SetActive(false);
                shop.transform.GetChild(4).gameObject.SetActive(false);
                shop.GetComponent<Button>().enabled = false;
                m_infoPanel.SetActive(false);
                // 이미지를 가리고, 버튼기능을 못하게 한다.
                

                // 돈 차감 및 인벤토리 추가
                theInGame.m_money -= m_CardCost[cardNum];
                theBuyList.IncludeContents(m_CardImg[cardNum], cardNum);      // 내가 산 카드목록을 채워주기 위한 함수.
                Debug.Log("BuyCard : " + cardNum);               
                // 돈 차감 및 인벤토리 추가
                
                break;
            }
        }
        
        //----- 상점에서 해당 넘버 카드가 눌렸을 때 반응하게 될 함수 -----
    }

    /// <summary>
    /// 경고 메세지 뜨는 코루틴 함수
    /// </summary>
    /// <param name="Txt">경고 메세지 글</param>
    /// <param name="delayTime">지속시간</param>
    /// <returns></returns>
    IEnumerator ShowWarningTxt(string Txt, float delayTime)
    {
        m_warningTxt.text = Txt;
        yield return new WaitForSeconds(delayTime);
        m_warningTxt.text = "";
    }

    bool isOpen = false;
    public void OnInfoPanel(int infoNum)
    {
        isOpen = !isOpen;
        m_infoPanel.SetActive(true);
        m_infoTxt.text = m_CardInfo[infoNum];
    }
    public void OffInfoPanel()
    {
        isOpen = false;
        m_infoPanel.SetActive(false);
    }

    /// <summary>
    /// 새로 고침 버튼 눌렀을 때 일어나는 이벤트.
    /// </summary>
    public void RefreshCard()
    {
        for (int n = 0; n < m_shopCard.Length; n++)
        {
            m_CardNum[n] = Random.Range(0, m_CardImg.Length);

            m_shopCard[n].transform.GetChild(0).GetComponent<Image>().sprite = m_CardImg[m_CardNum[n]];             // 카드이미지
            m_shopCard[n].transform.GetChild(1).GetComponent<Text>().text = m_CardName[m_CardNum[n]];                // 카드이름
            m_shopCard[n].transform.GetChild(2).GetComponent<Text>().text = m_CardCost[m_CardNum[n]].ToString();    // 카드가격
        }
    }


    // 캐릭터를 팔 때 관련 함수.
    [SerializeField] GameObject[] m_sellBtn;
    [SerializeField] GameObject m_iventorySlot;
    public int sellCost;

    public void SellOn(int charNum, int cLevel)
    {
        sellCost = (m_CardCost[charNum] / 10) * cLevel;

        m_sellBtn[0].SetActive(true);
        m_sellBtn[1].SetActive(true);
        m_sellBtn[0].transform.GetChild(0).GetComponent<Text>().text 
            = string.Format("판매골드\n{0}", sellCost.ToString());
        m_sellBtn[1].transform.GetChild(0).GetComponent<Text>().text
            = string.Format("판매골드\n{0}", sellCost.ToString());

        m_iventorySlot.SetActive(false);            // 내 인벤토리창 off
    }

    public void SellOff()
    {
        m_sellBtn[0].SetActive(false);
        m_sellBtn[1].SetActive(false);
        m_iventorySlot.SetActive(true);
    }
    // 캐릭터를 팔 때 관련 함수.
}
