using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStat : MonoBehaviour
{
    // 캐릭터 종류
    public enum eCharacterNum
    {
        One,        // 갈색
        Two,        // 초록
        Three,      // 파랑
        Four,       // 회색
        Five,       // 빨강
        Six,        // 미남
    }
    public eCharacterNum eCharNum;
    // 캐릭터 종류

    

    // Start is called before the first frame update
    void Start()
    {
        switch(eCharNum)
        {
            case eCharacterNum.One:

                break;
            case eCharacterNum.Two:

                break;
            case eCharacterNum.Three:

                break;
            case eCharacterNum.Four:

                break;
            case eCharacterNum.Five:

                break;
            case eCharacterNum.Six:

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
