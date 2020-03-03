using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    CharController theCController;
    SoundManager theSound;

    // Start is called before the first frame update
    void Start()
    {
        theCController = FindObjectOfType<CharController>();
        theSound = FindObjectOfType<SoundManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            theSound.PlayEffSound(SoundManager.eEff_Type.Char_BAttack);

            float a_dmg = collision.gameObject.GetComponent<EnemyController>().Hit(theCController.m_atk);
            
            Vector3 vector = collision.transform.position;
            vector.y += 1;
            
            // GameObject clone = Instantiate()
        }
    }
}
