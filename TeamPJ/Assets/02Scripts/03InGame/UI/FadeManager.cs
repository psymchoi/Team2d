using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    BaseSceneManager theBase;
    SoundManager theSound;

    BaseSceneManager.eStageState m_curStageIdx;

    public GameObject m_exitBtn = null;

    Animator m_animator;

    void Start()
    {
        m_animator = GetComponent<Animator>();

        theBase = FindObjectOfType<BaseSceneManager>();
        theSound = FindObjectOfType<SoundManager>();

        if(m_exitBtn != null)
            m_exitBtn.GetComponent<Button>().onClick.AddListener(delegate { SceneFadeOut2(); });
    }

    //---- 로비 ==> 인게임 ----//
    public void SceneFadeIn2()
    {
        m_animator.SetTrigger("FadeIn");

        //theSound.FadeIn_Bgm();
    }

    public void SceneFadeOut()
    {
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        m_animator.SetTrigger("FadeOut");
        
        // 사운드 서서히 줄어듬.
        theSound.FadeOut_Bgm();    
    }
    
    public void SceneMoveToInGame()
    {
        int stageIdx = 1;
        if ((int)m_curStageIdx != stageIdx)
            m_curStageIdx = (BaseSceneManager.eStageState)stageIdx;
        else
            m_curStageIdx = 0;

        BaseSceneManager.BaseSceneInstance.SceneMoveToInGame(m_curStageIdx);
    }
    //---- 로비 ==> 인게임 ----//

    
    //---- 인게임 ==> 인게임 or 인게임 ==> 로비 ----//
    public void SceneFadeOut2()
    {
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        m_animator.SetTrigger("FadeOut");

        // 사운드 서서히 줄어듬.
        theSound.FadeOut_Bgm();
    }
    public void SceneMoveToLobby()
    {
        theBase.SceneMoveToLobby(BaseSceneManager.eStageState.LOBBY);
    }
    //---- 인게임 ==> 인게임 or 인게임 ==> 로비 ----//
}
