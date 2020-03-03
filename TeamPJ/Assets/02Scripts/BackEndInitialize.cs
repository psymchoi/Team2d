using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackEndInitialize : MonoBehaviour
{
    // .Net 4 버전
    void Awake()
    {
        Backend.Initialize(HandleBackendCallback);
    }

    void HandleBackendCallback()
    {
        if(Backend.IsInitialized)
        {
            Debug.Log("뒤끝 SDK 초기화 완료");

            // ex
            // 버전체크 => 업데이트

            // 구글 해시키 획득
            if (!Backend.Utils.GetGoogleHash().Equals(""))
                Debug.Log(Backend.Utils.GetGoogleHash());
            // 서버시간 획득
            Debug.Log(Backend.Utils.GetServerTime());
        }
    }
}
