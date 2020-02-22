using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStageManager : MonoBehaviour
{
    BaseSceneManager theBase;
    SoundManager theSound;
    FadeManager theFade;

    //---- 옵션 관련 ----//
    public GameObject m_settingPanel;           // 세팅 켜지는 패널
    public GameObject m_optionPanel;            // 음향 및 옵션조절 패널
    //---- 옵션 관련 ----//

    //---- 스테이지 선택 관련 ----//
    public GameObject m_card;
    //---- 스테이지 선택 관련 ----//

    public Slider m_bgmVolume;
    public Slider m_effectVolume;
    
    float m_timeCheck = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        theBase = FindObjectOfType<BaseSceneManager>();
        theSound = FindObjectOfType<SoundManager>();
        theFade = FindObjectOfType<FadeManager>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    
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
