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
    public GameObject[] m_card;

    List<GameObject> m_myCard;
    // 캐릭터 카드 관련

    public eGameState m_curGameState;

    float m_timeCheck = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        InGameInstance = this;

        theBase = FindObjectOfType<BaseSceneManager>();
        theSound = FindObjectOfType<SoundManager>();
        theFade = FindObjectOfType<FadeManager>();
        theCamera = FindObjectOfType<CameraManager>();

        m_myCard = new List<GameObject>();

        m_curGameState = eGameState.Ready;
    }
    
    // Update is called once per frame
    void Update()
    {
        switch(m_curGameState)
        {
            case eGameState.Ready:
                m_timeCheck += Time.deltaTime;

                if(m_timeCheck >= 2.0f)
                    GameReady();
                break;
            case eGameState.Mapsetting:
                m_timeCheck += Time.deltaTime;

                if (m_timeCheck >= 0.5f)
                    GameMapSetting();
                break;
            case eGameState.Start:
                m_curGameState = eGameState.Play;
                
                break;
            case eGameState.Play:

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


        // 내 카드 관련
        for (int n = 0; n < m_cardCount; n++)
        {
            for (int m = 0; m < 3; m++)
            {
                GameObject go = Instantiate(m_card[n]);
                go.transform.parent = this.transform;
                go.transform.position = this.transform.position;
                go.SetActive(false);

                m_myCard.Add(go);
            }
        }
        // 내 카드 관련

        m_curGameState = eGameState.Start;
    }
    #endregion


    #region //---- 설정 관련 버튼 ----//
    public void ClickSettingBtn()
    {// 설정창 버튼
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        m_settingPanel.SetActive(true);
    }

    public void ClickBackToInGameBtn()
    {// '설정창 ==> 인게임' 버튼
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        m_settingPanel.SetActive(false);
    }
    //---- 설정 관련 버튼 ----//
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
