using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager StageInstance;
    public Transform[] m_tfEnemy;

    // Start is called before the first frame update
    void Start()
    {
        StageInstance = this;
        
    }
    
}
