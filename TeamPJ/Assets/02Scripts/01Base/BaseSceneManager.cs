using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseSceneManager : MonoBehaviour
{
    public static BaseSceneManager BaseSceneInstance;

    public enum eLoadingState
    {
        None = 0,
        Start,
        Unloading,
        Loading,
        End
    }

    public enum eStageState
    {
        None = 0,
        Stage01,
        Stage02,
        Stage03,
        Stage04,
        LOBBY,
        INGAME
    }

    [SerializeField] GameObject m_LoadingWnd;

    // public float m_unloadWaitSec = 0;
    // public float m_loadWaitSec = 0;
    public eStageState m_curStage;
    public eLoadingState m_curGameLoad;

    InGameManager theInGame;
    SoundManager theSound;
    FadeManager theFade;
    CameraManager theCamera;
    ShopButton theShop;

    // Start is called before the first frame update
    void Start()
    {
        BaseSceneInstance = this;

        SceneManager.LoadSceneAsync("LobbyManager", LoadSceneMode.Additive);

        m_curGameLoad = eLoadingState.None;
        m_curStage = eStageState.Stage01;

        PlayerPrefs.SetInt("IsClear", 0);      // 0이면 실패, 1이면 클리어


        theSound = FindObjectOfType<SoundManager>();
    }

    

    // 로비 ==> 인게임 씬
    public void UnloadLobbyScene()
    {
        SceneManager.UnloadSceneAsync("LobbyManager");

        SceneManager.LoadSceneAsync("InGameManager", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(m_curStage.ToString(), LoadSceneMode.Additive);
        Invoke("LoadInGameScene", 2);
    }
    public void LoadInGameScene()
    {
        // Debug.Log(m_curStage.ToString());
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_curStage.ToString()));

        // Debug.Log(theSound.m_bgmVolume);
        theSound.PlayBgmSound(SoundManager.eBGM_Type.InGame, theSound.m_bgmVolume, true);
    }
    // 로비 ==> 인게임 씬으


    // 인게임 ==> 인게임 씬
    public void BfUnloadStageScene()
    {
        ////  해당 객체를 찾는다.
        //theInGame = FindObjectOfType<InGameManager>();
        //theFade = FindObjectOfType<FadeManager>();
        //theCamera = FindObjectOfType<CameraManager>();
        //theShop = FindObjectOfType<ShopButton>();
        ////  해당 객체를 찾는다.

        // theFade.SceneFadeOut_Stage();
        FadeManager.FadeInstance.SceneFadeOut_Stage();

        Invoke("UnloadStageScene", 2);
    }
    public void UnloadStageScene()
    {
        SceneManager.UnloadSceneAsync("Stage0" + (int)m_curStage);
        SceneManager.UnloadSceneAsync("InGameManager");

        Invoke("LoadStageScene", 2);
    }
    public void LoadStageScene()
    {
        // m_curStage = eStageState.Stage01;

        SceneManager.LoadSceneAsync("InGameManager", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("Stage0" + (int)m_curStage, LoadSceneMode.Additive);
        
        Invoke("LoadSetStageScene", 2);
    }
    public void LoadSetStageScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Stage0" + (int)m_curStage));

        // theFade.SceneFadeIn_Stage();
        // theCamera.CameraBound();
        // theShop.RefreshCard();

        //// 캐릭터 초기화
        //for (int n = 0; n < theInGame.m_myCard.Count; n++)
        //{
        //    theInGame.m_myCard[n].GetComponent<CharController>().transform.position
        //        = theInGame.m_myCard[n].GetComponent<CharController>().m_originPos;     // 원래 위치로 초기화.

        //    theInGame.m_myCard[n].GetComponent<Animator>().SetInteger("AniState", 0);   // Idle로 다시 전환.
        //}
        //// 캐릭터 초기화

        //theInGame.m_curGameState = InGameManager.eGameState.NxMapsetting;
    }
    // 인게임 ==> 인게임 씬


    // 인게임 ==> 로비 씬
    public void UnloadInGameScene()
    {
        SceneManager.UnloadSceneAsync("Stage0" + (int)m_curStage);
        SceneManager.UnloadSceneAsync("InGameManager");

        SceneManager.LoadSceneAsync("LobbyManager", LoadSceneMode.Additive);
        Invoke("LoadLobbyScene", 2);
    }
    public void LoadLobbyScene()
    {
        m_curStage = eStageState.Stage01;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LobbyManager"));

        theSound.PlayBgmSound(SoundManager.eBGM_Type.Lobby, theSound.m_bgmVolume, true);
    }
    // 인게임 ==> 로비 씬


    /// <summary>
    /// '인게임 ==> 로비' / '인게임 ==> 인게임' 전환하려는 함수.
    /// </summary>
    /// <param name="stage"> 씬 전환될 매개변수 </param>
    public void SceneMoveToLobby(eStageState stage)
    {
        string[] loadStage;
        string[] unloadStage;

        if (stage == eStageState.LOBBY)
        {
            //unloadStage = new string[2];
            //loadStage = new string[1];

            //unloadStage[0] = "Stage0" + (int)m_curStage;
            //unloadStage[1] = "InGameManager";
            //loadStage[0] = "LobbyManager";

            Invoke("UnloadInGameScene", 2f);
        }
        else
        {
            // Debug.Log("stage again");
            //unloadStage = new string[2];
            //loadStage = new string[1];
            //unloadStage[0] = "Stage0" + (int)m_curStage;
            //unloadStage[1] = "InGameManager";
            //loadStage[0] = "Stage0" + (int)stage;

            Invoke("BfUnloadStageScene", 2f);
        }
    }

    /// <summary>
    /// '로비 ==> 인게임'으로 전환되는 함수
    /// </summary>
    /// <param name="stage">씬 전환될 매개변수</param>
    public void SceneMoveToInGame(eStageState stage)
    {
        m_curStage = stage;

        //string[] unloadStage = new string[1];
        //unloadStage[0] = "LobbyManager";

        //string[] loadStage = new string[2];
        //loadStage[0] = "InGameManager";
        //loadStage[1] = stage.ToString();

        Invoke("UnloadLobbyScene", 2f);
    }
}
