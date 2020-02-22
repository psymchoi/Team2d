using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum eBGM_Type
    {
        Lobby = 0,
        InGame,
    }

    public enum eEff_Type
    {
        Button,
    }

    [SerializeField] AudioClip[] m_bgmClips;
    [SerializeField] AudioClip[] m_effClips;

    public AudioSource m_bgmPlayer;
    public float m_bgmVolume = 0;
    public float m_effectVolume = 0.7f;

    List<AudioSource> m_ltEffPlayer;

    // Start is called before the first frame update
    void Start()
    {
        m_bgmPlayer = GetComponent<AudioSource>();
        m_ltEffPlayer = new List<AudioSource>();

        m_bgmVolume = m_bgmPlayer.volume = 0.7f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 이펙트 재생이 끝났으면 제거
        foreach(AudioSource item in m_ltEffPlayer)
        {
            if(!item.isPlaying)
            {
                m_ltEffPlayer.Remove(item);
                Destroy(item.gameObject);
                break;
            }
        }
    }

    /// <summary>
    /// Bgm 사운드 재생시켜주는 함수.
    /// </summary>
    /// <param name="type">무슨 Bgm?</param>
    /// <param name="vol">볼륨은?</param>
    /// <param name="isLoop">반복할거야?</param>
    public void PlayBgmSound(eBGM_Type type, float vol = 0.7f, bool isLoop = false)
    {
        m_bgmPlayer.clip = m_bgmClips[(int)type];
        m_bgmPlayer.volume = vol;
        m_bgmPlayer.loop = isLoop;

        m_bgmPlayer.Play();
    }

    /// <summary>
    /// 이펙트 사운드 재생해주는 함수.
    /// </summary>
    /// <param name="type">무슨 이펙트?</param>
    /// <param name="vol">볼륨은?</param>
    /// <param name="isLoop">반복할거야?</param>
    public void PlayEffSound(eEff_Type type, bool isLoop = false)
    {
        GameObject go = new GameObject("EffectSoundObj");
        go.transform.SetParent(transform);

        AudioSource AS = go.AddComponent<AudioSource>();
        AS.clip = m_effClips[(int)type];
        AS.volume = m_effectVolume;
        AS.loop = isLoop;

        AS.Play();

        m_ltEffPlayer.Add(AS);
    }

    // 음향 연출효과 관련 //
    public void FadeIn_Bgm()
    {
        StopAllCoroutines();
        StartCoroutine(BgmOn());
    }

    public void FadeOut_Bgm()
    {
        StopAllCoroutines();
        StartCoroutine(BgmOff());
    }

    IEnumerator BgmOn()
    {// Bgm 서서히 음향 올라감
        for (float n = 0; n <= m_bgmVolume; n += 0.1f)
        {
            m_bgmPlayer.volume = n;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator BgmOff()
    {// Bgm 서서히 음향 내려감
        for (float n = m_bgmPlayer.volume; n >= 0; n -= 0.02f)
        {
            m_bgmPlayer.volume = n;
            yield return new WaitForSeconds(0.01f);
        }
        //m_bgmPlayer.Stop();
    }
    // 음향 연출효과 관련 //

}
