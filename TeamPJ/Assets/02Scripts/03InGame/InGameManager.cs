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
        Start,
        Play,
        EndPlay,
        Result
    }

    BaseSceneManager theBase;
    SoundManager theSound;
    FadeManager theFade;
    CameraManager theCamera;

    // Setting 버튼 관련 Obj
    public GameObject m_optionPanel;
    public GameObject m_settingPanel;
    // Setting 버튼 관련 Obj

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

    public List<GameObject> m_myCard;

    public bool[] m_isActiveMyCard;
    // 캐릭터 카드 관련
    
    public eGameState m_curGameState;
    public Text t;

    Animator ani;
    float m_timeCheck;

    // Start is called before the first frame update
    void Start()
    {
        InGameInstance = this;

        theBase = FindObjectOfType<BaseSceneManager>();
        theSound = FindObjectOfType<SoundManager>();
        theFade = FindObjectOfType<FadeManager>();
        theCamera = FindObjectOfType<CameraManager>();

        m_myCard = new List<GameObject>();

        m_isActiveMyCard = new bool[9 * m_cardCount];

        m_curGameState = eGameState.Ready;

        ani = GetComponent<Animator>();
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
            case eGameState.Start:
                m_curGameState = eGameState.Play;
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

        // 게임 UI / 변수 관련
        m_Money = 20000;
        // 게임 UI / 변수 관련

        // 내 카드 관련
        for (int n = 0; n < m_cardCount; n++)
        {
            for(int m = 0; m < 9; m++)
            {
                GameObject go = Instantiate(m_card[n]);
                go.transform.parent = this.transform;
                go.transform.position = m_cardPos[m].transform.position;
                go.SetActive(false);

                m_myCard.Add(go);
            }
        }

        for(int n = 0; n < 9 * m_cardCount; n++)
        {
            m_isActiveMyCard[n] = false;
        }
        // 내 카드 관련

        m_curGameState = eGameState.Start;
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

    //---- SHOP 테스트 ----//
    bool a = false;
    public GameObject ShopUI;
    public void TestShop()
    {
        a = !a;
        ShopUI.GetComponent<Animator>().SetBool("ShopOnOff", a);       
    }
    //---- SHOP 테스트 ----//
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
