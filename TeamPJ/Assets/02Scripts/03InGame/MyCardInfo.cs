using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MyCardInfo : MonoBehaviour
{
    public Sprite[] m_CardImg;
    public string[] m_CardTxt;
    public string[] m_CardInfo;
    public GameObject m_infoPanel;
    public Text m_infoTxt;
    public Text m_WarningTxt;

    InGameManager theInGame;
    CardBuyList theBuyList;
    SoundManager theSound;

    // 이 카드의 정보  (0 ~ n번 닌자 중 랜덤하게 하나 추출넘버)
    public int m_CardNum;          // 번호
    int m_CardCost;         // 가격
    // 이 카드의 정보  (0 ~ n번 닌자 중 랜덤하게 하나 추출넘버)

    bool setInfo = false;

    // Start is called before the first frame update
    void Start()
    {
        m_CardNum = Random.Range(0, m_CardImg.Length);

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

        this.transform.GetChild(0).GetComponent<Image>().sprite = m_CardImg[m_CardNum];   // 카드이미지
        this.transform.GetChild(1).GetComponent<Text>().text = m_CardTxt[m_CardNum];      // 카드이름
        this.transform.GetChild(2).GetComponent<Text>().text = m_CardCost.ToString();     // 카드가격
        this.transform.GetChild(3).GetComponent<Button>().
            onClick.AddListener(delegate { Method_Info(); });                             // 카드정보

        GetComponent<Button>().onClick.AddListener(delegate { Method_Card(m_CardNum); });


        theInGame = FindObjectOfType<InGameManager>();
        theBuyList = FindObjectOfType<CardBuyList>();
        theSound = FindObjectOfType<SoundManager>();
    }

    void Method_Card(int cardNum)
    {
        // 해당 넘버 카드가 눌렸을 때 반응하게 될 함수
        if(theBuyList.m_isEmpty[theBuyList.m_isEmpty.Length - 1] == true)
        {
            if (theInGame.m_Money - m_CardCost >= 0)
            {
                theSound.PlayEffSound(SoundManager.eEff_Type.Button);

                // 이미지를 가리고, 버튼기능을 못하게 한다.
                this.transform.GetChild(0).gameObject.SetActive(false);
                this.transform.GetChild(1).gameObject.SetActive(false);
                this.transform.GetChild(2).gameObject.SetActive(false);
                this.transform.GetChild(3).gameObject.SetActive(false);
                this.transform.GetChild(4).gameObject.SetActive(false);
                this.GetComponent<Button>().enabled = false;
                m_infoPanel.SetActive(false);
                // 이미지를 가리고, 버튼기능을 못하게 한다.

                theInGame.m_Money -= m_CardCost;
                theBuyList.IncludeContents(m_CardNum);      // 내가 산 카드목록을 채워주기 위한 함수.
            }
            else
            {
                string warningTxt = "Not Enough Money";
                StartCoroutine(ShowWarningTxt(warningTxt, 2.0f));
            }
        }
        else
        {
            string warningTxt = "Inventory is Full";
            StartCoroutine(ShowWarningTxt(warningTxt, 2.0f));
        }
        // 해당 넘버 카드가 눌렸을 때 반응하게 될 함수
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

    public void Method_Info()
    {
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        setInfo = !setInfo;
        m_infoPanel.SetActive(setInfo);
        m_infoTxt.text = m_CardInfo[m_CardNum];
    }

    /// <summary>
    /// 새로 고침 버튼 눌렀을 때 일어나는 이벤트.
    /// </summary>
    public void RefreshCard()
    {
        m_CardNum = Random.Range(0, m_CardImg.Length);

        this.transform.GetChild(0).GetComponent<Image>().sprite = m_CardImg[m_CardNum];
        this.transform.GetChild(1).GetComponent<Text>().text = m_CardTxt[m_CardNum];
        this.transform.GetChild(2).GetComponent<Text>().text = m_CardCost.ToString();
    }

    
}
