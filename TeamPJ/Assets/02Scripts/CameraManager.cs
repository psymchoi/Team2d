using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // public float movSpeed = 50;             // 카메라 따라가는 속도.
    protected Plane plane;

    GameObject target;          // 카메라가 초기 위치
    GameObject bound;           // 카메라 영역 박스콜라이더

    // 박스컬라이더 영역의 최소.최대 xyz값을 지님.
    Vector2 minBound;
    Vector2 maxBound;
    // 박스컬라이더 영역의 최소.최대 xyz값을 지님.

    // 카메라의 반너비, 반높이 값을 지닐 변수.
    float halfWidth;
    float halfHeight;

    float clampedX;
    float clampedY;
    // 카메라의 반너비, 반높이 값을 지닐 변수.
    
    Vector2 touchStart;
    float groundZ = 0;

    bool m_MouseDownOn = false;
    
    private void Awake()
    {
        halfHeight = Camera.main.orthographicSize;                  // 카메라 size
        halfWidth = halfHeight * Screen.width / Screen.height;    // 공식.. (검색)
                                                                  // Screen.width / Screen.height = 해상도

    }

    //Vector2 direction = Vector2.zero;
   
    
    void Update()
    {
        #region 다른 방법 (문제점 있음)
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // 눌렀을 때 초기 마우스위치 반환.
        //    touchStart = GetWorldPosition(groundZ);
        //    // 눌렀을 때 초기 마우스위치 반환.
        //    direction = Vector3.zero;
        //}
        //if (Input.GetMouseButton(0))
        //{
        //    // 누르는 동안 '초기 눌렀던 곳 - 움직이고 있는 손' 위치 변환.
        //    direction = touchStart - GetWorldPosition(groundZ);
        //    Camera.main.transform.position += new Vector3(direction.x, direction.y, -9);
        //    // 누르는 동안 '초기 눌렀던 곳 - 움직이고 있는 손' 위치 변환.

        //    // 카메라가 Background를 넘지 않게 하기 위한..
        //    clampedX = Mathf.Clamp(Camera.main.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        //    clampedY = Mathf.Clamp(Camera.main.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        //    Camera.main.transform.position = new Vector3(clampedX, clampedY, -9);
        //    // 카메라가 Background를 넘지 않게 하기 위한..
        //}
        //if (!Input.GetMouseButton(0))
        //{
        //    if(direction != Vector2.zero)
        //        transform.Translate(direction.x * 0.93f, direction.y * 0.93f, -9);
        //}
        #endregion
        if(InGameManager.InGameInstance.m_curGameState == InGameManager.eGameState.Play)
        {
            #region 손가락으로 화면 터치시 이동
            //if (Input.GetMouseButtonDown(0)) //우측 마우스 버튼 On
            //{
            //    if (m_MouseDownOn == false)
            //    {
            //        StartCoroutine(MouseRoutine());
            //    }
            //}
            //else if (Input.GetMouseButtonUp(0)) //우측 마우스 버튼 Up
            //{
            //    m_MouseDownOn = false;
            //}

            //if (!Input.GetMouseButton(0)) //우측 마우스를 띄고 있는 동안에...
            //{
            //    m_MouseDownOn = false;
            //    m_OrbitSpeed = m_OrbitSpeed * 0.93f; //m_CacSpeed; //94%씩 수렴해 가고 있다. 숫자가 적을수록 얘 0.91f; 둔감해 진다.  클수록 0.99f 민감해 진다.

            //}

            //transform.Translate(m_OrbitSpeed.x * 50, 0, 0);

            // 카메라가 Background를 넘지 않게 하기 위한..
            //clampedX = Mathf.Clamp(Camera.main.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            //clampedY = Mathf.Clamp(Camera.main.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            //Camera.main.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
            // 카메라가 Background를 넘지 않게 하기 위한..
            #endregion
        }
    }

    private int minDragDistance = 20; //15

    private Vector2 m_OrbitSpeed;

    Vector3 a_CamCenter = Vector3.zero;
    Ray a_MousePos;

    float a_StartPosXX = 0.0f;
    float a_StartPosYY = 0.0f;

    float a_CurPosXX = 0.0f;
    float a_CurPosYY = 0.0f;

    float a_LastPosXX = 0.0f;
    float a_LastPosYY = 0.0f;

    //----- 카메라 이동
    private float distance = 10.0f;
    //----- 카메라 이동
    
    bool dragStarted = false;
    Vector2 startPos = Vector2.zero;
    Vector2 lastPos = Vector2.zero;
    Vector2 curPos = Vector2.zero;
    int a_MCount = 0;

    IEnumerator MouseRoutine()
    {
        m_MouseDownOn = true;

        m_OrbitSpeed = Vector2.zero;   //초기화 

        dragStarted = false;

        startPos = Input.mousePosition;

        //------------- startPos 에 해당됨
        a_CamCenter = Camera.main.GetComponent<Camera>().transform.position; //or transform.position
        a_MousePos = Camera.main.ScreenPointToRay(Input.mousePosition);

        a_StartPosXX = a_MousePos.origin.x - a_CamCenter.x; //startPos 에 해당됨
        //a_StartPosYY = a_MousePos.origin.y - a_CamCenter.y;
        //------------- startPos 에 해당됨

        lastPos = startPos;

        a_LastPosXX = a_StartPosXX;
        //a_LastPosYY = a_StartPosYY;

        //float timeStart = Mathf.Infinity;
        a_MCount = 0;

        while (m_MouseDownOn == true)
        {
            curPos = Input.mousePosition;

            //------------- curPos 에 해당됨
            a_CamCenter = Camera.main.GetComponent<Camera>().transform.position; //or transform.position
            a_MousePos = Camera.main.ScreenPointToRay(Input.mousePosition);

            a_CurPosXX = a_MousePos.origin.x - a_CamCenter.x; //curPos 에 해당됨
            //a_CurPosYY = a_MousePos.origin.y - a_CamCenter.y;
            //------------- curPos 에 해당됨

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    if (!dragStarted)
                    {
                        if (Vector2.Distance(curPos, startPos) > minDragDistance) //curPos != lastPos)  //Vector3.Distance(curPos, startPos) > 15.0f)  //마우스 드레그 시작을 의미함
                        {
                            dragStarted = true;

                            m_OrbitSpeed.x = a_StartPosXX - a_CurPosXX;
                            m_OrbitSpeed.y = a_StartPosYY - a_CurPosYY;
                        }
                    }
                    else
                    {
                        if (curPos != lastPos)
                        {
                            m_OrbitSpeed.x = a_LastPosXX - a_CurPosXX;
                            m_OrbitSpeed.y = a_LastPosYY - a_CurPosYY;

                            a_MCount = 0;
                        }
                    }

                    lastPos = curPos;
                    a_LastPosXX = a_CurPosXX;
                    a_LastPosYY = a_CurPosYY;
                }//if (touch.phase == TouchPhase.Moved)
                else if (dragStarted) //if (fireOnDraggingWhenNotMoving) //드레그중 마우스를 멈추면 지도 스크롤도 멈춰야 한다.  ---> 그러나 이것 때문에 폰에서는 마우스를 놓고 난 뒤의 부드러운 감속이 안된다.
                {
                    if (3 < a_MCount) //3번 정도까지 허용하고 멈추게 한다.
                    {
                        a_MCount = 3;
                    }
                    a_MCount++;
                }
                if (2 < a_MCount) //즉시 멈추게 처리
                {
                    m_OrbitSpeed = Vector2.zero;   //5번 동안 동안 움직이지 않으면 멈춰 버린다.  //다른 걸로 막자!!!
                }
#else  //Unity Edit 는 이쪽에서 실행 된다.
            if (!dragStarted)
            {
                if (curPos != lastPos)  //Vector3.Distance(curPos, startPos) > 15.0f)  //마우스 드레그 시작을 의미함
                {
                    dragStarted = true;

                    m_OrbitSpeed.x = a_StartPosXX - a_CurPosXX;
                   // m_OrbitSpeed.y = a_StartPosYY - a_CurPosYY;
                }
            }
            else
            {
                if (curPos != lastPos)
                {
                    m_OrbitSpeed.x = a_LastPosXX - a_CurPosXX;
                    //m_OrbitSpeed.y = a_LastPosYY - a_CurPosYY;
                }
                else //if (fireOnDraggingWhenNotMoving) //드레그중 마우스를 멈추면 지도 스크롤도 멈춰야 한다.  ---> 그러나 이것 때문에 폰에서는 마우스를 놓고 난 뒤의 부드러운 감속이 안된다.
                {

                    //일정시간 유지 된다.면... 멈춰주는거 고려 한다. 
                    m_OrbitSpeed = Vector2.zero;   //실제 Delta값 계산
                }
            }

            lastPos = curPos;
            a_LastPosXX = a_CurPosXX;
            //a_LastPosYY = a_CurPosYY;
#endif

            yield return null;
        }

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)
#else  //Unity Edit 는 이쪽에서 실행 된다.
        if (dragStarted) //Mouse Up의미함
        {
            //bool isFlick = false;
            //if (Time.realtimeSinceStartup - timeStart < 0.5f) isFlick = true;

            //------------- curPos 에 해당됨
            a_CamCenter = Camera.main.GetComponent<Camera>().transform.position; //or transform.position
            a_MousePos = Camera.main.ScreenPointToRay(Input.mousePosition);

            a_CurPosXX = a_MousePos.origin.x - a_CamCenter.x; //curPos 에 해당됨
            //a_CurPosYY = a_MousePos.origin.y - a_CamCenter.y;
            //------------- curPos 에 해당됨

            m_OrbitSpeed.x = a_LastPosXX - a_CurPosXX;    //---> 그러나 이것 때문에 폰에서는 마우스를 놓고 난 뒤의 부드러운 감속이 안된다.
           // m_OrbitSpeed.y = a_LastPosYY - a_CurPosYY;
        }
#endif

    }

    #region 다른 방법 (문제점 있음)
    //Vector2 GetWorldPosition(float z)
    //{
    //    Ray mousePos = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));

    //    ground.Raycast(mousePos, out distance);

    //    return mousePos.GetPoint(distance);
    //}
    #endregion

    public void CameraBound()
    {
        bound = GameObject.FindGameObjectWithTag("CameraBound");
        minBound = bound.GetComponent<BoxCollider2D>().bounds.min;
        maxBound = bound.GetComponent<BoxCollider2D>().bounds.max;

        //target = GameObject.FindGameObjectWithTag("CameraStartPoint");
        //this.transform.position = target.transform.position;

        //this.transform.position.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);
    }
}
