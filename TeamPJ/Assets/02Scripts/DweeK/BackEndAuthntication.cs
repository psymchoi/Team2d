using BackEnd;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackEndAuthntication : MonoBehaviour
{
    public InputField idInput;
    public InputField paInput;
    

    #region 동기방식

    // 1. 동기 방식
    // 순차적으로 앞의 코드가 완료되면 다음줄 실행

    // 동기방식 회원가입
    public void OnClickSignUp1()
    {
        BackendReturnObject BRO = Backend.BMember.CustomSignUp(idInput.text, paInput.text, "로그인 강좌로 가입된 유저");

        if(BRO.IsSuccess() == true)
        {
            Debug.Log("[동기방식] 회원가입 완료");
        }
        else
        {

        }
        
        Debug.Log("동기 방식 끝줄 ==============");
    }

    // 동기방식 로그인
    public void OnClickLogin1()
    {
        BackendReturnObject BRO = Backend.BMember.CustomLogin(idInput.text, paInput.text);

        if (BRO.IsSuccess() == true)
        {
            Debug.Log("[동기방식] 로그인 완료");
        }
        else
        {
            Debug.Log("[비동기방식] 로그인 실패");
        }

        Debug.Log("동기 방식 끝줄 ==============");
    }

    // 자동 로그인 동기방식
    public void AutoLogin1()
    {
        BackendReturnObject backendReturnObject = Backend.BMember.LoginWithTheBackendToken();

        if (backendReturnObject.IsSuccess() == true)
        {
            Debug.Log("[동기방식] 자동로그인 완료");
        }
        else
        {
            Debug.Log("[동기방식] 자동로그인 실패");
        }

        Debug.Log("동기 방식 ==============");
    }
    #endregion

    #region 비동기 방식
    // 2. 비동기 방식
    // 순차적으로 실행되나 앞의 코드의 완료여부 상관없이 다음줄 실행
    BackendReturnObject bro = new BackendReturnObject();
    bool isSuccess = false;

    void Update()
    {
        if(isSuccess == true)
        {
            // SaveToken (BackendReturnObject BRO) -> void
            // 비동기 메소드는 update()문에서 SaveToken을 꼭 적용해야 한다.
            Backend.BMember.SaveToken(bro);
            isSuccess = false;
            bro.Clear();
        }
    }

    // 비동기 회원가입
    public void OnClickSignUp2()
    {
        Backend.BMember.CustomSignUp(idInput.text, paInput.text, "비동기 회원가입 방식", (BRO) =>
        {
            bro = BRO;
            isSuccess = BRO.IsSuccess();            // 가입 잘 되었으면 true, 아니면 false

            if (BRO.IsSuccess() == true)
            {
                Debug.Log("[비동기방식] 회원가입 완료");
            }
            else
            {
                Debug.Log("[비동기방식] 회원가입 실패");
            }
        });

        Debug.Log("비동기 방식 끝줄 ==============");
    }

    // 비동기 로그인
    public void OnClickLogin2()
    {
        Backend.BMember.CustomLogin(idInput.text, paInput.text, "비동기 로그인 방식", (BRO) =>
        {
            bro = BRO;
            isSuccess = BRO.IsSuccess();            // 가입 잘 되었으면 true, 아니면 false

            if (BRO.IsSuccess() == true)
            {
                Debug.Log("[비동기방식] 로그인 완료");
                LobbyManager.InstanceLobby.eState = LobbyManager.eGameState.LoadNotice;
            }
            else
            {
                Debug.Log("[비동기방식] 로그인 실패");
            }
        });

        Debug.Log("비동기 방식 끝줄 ==============");
    }
    

    // 자동 로그인 비동기방식
    public void AutoLogin2()
    {
        Backend.BMember.LoginWithTheBackendToken((callback) =>
        {
            bro = callback;
            isSuccess = callback.IsSuccess();

            if (isSuccess == true)
            {
                Debug.Log("[동기방식] 자동로그인 완료");
            }
            else
            {
                Debug.Log("[동기방식] 자동로그인 실패");
            }
        });

        Debug.Log("동기 방식 ==============");
    }
    #endregion

    
    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false)
            .RequestEmail()                 // 이메일 요청
            .RequestIdToken()               // 토큰 요청
            .Build();

        //커스텀된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = false;

        //GPGS 시작.
        PlayGamesPlatform.Activate();
        GoogleAuth();
    }

    // 구글에 로그인하기
    private void GoogleAuth()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            Social.localUser.Authenticate(success =>
            {
                if (success == false)
                {
                    Debug.Log("구글 로그인 실패");
                    return;
                }

                // 로그인이 성공되었습니다.
                Debug.Log("GetIdToken - " + PlayGamesPlatform.Instance.GetIdToken());
                Debug.Log("Email - " + ((PlayGamesLocalUser)Social.localUser).Email);
                Debug.Log("GoogleId - " + Social.localUser.id);
                Debug.Log("UserName - " + Social.localUser.userName);
                Debug.Log("UserName - " + PlayGamesPlatform.Instance.GetUserDisplayName());

                OnClickGPGSLogin();
            });
        }
    }

    // 구글 토큰 받아오기
    private string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두번째 방법
            // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어있지 않습니다. 잠시 후 다시 시도하세요.");
            GoogleAuth();
            return null;
        }
    }

    // 구글토큰으로 뒤끝서버 로그인하기 - 동기 방식
    public void OnClickGPGSLogin()
    {
        BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs로 만든계정");
        if (BRO.IsSuccess())
        {
            Debug.Log("구글 토큰으로 뒤끝서버 로그인 성공 - 동기 방식-");
            LobbyManager.InstanceLobby.eState = LobbyManager.eGameState.LoadNotice;
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "200":
                    Debug.Log("이미 회원가입된 회원");
                    break;

                case "403":
                    Debug.Log("차단된 사용자 입니다. 차단 사유 : " + BRO.GetErrorCode());
                    break;
            }
        }
    }

    // 이미 가입된 상태인지 확인
    public void OnClickCheckUserAuthenticate()
    {
        BackendReturnObject BRO = Backend.BMember.CheckUserInBackend(GetTokens(), FederationType.Google);
        if (BRO.GetStatusCode() == "200")
        {
            Debug.Log("가입 중인 계정입니다.");

            // 해당 계정 정보
            Debug.Log(BRO.GetReturnValue());
        }

        else
        {
            Debug.Log("가입된 계정이 아닙니다.");
        }
    }

    public void OnClickChangeCustomToFederation()
    {
        BackendReturnObject BRO = Backend.BMember.ChangeCustomToFederation(GetTokens(), FederationType.Google);

        if (BRO.IsSuccess())
        {
            Debug.Log("페더레이션 계정으로 변경 완료");

        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "400":
                    if (BRO.GetErrorCode() == "BadParameterException")
                    {
                        Debug.Log("이미 ChangeCustomToFederation 완료 되었는데 다시 시도한 경우");
                    }

                    else if (BRO.GetErrorCode() == "UndefinedParameterException")
                    {
                        Debug.Log("customLogin 하지 않은 상황에서 시도한 경우");
                    }
                    break;

                case "409":
                    // 이미 가입되어 있는 경우
                    Debug.Log("Duplicated federationId, 중복된 federationId 입니다");
                    break;
            }
        }

    }
    
}
