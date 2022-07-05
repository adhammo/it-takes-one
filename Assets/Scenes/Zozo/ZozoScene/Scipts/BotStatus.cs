using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStatus : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Animator anim;
    public bool BotisDied = false;

    void Start()
    {
        currentHealth = maxHealth ;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if ( currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        BotisDied = true;
        anim.SetBool("Die",true);
        Destroy(gameObject,4f);
    }
}
