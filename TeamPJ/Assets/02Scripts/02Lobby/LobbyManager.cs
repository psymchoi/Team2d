using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    SoundManager theSound;

    public GameObject m_associateLobbyBtn;
    public GameObject m_optionBackBtn;
    public GameObject m_achievementBackBtn;

    public GameObject m_optionPanel;
    public GameObject m_achievementPanel;

    public Slider m_bgmVolume;
    public Slider m_effectVolume;

    void Start()
    {
        theSound = FindObjectOfType<SoundManager>();
       //m_optionPanel.SetActive(false);
    }

    // 옵션 관련 버튼 //
    public void ClickOptionBtn()
    {// 로비 '옵션' 버튼
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        theSound.m_bgmVolume = m_bgmVolume.value = theSound.m_bgmPlayer.volume;    
        m_effectVolume.value = theSound.m_effectVolume;

        m_associateLobbyBtn.SetActive(false);
        m_optionPanel.SetActive(true);
    }
    public void ClickOptionBackBtn()
    {// 뒤로가기 버튼
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        m_associateLobbyBtn.SetActive(true);
        m_optionPanel.SetActive(false);
    }

    public void CurBgmVolume()
    {// bgm 음향 조절
        theSound.m_bgmVolume = theSound.m_bgmPlayer.volume = m_bgmVolume.value;
    }

    public void CurEffectVolume()
    {// 이펙트 음향 조절
        theSound.m_effectVolume = m_effectVolume.value;
        Debug.Log(theSound.m_effectVolume);
    }

    // 옵션 관련 버튼 //

    
    // 업적 관련 버튼 //
    public void ClickAchievementBtn()
    {// 로비 '업적'버튼
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        m_associateLobbyBtn.SetActive(false);
        m_achievementPanel.SetActive(true);
    }
    public void ClickAchievementBackBtn()
    {
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        m_associateLobbyBtn.SetActive(true);
        m_achievementPanel.SetActive(false);
    }
    // 업적 관련 버튼 //

    public void ClickExitBtn()
    {
        theSound.PlayEffSound(SoundManager.eEff_Type.Button);

        #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
            Application.OpenURL(webplayerQuitURL);
        #else
            Application.Quit();
        #endif
    }
}
