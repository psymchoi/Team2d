using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using System.Text.RegularExpressions;

public class BackEndNickname : MonoBehaviour
{
    [SerializeField] InputField nickInput;

    // 한글, 영어, 숫자만 입력 가능하게
    bool CheckNickname()
    {
        return Regex.IsMatch(nickInput.text, "^[0-9a-zA-Z가-힣]*$");
    }

    /// <summary>
    /// 닉네임 생성
    /// </summary>
    public void OnClickCreateName()
    {
        // 한글, 영어, 숫자로만 닉네임을 만들었는지 체크
        if(CheckNickname() == false)
        {
            Debug.Log("닉네임은 한글, 영어, 숫자로만 만들 수 있습니다.");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.CreateNickname(nickInput.text);

        if(BRO.IsSuccess())
        {
            Debug.Log("닉네임 생성 완료");
        }
        else
        {
            switch(BRO.GetStatusCode())
            {
                case "409":
                    Debug.Log("이미 중복된 닉네임이 있는 경우");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long")) 
                        Debug.Log("20자 이상의 닉네임인 경우");
                    else if (BRO.GetMessage().Contains("blank"))
                        Debug.Log("닉네임에 앞/뒤 공백이 있는 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생 : " + BRO.GetErrorCode());
                    break;
            }
        }
    }

    /// <summary>
    /// 닉네임 변경
    /// </summary>
    public void OnClickUpdateName()
    {
        // 한글, 영어, 숫자로만 닉네임을 만들었는지 체크
        if(CheckNickname() == false)
        {
            Debug.Log("닉네임은 한글, 영어, 숫자로만 만들 수 있습니다.");
            return;
        }

        BackendReturnObject BRO = Backend.BMember.UpdateNickname(nickInput.text);

        if (BRO.IsSuccess())
        {
            Debug.Log("닉네임 변경 완료");
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "409":
                    Debug.Log("이미 중복된 닉네임이 있는 경우");
                    break;

                case "400":
                    if (BRO.GetMessage().Contains("too long"))
                        Debug.Log("20자 이상의 닉네임인 경우");
                    else if (BRO.GetMessage().Contains("blank"))
                        Debug.Log("닉네임에 앞/뒤 공백이 있는 경우");
                    break;

                default:
                    Debug.Log("서버 공통 에러 발생 : " + BRO.GetErrorCode());
                    break;
            }
        }
    }
}
