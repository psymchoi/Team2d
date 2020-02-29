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
        NxMapsetting,
        ReadyForPlay,
        WaitPlayTime,
        Play,
        EndPlay,
        Result
    }

    // UI관련 객체
    public GameObject m_shopBtn;
    public GameObject m_settingBtn;
    public GameObject m_optionBtn;
    public GameObject m_backToInGameBtn;
    public GameObject m_backToLobbyBtn;

    public GameObject m_optionPanel;
    public GameObject m_settingPanel;

    public GameObject m_cardInfoPanel;

    public Slider m_timer;
    // UI관련 객체

    // 옵션 bgm, effect 볼륨 조절 슬라이더
    public Slider m_bgmVolume;
    public Slider m_effectVolume;
    // 옵션 bgm, effect 볼륨 조절 슬라이더

    public int m_curStage;
    public Text m_curStageTxt;
    public Text m_warningTxt;

    // 캐릭터 카드 관련
    public int m_cardCount = 6;             // 캐릭터 이미지 개수
    public int m_money;                     // 플레이어 돈
    public Text m_moneyTxt;                 // 플레이어 돈을 보여줄 텍스트
    public GameObject[] m_card;             // 캐릭터 객체
    public GameObject[] m_cardZone;         // 캐릭터 설치존


    public List<GameObject> m_myCard;       // 현재 스폰되어 있는 캐릭터를 담을 리스트

    public int m_dragCardKind;              // 인벤토리에서 선택한 카드 종류 넘버
    public int m_slotNum;                   // 인벤토리 해당 넘버 저장하기위한 변수

    public bool[] m_isActiveMyCard;         // 미리 생성해 놓은 캐릭터 중에서 Active 되어있는 객체 판별변수
    public bool[] m_isCharSlotOn;           // 캐릭터 설치존 열려 있는지 판단여부
    // 캐릭터 카드 관련
    
    public eGameState m_curGameState;
    
    Animator ani;

    float m_timeCheck;

    BaseSceneManager theBase;
    SoundManager theSound;
    FadeManager theFade;
    CameraManager theCamera;
    MyCardInfo theCardInfo;

    // Start is called before the first frame update
    void Start()
    {
        InGameInstance = this;

        m_curGameState = eGameState.Ready;

        theBase = FindObjectOfType<BaseSceneManager>();
        theSound = FindObjectOfType<SoundManager>();
        theFade = FindObjectOfType<FadeManager>();
        theCamera = FindObjectOfType<CameraManager>();
        theCardInfo = FindObjectOfType<MyCardInfo>();

        m_myCard = new List<GameObject>();
        ani = GetComponent<Animator>();

        m_shopBtn.GetComponent<Button>().onClick.AddListener(delegate { ClickShopBtn(); });
        m_settingBtn.GetComponent<Button>().onClick.AddListener(delegate { ClickSettingBtn(); });
        m_optionBtn.GetComponent<Button>().onClick.AddListener(delegate { ClickOptionBtn(); });
        m_backToInGameBtn.GetComponent<Button>().onClick.AddListener(delegate { ClickBackToInGameBtn(); });
        m_backToLobbyBtn.GetComponent<Button>().onClick.AddListener(delegate { ClickExitToLobbyBtn(); });
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
            case eGameState.NxMapsetting:
                NxMapSetting();
                break;
            case eGameState.ReadyForPlay:
                ReadyForGamePlay();
                break;
            case eGameState.WaitPlayTime:
                m_timer.value -= Time.deltaTime / 5;
                if (m_timer.value <= .0f)
                {
                    m_timer.gameObject.SetActive(false);
                    m_curGameState = eGameState.Play;
                }
                break;
            case eGameState.Play:
                GamePlay();
                break;
            case eGameState.EndPlay:
                ResetStage();
                break;
            case eGameState.Result:

                break;
        }
    }


    #region //---- 실제 인게임 관련 함수 ----//
    void GameReady()
    {
        m_curGameState = eGameState.Mapsetting;
        m_timeCheck = 0;
    }

    /// <summary>
    /// 씬 로드할 때 맵 세팅 부분
    /// </summary>
    public void GameMapSetting()
    {
        m_curGameState = eGameState.Mapsetting;

        m_timeCheck = 0;

        m_curStage = 1;
        // Debug.Log("curstage : " + m_curStage);
        m_curStageTxt.text = "Stage " + m_curStage.ToString();

        // 카메라 관련
        theCamera.CameraBound();
        // 카메라 관련

        // Fadein 하는 부분
        theFade.SceneFadeIn2();
        // Fadein 하는 부분

        // UI 관련
        m_money = 200;                                    // 내가 처음 가지고 있을 돈

        m_dragCardKind = 0;                                 // 인벤토리에서 선택한 카드 종류 넘버
        m_slotNum = 0;                                      // 인벤토리 해당 넘버 저장하기위한 변수
        m_isActiveMyCard = new bool[9 * m_card.Length];     // 캐릭터를 각 칸에 맞춰 9개씩 생성
        m_isCharSlotOn = new bool[9];                       // 캐릭터 설치는 9칸

        m_shopBtn.GetComponent<Button>().enabled = true;
        m_timer.value = 1.0f;
        // UI 관련

        //----- 내 카드 관련 -----
        for (int n = 0; n < m_card.Length; n++)
        {
            for(int m = 0; m < 9; m++)
            {
                GameObject go = Instantiate(m_card[n]);             // 캐릭터를 오브젝트 풀 형식으로 생성
                go.transform.parent = this.transform;
                go.transform.position = m_cardZone[m].transform.position;
                go.SetActive(false);
            }
        }

        // 캐릭터가 SetActive 되있는지 여부
        for(int n = 0; n < 9 * m_cardCount; n++)
            m_isActiveMyCard[n] = false;
        // 캐릭터가 SetActive 되있는지 여부
        
        // 캐릭터 설치가능 슬롯여부
        for(int n = 0; n < 9; n++)
            m_isCharSlotOn[n] = true;
        // 캐릭터 설치가능 슬롯여부
        //----- 내 카드 관련 -----



        // Enemy로 된 tag 객체들 모조리 찾아오기.
        GameObject[] enmy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject obj in enmy)
        {
            if (obj != null)
                StageManager.StageInstance.m_tfEnemy.Add(obj);
        }
        // Debug.Log("m_tfEnemy : " + StageManager.StageInstance.m_tfEnemy.Count);
        // Enemy로 된 tag 객체들 모조리 찾아오기.

        m_curGameState = eGameState.ReadyForPlay;
    }

    public void NxMapSetting()
    {
        m_isClear = false;                                      // 클리어했다는걸 다시 초기화.

        m_curStage += 1;
        m_curStageTxt.text = "Stage " + m_curStage.ToString();

        m_timer.value = 1.0f;                                   // Slider 바 초기화.
        m_timer.gameObject.SetActive(true);                     // Slider 켜기.
        m_shopBtn.GetComponent<Button>().enabled = true;        // Shop버튼 활성화.
        StageManager.StageInstance.m_tfEnemy.Clear();

        // Enemy로 된 tag 객체들 모조리 찾아오기.
        GameObject[] enmy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject obj in enmy)
        {
            if (obj != null)
                StageManager.StageInstance.m_tfEnemy.Add(obj);
        }
        // Debug.Log("m_tfEnemy : " + StageManager.StageInstance.m_tfEnemy.Count);
        // Enemy로 된 tag 객체들 모조리 찾아오기.

        m_curGameState = eGameState.ReadyForPlay;
    }

    /// <summary>
    /// 전투 준비 전 단계
    /// </summary>
    public void ReadyForGamePlay()
    {
        if (m_timer.value <= .0f)
        {
            bool isAllEmpty = true;
            for(int n = 0; n < m_isCharSlotOn.Length; n++)
            {
                if (m_isCharSlotOn[n] == false)
                    isAllEmpty = false;
            }
            if(isAllEmpty == true)
            {
                m_curGameState = eGameState.Result;
                StartCoroutine(ShowResultTxt("You Lose..", 2.5f));
                return;
            }

            // 설치존 SetActive 끈다
            for (int n = 0; n < 9; n++)           
                m_cardZone[n].SetActive(false);
            // 설치존 SetActive 끈다
            
            m_timer.value = 1.0f;
            m_shopBtn.GetComponent<Button>().enabled = false;
            OffShopUI();                            // Shop UI 끈다
            theCardInfo.OffInfoPanel();           // 정보패널 끈다
            
            m_curGameState = eGameState.WaitPlayTime;
            return;
        }
        
         m_timer.value -= Time.deltaTime / 20;        
        // Debug.Log(m_timer.value);

        // 실시간 돈 업데이트
        m_moneyTxt.text = m_money.ToString();
        // 실시간 돈 업데이트
    }
    IEnumerator ShowResultTxt(string Txt, float delayTime)
    {
        m_warningTxt.text = Txt;
        yield return new WaitForSeconds(delayTime);
        theFade.SceneFadeOut_ToLobby();
    }

    /// <summary>
    /// 전투 준비 완료 후 단계
    /// </summary>
    public bool m_isClear = false;
    public void GamePlay()
    {
        if(m_isClear == true)
        {
            m_curGameState = eGameState.EndPlay;
        }

        // 실시간 돈 업데이트
        m_moneyTxt.text = m_money.ToString();
        // 실시간 돈 업데이트
    }

    public void ResetStage()
    {
        // Debug.Log("ResetStage");
        BaseSceneManager.BaseSceneInstance.SceneMoveToLobby(BaseSceneManager.eStageState.Stage01);
        m_curGameState = eGameState.Result;
    }
    #endregion



    #region //---- 설정 관련 버튼 ----//
    /// <summary>
    /// 설정창 버튼
    /// </summary>
    public void ClickSettingBtn()
    {
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        m_settingPanel.SetActive(true);

        Time.timeScale = 0.0f;          // 모든 기능 일시정지.
    }

    /// <summary>
    /// '설정창 ==> 인게임'으로 돌아가기
    /// </summary>
    public void ClickBackToInGameBtn()
    {
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        m_settingPanel.SetActive(false);

        Time.timeScale = 1.0f;          // 원래 프레임 단위 속도로 실행.
    }

    public void ClickExitToLobbyBtn()
    {
        Time.timeScale = 1.0f;

        theFade.SceneFadeOut_ToLobby();
    }
    //---- 설정 관련 버튼 ----//

    //---- Shop UI on / off ----//
    public GameObject ShopUI;
    bool m_isOpen = false;

    public void ClickShopBtn()
    {
        Debug.Log("shop click");
        m_isOpen = !m_isOpen;
        ShopUI.GetComponent<Animator>().SetBool("ShopOnOff", m_isOpen);       
    }
    public void OffShopUI()
    {
        m_isOpen = false;
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
