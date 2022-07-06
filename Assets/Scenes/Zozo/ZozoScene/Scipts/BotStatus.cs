using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStatus : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Animator anim;
    public bool BotisDied = false;

    public int ObjectIdentifier = 0;

    void Start()
    {
        currentHealth = maxHealth ;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if ( currentHealth <= 0)
        {
            if(ObjectIdentifier == 0)   /* Humanoid*/
            {
                            Die();
            }
            else if (ObjectIdentifier == 1 )   /* Towers */
            {
                Towers_Die();
            }

        }
    }

    public void Die()
    {
        BotisDied = true;
        anim.SetBool("Die",true);
        Destroy(gameObject,4f);
    }

    public void Towers_Die()
    {
        BotisDied = true;
        Destroy(gameObject,2f);
    }
}
