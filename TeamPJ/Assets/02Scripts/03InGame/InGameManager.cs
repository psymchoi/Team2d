using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    public static InGameManager InGameInstance;

    public enum eGameState
    {
        None,
        Ready,
        Mapsetting,
        ReadyForPlay,
        Play,
        EndPlay,
        Result
    }

    BaseSceneManager theBase;
    SoundManager theSound;
    FadeManager theFade;
    CameraManager theCamera;

    // UI관련 객체
    public GameObject m_settingBtn;
    public GameObject m_shopBtn;

    public GameObject m_optionPanel;
    public GameObject m_settingPanel;

    public Slider m_timer;
    // UI관련 객체

    // 옵션 bgm, effect 볼륨 조절 슬라이더
    public Slider m_bgmVolume;
    public Slider m_effectVolume;
    // 옵션 bgm, effect 볼륨 조절 슬라이더

    // 캐릭터 카드 관련
    public int m_cardCount = 6;
    public int m_Money;
    public Text m_MoneyTxt;
    public GameObject[] m_card;
    public GameObject[] m_cardPos;
    public Vector3[] m_charPos;

    [SerializeField] List<GameObject> m_myCard;

    public bool[] m_isActiveMyCard;
    // 캐릭터 카드 관련
    
    public eGameState m_curGameState;

    Animator ani;
    float m_timeCheck;

    // Start is called before the first frame update
    void Start()
    {
        InGameInstance = this;

        m_curGameState = eGameState.Ready;

        theBase = FindObjectOfType<BaseSceneManager>();
        theSound = FindObjectOfType<SoundManager>();
        theFade = FindObjectOfType<FadeManager>();
        theCamera = FindObjectOfType<CameraManager>();

        m_myCard = new List<GameObject>();
        ani = GetComponent<Animator>();

        m_settingBtn.GetComponent<Button>().onClick.AddListener(delegate { ClickSettingBtn(); });
        m_shopBtn.GetComponent<Button>().onClick.AddListener(delegate { ClickShopBtn(); });
    }
    
    // Update is called once per frame
    void Update()
    {
        switch(m_curGameState)
        {
            case eGameState.Ready:
                GameReady();
                break;
            case eGameState.Mapsetting:
                m_timeCheck += Time.deltaTime;
                
                if(m_timeCheck >= 2.0f)
                    GameMapSetting();
                break;
            case eGameState.ReadyForPlay:
                ReadyForGamePlay();
                break;
            case eGameState.Play:
                GamePlay();
                break;
            case eGameState.EndPlay:

                break;
            case eGameState.Result:

                break;
        }
    }


    #region //---- 실제 인게임 관련 함수 ----//
    void GameReady()
    {
        // theSound.FadeIn_Bgm();
        m_curGameState = eGameState.Mapsetting;
        m_timeCheck = 0;
    }

    public void GameMapSetting()
    {
        m_curGameState = eGameState.Mapsetting;

        m_timeCheck = 0;

        // 카메라 관련
        theCamera.CameraBound();
        // 카메라 관련

        // Fadein Screen
        theFade.SceneFadeIn2();
        // Fadein Screen

        // UI 관련
        m_Money = 20000;

        m_isActiveMyCard = new bool[9 * m_cardCount];

        m_shopBtn.GetComponent<Button>().enabled = true;
        m_timer.value = 1.0f;
        // UI 관련

        // 내 카드 관련
        m_charPos = new Vector3[m_card.Length];
        for (int n = 0; n < m_cardCount; n++)
        {
            for(int m = 0; m < 9; m++)
            {
                GameObject go = Instantiate(m_card[n]);
                go.transform.parent = this.transform;
                m_charPos[n] = go.transform.position = m_cardPos[m].transform.position;
                go.SetActive(false);

                m_myCard.Add(go);
            }
        }

        for(int n = 0; n < 9 * m_cardCount; n++)
        {
            m_isActiveMyCard[n] = false;
        }
        // 내 카드 관련

        m_curGameState = eGameState.ReadyForPlay;
    }

    public void ReadyForGamePlay()
    {
        if (m_timer.value <= .0f)
        {
            m_timer.value = 1.0f;
            m_timer.gameObject.SetActive(false);
            m_shopBtn.GetComponent<Button>().enabled = false;
            OffShopUI();

            m_curGameState = eGameState.Play;
            return;
        }

        
         m_timer.value -= Time.deltaTime / 50;
        
        // Debug.Log(m_timer.value);

        m_MoneyTxt.text = m_Money.ToString();
    }

    public void GamePlay()
    {
        m_MoneyTxt.text = m_Money.ToString();
    }
    #endregion

    

    #region //---- 설정 관련 버튼 ----//
    public void ClickSettingBtn()
    {// 설정창 버튼
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        m_settingPanel.SetActive(true);

        Time.timeScale = 0.0f;          // 모든 기능 일시정지.
    }

    public void ClickBackToInGameBtn()
    {// '설정창 ==> 인게임' 버튼
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        m_settingPanel.SetActive(false);

        Time.timeScale = 1.0f;          // 원래 프레임 단위 속도로 실행.
    }
    //---- 설정 관련 버튼 ----//

    //---- Shop UI on / off ----//
    public GameObject ShopUI;
    bool m_isOpen = false;

    public void ClickShopBtn()
    {
        m_isOpen = !m_isOpen;
        ShopUI.GetComponent<Animator>().SetBool("ShopOnOff", m_isOpen);       
    }
    public void OffShopUI()
    {
        ShopUI.GetComponent<Animator>().SetBool("ShopOnOff", false);
    }
    //---- Shop UI on / off ----//
    #endregion


    #region  //---- 옵션 관련 버튼 ----//
    public void ClickOptionBtn()
    {// '설정창 ==> 옵션' 버튼
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        theSound.m_bgmVolume = m_bgmVolume.value = theSound.m_bgmPlayer.volume;
        m_effectVolume.value = theSound.m_effectVolume;

        m_settingPanel.SetActive(false);
        m_optionPanel.SetActive(true);
    }

    public void ClickBackToSettingBtn()
    {// '옵션 ==> 세팅' 버튼
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        m_settingPanel.SetActive(true);
        m_optionPanel.SetActive(false);
    }

    public void CurBgmVolume()
    {// bgm 음향 조절
        theSound.m_bgmVolume = theSound.m_bgmPlayer.volume = m_bgmVolume.value;
        Debug.Log(theSound.m_bgmPlayer.volume);
    }
    public void CurEffectVolume()
    {// 이펙트 음향 조절
        theSound.m_effectVolume = m_effectVolume.value;
        Debug.Log(theSound.m_effectVolume);
    }
    //---- 옵션 관련 버튼 ----//
    #endregion
}
