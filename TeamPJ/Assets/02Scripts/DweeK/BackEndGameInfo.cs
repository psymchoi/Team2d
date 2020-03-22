using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class BackEndGameInfo : MonoBehaviour
{
    #region 게임정보 저장
    /// <summary>
    /// 버튼 클릭 시 데이터 저장 (TEST)
    /// </summary>
    public void OnClickInsertData()
    {
        int charLevel = Random.Range(0, 99);
        int charExp = Random.Range(0, 9999);
        int charScore = Random.Range(0, 99999);

        // Param은 뒤끝 서버와 통신을 할 때 넘겨주는 파라미터 클래스 입니다.
        Param param = new Param();
        param.Add("lv", charLevel);
        param.Add("exp", charExp);
        param.Add("score", charScore);

        // 값을 Dictionary로 사용하기
        Dictionary<string, int> equipment = new Dictionary<string, int>()
        {
            { "weapon", 123 },
            { "armor", 111 },
            { "helmet", 1345 }
        };

        param.Add("equipItem", equipment);

        BackendReturnObject BRO = Backend.GameInfo.Insert("custom", param);     // "custom" 테이블에 param데이터를 추가!

        if(BRO.IsSuccess())
        {
            Debug.Log("indate : " + BRO.GetInDate());
        }
        else
        {
            switch(BRO.GetStatusCode())
            {
                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;
                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;
                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;
                default:
                    Debug.Log("서버 공통 에러 발생 : " + BRO.GetMessage());
                    break;
            }
        }
    }
   
    /// <summary>
    /// 맨 처음 설치하고 들어올 시 게임 정보 생성 및 초기화
    /// </summary>
    public void UserInfoData()
    {
        Debug.Log("UserInfoData Active");

        int charMoney = 3000;
        int charDiamond = 0;
        int charRank = 1;
        int charPirce = 0;
        int charScore = 0;

        // Param은 뒤끝 서버와 통신을 할 때 넘겨주는 파라미터 클래스 입니다.
        Param param = new Param();
        param.Add("Mny", charMoney);            // 돈
        param.Add("Dia", charDiamond);          // 다이아
        param.Add("Rnk", charRank);               // 랭킹
        param.Add("Prc", charPirce);                // 등급(보상을 위한)
        param.Add("Scr", charScore);               // 플레이점수(층)

        // 캐릭터 보유여부
        Dictionary<string, bool> myCharacter = new Dictionary<string, bool>()
        {
            { "Char1", true },
            { "Char2", true },
            { "Char3", false},
            { "Char4", false},
            { "Char5", false},
        };

        param.Add("myChar", myCharacter);

        BackendReturnObject BRO = Backend.GameInfo.Insert("characterInfo", param);     // "characterInfo" 테이블에 param데이터를 추가!

        if (BRO.IsSuccess())
        {
            Debug.Log("indate : " + BRO.GetInDate());
        }
        else
        {
            switch (BRO.GetStatusCode())
            {
                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;
                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;
                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;
                default:
                    Debug.Log("서버 공통 에러 발생 : " + BRO.GetMessage());
                    break;
            }
        }
    }
    #endregion

    
    #region 게임정보 읽기
    public void OnClickGetTableList()
    {
        BackendReturnObject BRO = Backend.GameInfo.GetTableList();              // 테이블 리스트를 반환

        if(BRO.IsSuccess())
        {
            // publicTables 값들을 읽어오는
            JsonData publics = BRO.GetReturnValuetoJSON()["publicTables"];

            Debug.Log("public Tables--------------------");
            foreach(JsonData row in publics)
            {
                Debug.Log(row.ToString());
            }
            // publicTables 값들을 읽어오는

            // privateTables 값들을 읽어오는
            Debug.Log("private Tables--------------------");
            JsonData privates = BRO.GetReturnValuetoJSON()["privateTables"];
            foreach(JsonData row in privates)
            {
                Debug.Log(row.ToString());
            }
            // privateTables 값들을 읽어오는
        }
        else
        {
            Debug.Log("서버 공통 에러 발생 : " + BRO.GetMessage());
        }
    }

    /// <summary>
    /// Public 테이블에 저장된 데이터 불러오기
    /// </summary>
    public void OnClickPublicContents()
    {
        BackendReturnObject BRO = Backend.GameInfo.GetPublicContents("custom", 1);      // (어떤 테이블? , 불러올 정보 개수)
        // 두번째 인자가 0일 때 default 값이 되며 limit는 10개이다. 10개의 데이터를 가져온다
        // BackendReturnObject BRO = Backend.GameInfo.GetPublicContents("custom");

        if(BRO.IsSuccess())
        {
            GetGameInfo(BRO.GetReturnValuetoJSON());
        }
        else
        {
            // 에러 체크
            CheckError(BRO);
        }
    }
    
    string firstKey = string.Empty;
    public void OnClickPublicContentsNext()
    {
        BackendReturnObject BRO;

        Debug.Log(firstKey);

        if(firstKey == string.Empty)
        {
            BRO = Backend.GameInfo.GetPublicContents("custom", 1);
        }
        else
        {
            // 이전에 불러온 데이터(firstKey)의 다음 순번 데이터를 불러온다.
            BRO = Backend.GameInfo.GetPublicContents("custom", firstKey, 1);
        }

        if(BRO.IsSuccess())
        {
            firstKey = BRO.FirstKeystring();
            GetGameInfo(BRO.GetReturnValuetoJSON());
        }
        else
        {
            CheckError(BRO);
        }
    }

    /// <summary>
    /// 비공개 테이블에서 본인 정보 가져오기 (자기정보만)
    /// </summary>
    public void OnClickGetPrivateContents()
    {
        BackendReturnObject BRO = Backend.GameInfo.GetPrivateContents("character");

        if(BRO.IsSuccess())
        {
            GetGameInfo(BRO.GetReturnValuetoJSON());
        }
        else
        {
            CheckError(BRO);
        }
    }

    /// <summary>
    /// 공개 테이블에서 특정 유저의 정보 불러오기 (불량 문제로 강좌에서 생략..)
    /// </summary>
    public void OnClickGetPublicContentsByGamerIndate()
    {
        // 해당 유저의 닉네임으로 Indate 값을 불러올 수 있다.
        // https://develpoer.thebackend.io/unity3d/guide/social/getuser/ 페이지에서 특정 유저의 Indate 값 불러오기 참고.

        BackendReturnObject BRO = Backend.GameInfo.GetPublicContentsByGamerIndate("custom", "Indate값");

        if(BRO.IsSuccess())
        {
            GetGameInfo(BRO.GetReturnValuetoJSON());
        }
        else
        {
            CheckError(BRO);
        }
    }

    
    void GetGameInfo(JsonData returnData)
    {
        // ReturnValue가 존재하고, 데이터가 있는지 확인
        if(returnData != null)
        {
            Debug.Log("데이터가 존재합니다.");

            // rows로 전달받은 경우
            if(returnData.Keys.Contains("rows"))                // 여러개의 데이터
            {
                JsonData rows = returnData["rows"];
                for(int i = 0; i < rows.Count; i++)
                {
                    GetData(rows[i]);
                }
            }
            else if(returnData.Keys.Contains("row"))            // 한개의 데이터
            {
                JsonData row = returnData["row"];
                GetData(row[0]);
            }
        }
        else
        {
            Debug.Log("데이터가 없습니다.");
        }
    }

    /// <summary>
    /// json Parsing
    /// </summary>
    /// <param name="data"></param>
    void GetData(JsonData data)
    {
        var score = data["score"][0];
        var exp = data["exp"][0];
        var lv = data["lv"][0];
        var equipItem = data["equipItem"][0];

        Debug.Log("score : " + score);
        Debug.Log("exp : " + exp);

        // 아래의 키가 존재하는지 확인하고 데이터를 파싱하는 방법입니다. (안전장치 확인용)
        if(data.Keys.Contains("lv"))
        {
            Debug.Log("lv : " + data["lv"][0]);
        }
        else
        {
            Debug.Log("존재하지 않는 키 입니다.");
        }

        // 패당 값이 배열로 저장되어 있을 경우는 아래와 같이 키가 존재하는지 확인합니다. (안전장치 확인용)
        if (data.Keys.Contains("equipItem"))
        {
            JsonData equipData = data["equipItem"][0];

            if(equipData.Keys.Contains("weapon"))
            {
                Debug.Log("weapon : " + equipData["weapon"][0]);
            }
        }
    }

    /// <summary>
    /// 게임 정보 읽기 관련 에러처리를
    /// 하나의 메소드로 묶었습니다. (내용을 이해하시는데 어려움이 있을거 같아)
    /// </summary>
    /// <param name="BRO"></param>
    void CheckError(BackendReturnObject BRO)
    {
        switch(BRO.GetStatusCode())
        {
            case "200":
                Debug.Log("해당 유저의 데이터가 테이블에 없습니다.");
                break;

            case "404":
                if (BRO.GetMessage().Contains("gamer not found"))
                {
                    Debug.Log("gamerIndate가 존재하지 gamer의 indate인 경우");
                }
                else if(BRO.GetMessage().Contains("table not found"))
                {
                    Debug.Log("존재하지 않는 테이블");
                }
                break;

            case "400":
                if(BRO.GetMessage().Contains("bad limit"))
                {
                    Debug.Log("limit 값이 100이상인 경우");
                }
                break;
        }
    }
    #endregion


    #region 게임정보 수정
    string gamerIndt = string.Empty;
    public void OnClickGameInfoUpdate()
    {
        Param param = new Param();
        param.Add("score", 9999);               // score 9999로 수정
        

        BackendReturnObject BRO = Backend.GameInfo.Update("custom", "2020-03-19T09:39:37.966Z", param);     // (테이블이름, 변경 데이터 date 값)
        if (gamerIndt == string.Empty)
            gamerIndt = BRO.GetInDate();
        if(BRO.IsSuccess())
        {
            Debug.Log("수정 완료");
        }
        else
        {
            switch(BRO.GetStatusCode())
            {
                case "405":
                    Debug.Log("param에 partition, gamer_id, inDate, updateAt 네가지 필드가 있는 경우");
                    break;
                case "403":
                    Debug.Log("퍼블릭테이블의 타인정보를 수정하고자 하였을 경우");
                    break;
                case "404":
                    Debug.Log("존재하지 않는 tableName인 경우");
                    break;
                case "412":
                    Debug.Log("비활성화 된 tableName인 경우");
                    break;
                case "413":
                    Debug.Log("하나의 row( column들의 집합 )이 400KB를 넘는 경우");
                    break;
                default:
                    Debug.Log("서버 공통 에러 발생 : " + BRO.GetMessage());
                    break;
            }
        }
    }

    #endregion
}
