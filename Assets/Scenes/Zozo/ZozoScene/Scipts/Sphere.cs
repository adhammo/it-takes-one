using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public Bots Bot;

    public int objectidenetify = 0;
    public float damage = 20f;

    public void Start()
    {

    }
    public void OnTriggerEnter ( Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
             if(Bot.isAttacking && objectidenetify== 0)   /* AI */
             {
                 /* Damage Player */
                 Bot.Player.GetComponent<Death>().TakeDamage(damage);
             }
             else if (objectidenetify == 1 )   /* Towers */
             {
                 Bot.Player.GetComponent<Death>().TakeDamage(damage);
             }

        }
    }

}
