using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStatus : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Animator anim;
    public bool BotisDied = false;

    public AudioSource source;
    public AudioClip clip;

    public int ObjectIdentifier = 0;

    public BotsTracker tracker;

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
                if(BotisDied == false)
                {
                     Die();
                }

            }
            else if (ObjectIdentifier == 1 )   /* Towers */
            {
                if(BotisDied == false)
                {
                     Towers_Die();
                }
            }

        }
    }

    public void Die()
    {
        BotisDied = true;
        if(tracker)
            tracker.DiedBotsCounter ++ ;

        if(anim)
            anim.SetBool("Die",true);

        Destroy(gameObject,4f);
    }

    public void Towers_Die()
    {
        BotisDied = true;
        tracker.DiedBotsCounter ++ ;
        source.PlayOneShot(clip);
        Destroy(gameObject,2f);
    }
}
