using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseSceneManager : MonoBehaviour
{
    public static BaseSceneManager BaseSceneInstance;
    SoundManager theSound;

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

    public float m_unloadWaitSec = 0;
    public float m_loadWaitSec = 0;
    public eStageState m_curStage;
    public eLoadingState m_curGameLoad;

    public eLoadingState GameLoadState
    {
        set { m_curGameLoad = value; }
        get { return m_curGameLoad; }
    }

    // Start is called before the first frame update
    void Start()
    {
        BaseSceneInstance = this;

        SceneManager.LoadSceneAsync("LobbyManager", LoadSceneMode.Additive);

        m_curGameLoad = eLoadingState.None;
        m_curStage = eStageState.Stage01;

        theSound = FindObjectOfType<SoundManager>();
    }
    
    
    // 로비 ==> 인게임 씬으로
    public void UnloadLobbyScene()
    {
        SceneManager.UnloadSceneAsync("LobbyManager");

        SceneManager.LoadSceneAsync("InGameManager", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(m_curStage.ToString(), LoadSceneMode.Additive);
        Invoke("LoadInGameScene", 3);
    }
    public void LoadInGameScene()
    {
        Debug.Log(m_curStage.ToString());
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_curStage.ToString()));

        // Debug.Log(theSound.m_bgmVolume);
        theSound.PlayBgmSound(SoundManager.eBGM_Type.InGame, theSound.m_bgmVolume, true);
    }
    // 로비 ==> 인게임 씬으로


    // 인게임 ==> 로비 씬으로
    public void UnloadInGameScene()
    {
        SceneManager.UnloadSceneAsync("Stage0" + (int)m_curStage);
        SceneManager.UnloadSceneAsync("InGameManager");

        SceneManager.LoadSceneAsync("LobbyManager", LoadSceneMode.Additive);
        Invoke("LoadLobbyScene", 2);
    }
    public void LoadLobbyScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LobbyManager"));

        Debug.Log(theSound.m_bgmVolume);
        theSound.PlayBgmSound(SoundManager.eBGM_Type.Lobby, theSound.m_bgmVolume, true);
        m_curStage = eStageState.Stage01;
    }
    // 인게임 ==> 로비 씬으로

    
    /// <summary>
    /// '인게임 ==> 로비' 전환하려는 함수.
    /// </summary>
    /// <param name="stage">씬 전환될 매개변수</param>
    public void SceneMoveToLobby(eStageState stage)
    {
        string[] loadStage;
        string[] unloadStage;

        if (stage == eStageState.LOBBY)
        {
            unloadStage = new string[2];
            loadStage = new string[1];

            unloadStage[0] = "Stage0" + (int)m_curStage;
            unloadStage[1] = "InGameManager";
            loadStage[0] = "LobbyManager";
            Invoke("UnloadInGameScene", 2f);
        }
        else
        {
            unloadStage = new string[1];
            loadStage = new string[1];
            unloadStage[0] = "Stage0" + (int)m_curStage;
            loadStage[0] = "Stage0" + (int)stage;
        }
    }

    /// <summary>
    /// '로비 ==> 인게임'으로 전환되는 함수
    /// </summary>
    /// <param name="stage">씬 전환될 매개변수</param>
    public void SceneMoveToInGame(eStageState stage)
    {
        m_curStage = stage;

        string[] unloadStage = new string[1];
        unloadStage[0] = "LobbyManager";

        string[] loadStage = new string[2];
        loadStage[0] = "InGameManager";
        loadStage[1] = stage.ToString();
        
        Invoke("UnloadLobbyScene", 2f);
    }
}
