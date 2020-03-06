using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BackEndNotice : MonoBehaviour
{
    // 공지UI
    [SerializeField]
    Transform noticePop;

    [SerializeField] GameObject[] m_NoticeBtn;

    /// <summary>
    /// 전체 공지를 받아오고
    /// 그 중 1개의 공지를 표시하는 메소드이다.
    /// 처음 게임 키면 나오는 공지사항
    /// </summary>
    public void LoadNoticeList(int noticeNum)
    {
        BackendReturnObject BRO = Backend.Notice.NoticeList();

        if(BRO.IsSuccess())
        {
            // 전체 공지 리스트
            Debug.Log(BRO.GetReturnValue());

            // 전체 공지 중에 1번째 공지를 저장합니다.
            Debug.Log("noticeNum : " + noticeNum);
            JsonData noticeData = BRO.GetReturnValuetoJSON()["rows"][noticeNum];
            // Debug.Log(BRO.GetReturnValuetoJSON()["rows"].Count);


            // 제목, 내용 참조하기.
            noticePop.Find("title").GetComponentInChildren<Text>().text = noticeData["title"][0].ToString();
            noticePop.Find("content").GetComponentInChildren<Text>().text = noticeData["content"][0].ToString();

            // 이미지 참조하기
            if(noticeData["imageKey"][0] != null)
            {
                StartCoroutine(WWWImageDown("http://upload-console.thebackend.io" + noticeData["imageKey"][0]));
            }
            else
            {
                noticePop.gameObject.SetActive(true);
                Debug.Log("공지 사항을 받아오지 못했습니다.");
            }

            // 공지 게시 일자
            noticePop.Find("postingDateTxt").GetComponentInChildren<Text>().text = noticeData["postingDate"][0].ToString();

            // 버튼 링크 주소
            noticePop.Find("LinkBtn").GetComponent<Button>().onClick.AddListener(() => LinkWindow(noticeData["linkUrl"][0].ToString()));

            // 버튼 이름.
            noticePop.Find("LinkBtn").GetComponentInChildren<Text>().text = noticeData["linkButtonName"][0].ToString();

            for (int n = 0; n < BRO.GetReturnValuetoJSON()["rows"].Count; n++)
            {
                m_NoticeBtn[n].SetActive(true);
            }
            // Debug.Log("공지사항 로딩 완료");
            LobbyManager.InstanceLobby.eState = LobbyManager.eGameState.finish;
        }
        else
        {
            Debug.Log("서버 공통 에러 발생 : " + BRO.GetErrorCode());
        }
    }

   

    /// <summary>
    /// 버튼 클릭 시 링크 주소로 이동
    /// </summary>
    /// <param name="url"></param>
    void LinkWindow(string url)
    {
        Application.OpenURL(url);
    }

    /// <summary>
    /// 이미지 받아오기
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator WWWImageDown(string url)
    {
        UnityWebRequest wr = new UnityWebRequest(url);
        DownloadHandlerTexture texDl = new DownloadHandlerTexture(true);

        wr.downloadHandler = texDl;
        yield return wr.SendWebRequest();

        if(!(wr.isNetworkError || wr.isHttpError))
        {
            if(texDl.texture != null)
            {
                Texture2D t = texDl.texture;
                Sprite s = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
                noticePop.Find("Image").GetComponent<Image>().sprite = s;
            }

            noticePop.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError(wr.error);
            Debug.Log("공지 사항을 받아오지 못했습니다.");
        }
    }

    /// <summary>
    /// 전체 공지 중 개별 공지 받아오기
    /// </summary>
    public void OnClickNoticeOne()
    {
        // 개별 공지를 받아오는 것인데
        // 현재는 전체 공지를 받아온 뒤에만 사용할 수 있어서
        // 쓰이지 않습니다.
        // 뒤끝서버 쪽에서 상황을 인지하고 개선 중이라고 하네요

        BackendReturnObject BRO = Backend.Notice.NoticeOne("");

        if(BRO.IsSuccess())
        {
            JsonData noticeData = BRO.GetReturnValuetoJSON()["row"];
            JsonData nicknameJson = noticeData["title"];
            Debug.Log(nicknameJson[0].ToString());
        }
        else
        {
            Debug.Log("서버 공통 에러 발생 : " + BRO.GetErrorCode());
        }
    }

    
    public void BackBtn()
    {
        noticePop.gameObject.SetActive(false);
    }
}
