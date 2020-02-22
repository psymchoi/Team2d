using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyCardInfo : MonoBehaviour
{
    public Sprite[] m_myCardImg;
    public string[] m_myCardTxt;

    // 이 카드의 정보  (0 ~ 5번 닌자 중 랜덤하게 하나 추출넘버)
    int m_myCardNum;
    // 이 카드의 정보  (0 ~ 5번 닌자 중 랜덤하게 하나 추출넘버)

    // Start is called before the first frame update
    void Start()
    {
        m_myCardNum = Random.Range(0, m_myCardImg.Length);

        this.GetComponent<Image>().sprite = m_myCardImg[m_myCardNum];
        this.transform.GetChild(0).GetComponent<Text>().text = m_myCardTxt[m_myCardNum];

        GetComponent<Button>().onClick.AddListener(delegate { Method_Card(m_myCardNum); });
    }

    void Method_Card(int cardNum)
    {
        // 해당 넘버 카드가 눌렸을 때 반응하게 될 함수
        Debug.Log(cardNum);
        // 해당 넘버 카드가 눌렸을 때 반응하게 될 함수
    }

    /// <summary>
    /// 새로 고침 버튼 눌렀을 때 일어나는 이벤트.
    /// </summary>
    public void RefreshCard()
    {
        m_myCardNum = Random.Range(0, m_myCardImg.Length);

        this.GetComponent<Image>().sprite = m_myCardImg[m_myCardNum];
        this.transform.GetChild(0).GetComponent<Text>().text = m_myCardTxt[m_myCardNum];
    }
}
