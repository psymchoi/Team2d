using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtCharacter : MonoBehaviour
{
    EnemyController theEController;
    SoundManager theSound;

    // Start is called before the first frame update
    void Start()
    {
        theEController = FindObjectOfType<EnemyController>();
        theSound = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            // theSound.PlayEffSound(SoundManager.eEff_Type.Char_BAttack);

            float a_dmg = collision.gameObject.GetComponent<CharController>().Hit(theEController.m_atk);

            Vector3 vector = collision.transform.position;
            vector.y += 1;

            // GameObject clone = Instantiate()
        }
    }
}
