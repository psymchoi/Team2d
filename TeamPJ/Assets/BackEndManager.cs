using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackEndManager : MonoBehaviour
{
    public static BackEndManager instance = null;
    public static BackEndManager MyInstance { get => instance; set => instance = value; }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitBackEnd();
    }

    void InitBackEnd()
    {
        Backend.Initialize(BRO =>
        {
            Debug.Log("뒤끝 초기화 진행" + BRO);

            // 성공
            if (BRO.IsSuccess())
            {
                Debug.Log(Backend.Utils.GetServerTime());
            }

            // 실패
            else
            {

            }
        });
    }

    public void ShowErrorUI(BackendReturnObject backendReturn)
    {
        int statusCode = int.Parse(backendReturn.GetStatusCode());

        switch(statusCode)
        {
            case 401:
                Debug.Log("ID 또는 비밀번호가 틀렸습니다.");
                break;

            case 403:
                // 콘솔창에 입력한 차단 사유가 GetErrorCode() 로 전달된다.
                Debug.Log(backendReturn.GetErrorCode());
                break;

            case 404:
                Debug.Log("game not found, game을(를) 찾을 수 없습니다.");
                break;

            case 408:
                // 타입아웃 오류(서버에서 응답이 늦거나 ,네트워크 등이 끊겨 있는 경우)
                // 요청 오류
                Debug.Log(backendReturn.GetMessage());
                break;

            case 409:
                Debug.Log("중복된 ID 입니다.");
                break;

            case 410:
                Debug.Log("bad refreshToken, 잘못된 refreshToken 입니다.");
                break;

            case 429:
                // 데이터베이스 할당량을 초과한 경우
                // 데이터베이스 할당량 업데이트 중인 경우
                Debug.Log(backendReturn.GetMessage());
                break;

            case 503:
                // 서버가 정상적으로 작동하지 않는 경우
                Debug.Log(backendReturn.GetMessage());
                break;

            case 504:
                // 타임아웃 오류(서버에서 응답이 늦거나, 네트워크 등이 끊겨 있는 경우)
                Debug.Log(backendReturn.GetMessage());
                break;
        }
    }
}
