using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseSceneManager : MonoBehaviour
{
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

    eLoadingState m_curGameLoad;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadSceneAsync("LobbyManager", LoadSceneMode.Additive);

        m_curGameLoad = eLoadingState.None;
        m_curStage = eStageState.Stage01;

        theSound = FindObjectOfType<SoundManager>();
    }

    /// <summary>
    /// 비동기화 방식의 '씬 로딩/언로딩' 하는 코루틴함수
    /// </summary>
    /// <param name="loadName">로딩할 씬 이름</param>
    /// <param name="unloadName">언로딩할 씬 이름</param>
    /// <returns></returns>
    //IEnumerator LoadingScene(string[] loadName = null, string[] unloadName = null)
    //{
    //    AsyncOperation AO;

    //    GameObject go = Instantiate(m_LoadingWnd);       
    //    LoadingWnd theWnd = go.GetComponent<LoadingWnd>();

    //    int amount;

    //    m_curGameLoad = eLoadingState.Unloading;
    //    if (unloadName == null)
    //        amount = 0;
    //    else
    //        amount = unloadName.Length;

    //    for(int n = 0; n < amount; n++)
    //    {
    //        SceneManager.UnloadSceneAsync(unloadName[n]);
    //        //AO = SceneManager.UnloadSceneAsync(unloadName[n]);
    //        //while (!AO.isDone)
    //        //{
    //        //    theWnd.SliderState((AO.progress / amount) + (n / amount));
    //        //    yield return null;
    //        //}
    //        //yield return new WaitForSeconds(m_unloadWaitSec);
    //    }
        
    //    m_curGameLoad = eLoadingState.Loading;
    //    if (loadName == null)
    //        amount = 0;
    //    else
    //        amount = loadName.Length;

    //    for(int n = 0; n < amount; n++)
    //    {
    //        AO = SceneManager.LoadSceneAsync(loadName[n], LoadSceneMode.Additive);
    //        while (!AO.isDone)
    //        {
    //            theWnd.SliderState((AO.progress / amount) + (n / amount));
    //           yield return null;
    //        }
    //        yield return new WaitForSeconds(m_loadWaitSec);
    //    }

    //    SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadName[amount - 1]));

    //    // 스테이지마다 바뀔 BGM 사운드 설정    
    //    if (m_curStage == eStageState.Lobby)
    //    {
    //        theSound.PlayBgmSound(SoundManager.eBGM_Type.Lobby, theSound.m_bgmVolume, true);
    //    }
    //    else if(m_curStage == eStageState.InGame)
    //    {
    //        theSound.PlayBgmSound(SoundManager.eBGM_Type.InGame, theSound.m_bgmVolume, true);
    //    }

    //    m_curGameLoad = eLoadingState.End;

    //    //if (go != null)
    //    {
    //        Destroy(theWnd.gameObject);
    //    }
    //}

    IEnumerator StageToLobby(string[] loadName = null, string[] unloadName = null)
    {
        int amount;
        if (unloadName == null)
            amount = 0;
        else
            amount = unloadName.Length;

        m_curGameLoad = eLoadingState.Unloading;
        for (int n = 0; n < amount; n++)
        {
            SceneManager.UnloadSceneAsync(unloadName[n]);
            yield return new WaitForSeconds(m_unloadWaitSec);
        }

        if (loadName == null)
            amount = 0;
        else
            amount = loadName.Length;

        m_curGameLoad = eLoadingState.Loading;
        for (int n = 0; n < amount; n++)
        {
            SceneManager.LoadSceneAsync(loadName[n], LoadSceneMode.Additive);
            yield return new WaitForSeconds(m_loadWaitSec);
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadName[amount - 1]));

        if (m_curStage == eStageState.LOBBY)
        {
            theSound.PlayBgmSound(SoundManager.eBGM_Type.Lobby, theSound.m_bgmVolume, true);
        }
        else
        {
            theSound.PlayBgmSound(SoundManager.eBGM_Type.InGame, theSound.m_bgmVolume, true);
        }

        m_curGameLoad = eLoadingState.End;

        // yield return null;
    }

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
            Debug.Log(m_curStage);
            unloadStage = new string[2];
            loadStage = new string[1];

            unloadStage[0] = "Stage0" + (int)m_curStage;
            unloadStage[1] = "InGameManager";
            loadStage[0] = "LobbyManager";
        }
        else
        {
            unloadStage = new string[1];
            loadStage = new string[1];
            unloadStage[0] = "Stage0" + (int)m_curStage;
            loadStage[0] = "Stage0" + (int)stage;
        }
        
        m_curStage = stage;
        StartCoroutine(StageToLobby(loadStage, unloadStage));
    }

    /// <summary>
    /// '로비 ==> 인게임'으로 전환되는 함수
    /// </summary>
    /// <param name="stage">씬 전환될 매개변수</param>
    public void SceneMoveToInGame(eStageState stage)
    {
        m_curStage = stage;
        Debug.Log(m_curStage);

        string[] unloadStage = new string[1];
        unloadStage[0] = "LobbyManager";

        string[] loadStage = new string[2];
        loadStage[0] = "InGameManager";
        loadStage[1] = stage.ToString();
        Debug.Log(stage.ToString());
        
        StartCoroutine(StageToLobby(loadStage, unloadStage));
    }
}
