using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator m_aniCtrl;

    public bool m_isDeath = false;

    // Start is called before the first frame update
    void Start()
    {
        m_aniCtrl = GetComponent<Animator>();    
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("AttackFromPlayer"))
        {
            m_isDeath = true;
            m_aniCtrl.SetBool("Dead", true);
            Invoke("VanishChar", 3.0f);
        }
    }
    
    public void VanishChar()
    {
        Destroy(this.gameObject);
    }
}
