using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public Bots Bot;
    public void OnTriggerEnter ( Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if(Bot.isAttacking)
            {
                /* Damage Player */
            }
        }
    }

}
