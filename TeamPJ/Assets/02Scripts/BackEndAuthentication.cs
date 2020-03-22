using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;

public class BackEndAuthentication : MonoBehaviour
{
    public InputField idInput;
    public InputField paInput;

    // 1. 동기 방식
    // 순차적으로 앞의 코드가 완료되면 다음줄 실행!

    // 동기방식 회원가입
    public void OnClickSignUp1()
    {
        BackendReturnObject BRO = Backend.BMember.CustomSignUp(idInput.text, paInput.text, "로그인 강좌로 가입된 유저");

        if(BRO.IsSuccess())
        {
            Debug.Log("[동기방식] 회원 가입 완료");
        }
        else
        {
            // BackEndManager.MyInstance.ShowErrorUI(BRO);
        }

        Debug.Log("동기 방식 끝줄 ================================");
    }

    // 동기방식 로그인
    public void OnClickLogin1()
    {
        BackendReturnObject BRO = Backend.BMember.CustomLogin(idInput.text, paInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("[동기방식] 로그인 완료");
        }
        else
        {
            // BackEndManager.MyInstance.ShowErrorUI(BRO);
        }

        Debug.Log("동기 방식 끝줄 ================================");
    }

    // 2. 비동기 방식
    // 순차적으로 실행되나 앞의 코드의 완료여부 상관없이 다음줄 실행
}
