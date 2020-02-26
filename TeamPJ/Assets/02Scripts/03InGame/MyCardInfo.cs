using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyCardInfo : MonoBehaviour
{
    // 카드의 정보  
    public Sprite[] m_CardImg;
    public string[] m_CardTxt;
    public string[] m_CardInfo;
    public int[] m_CardCost;
    public int[] m_CardNum;   // 번호
    // 카드의 정보  

    public GameObject[] m_shopCard;
    public GameObject m_infoPanel;
    public Text m_infoTxt;
    public Text m_WarningTxt;

    InGameManager theInGame;
    CardBuyList theBuyList;
    SoundManager theSound;
    
    bool setInfo = false;

    // Start is called before the first frame update
    void Start()
    {
        m_CardNum = new int[m_shopCard.Length];     // 이 번호는 랜덤하게 가질 이미지번호. 최대개수는 상점목록 개수(지금은 5개)

        for (int n = 0; n < m_shopCard.Length; n++)
        {
            m_CardNum[n] = Random.Range(0, m_CardImg.Length);

            m_shopCard[n].transform.GetChild(0).GetComponent<Image>().sprite = m_CardImg[m_CardNum[n]];   // 카드이미지
            m_shopCard[n].transform.GetChild(1).GetComponent<Text>().text = m_CardTxt[m_CardNum[n]];      // 카드이름
            m_shopCard[n].transform.GetChild(2).GetComponent<Text>().text = m_CardCost[m_CardNum[n]].ToString();     // 카드가격
        }
        
        m_shopCard[0].transform.GetChild(3).GetComponent<Button>().
                onClick.AddListener(delegate { Method_Info(m_CardNum[0]); });                             // 카드정보
        m_shopCard[1].transform.GetChild(3).GetComponent<Button>().
               onClick.AddListener(delegate { Method_Info(m_CardNum[1]); });                             // 카드정보
        m_shopCard[2].transform.GetChild(3).GetComponent<Button>().
               onClick.AddListener(delegate { Method_Info(m_CardNum[2]); });                             // 카드정보
        m_shopCard[3].transform.GetChild(3).GetComponent<Button>().
               onClick.AddListener(delegate { Method_Info(m_CardNum[3]); });                             // 카드정보
        m_shopCard[4].transform.GetChild(3).GetComponent<Button>().
               onClick.AddListener(delegate { Method_Info(m_CardNum[4]); });                             // 카드정보

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
        // 상점에서 해당 넘버 카드가 눌렸을 때 반응하게 될 함수
        int n;
        for (n = 0; n < theBuyList.m_isEmpty.Length; n++)
        {
            if (theBuyList.m_isEmpty[n] == true)
            {
                theSound.PlayEffSound(SoundManager.eEff_Type.Button);

                // 돈이 부족하면 빠꾸
                if (theInGame.m_Money - m_CardCost[cardNum] <= 0)
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
                theInGame.m_Money -= m_CardCost[cardNum];
                theBuyList.IncludeContents(m_CardImg[cardNum], cardNum);      // 내가 산 카드목록을 채워주기 위한 함수.
                Debug.Log("BuyCard : " + cardNum);
                // 돈 차감 및 인벤토리 추가

                break;
            }
        }

        if (n == 6)
        {
            string warningTxt = "Inventory is Full";
            StartCoroutine(ShowWarningTxt(warningTxt, 2.0f));
        }
        // 상점에서 해당 넘버 카드가 눌렸을 때 반응하게 될 함수
    }

    /// <summary>
    /// 경고 메세지 뜨는 코루틴 함수
    /// </summary>
    /// <param name="Txt">경고 메세지 글</param>
    /// <param name="delayTime">지속시간</param>
    /// <returns></returns>
    IEnumerator ShowWarningTxt(string Txt, float delayTime)
    {
        m_WarningTxt.text = Txt;
        yield return new WaitForSeconds(delayTime);
        m_WarningTxt.text = "";
    }

    public void Method_Info(int infoNum)
    {
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        setInfo = !setInfo;
        m_infoPanel.SetActive(setInfo);
        m_infoTxt.text = m_CardInfo[infoNum];
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
            m_shopCard[n].transform.GetChild(1).GetComponent<Text>().text = m_CardTxt[m_CardNum[n]];                // 카드이름
            m_shopCard[n].transform.GetChild(2).GetComponent<Text>().text = m_CardCost[m_CardNum[n]].ToString();    // 카드가격
        }
    }

    
}
