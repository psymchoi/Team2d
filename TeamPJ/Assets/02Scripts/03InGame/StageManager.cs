using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager StageInstance;
    public List<GameObject> m_tfEnemy;

    public GameObject[] m_emyCard;        // 몬스터 객체
    public GameObject[] m_emyZone;        // 몬스터 객체 설치존

    bool[] m_isEmySlotOn;

    // Start is called before the first frame update
    void Start()
    {
        StageInstance = this;

        m_isEmySlotOn = new bool[m_emyZone.Length];
        for (int n = 0; n < m_emyZone.Length; n++)
            m_isEmySlotOn[n] = true;

        EnemySpawn();
    }

    public void EnemySpawn()
    {
        int a_howMany = Random.Range(2, m_emyZone.Length - 2);
        int a_emyKind;
        int a_emyPos;
        for (int n = 0; n < a_howMany; n++)
        {
            a_emyKind = Random.Range(0, m_emyCard.Length);
            a_emyPos = Random.Range(0, m_emyZone.Length);

            if(m_isEmySlotOn[a_emyPos] == false)
            {
                n--;
                continue;
            }
            
            GameObject go = Instantiate(m_emyCard[a_emyKind]);
            go.transform.parent = this.transform;
            go.transform.position = m_emyZone[a_emyPos].transform.position;

            m_isEmySlotOn[a_emyPos] = false;
        }
    }
}
