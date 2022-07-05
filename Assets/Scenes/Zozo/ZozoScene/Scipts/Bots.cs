using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Bots : MonoBehaviour
{
    public Transform Player;
    public NavMeshAgent agent;
    public Animator anim;
    public float LookRadius = 30f;

    public BotStatus Status;
    
    void Update()
    {
            if(!Status.BotisDied)
            {
                CheckDistance();
            }
            else
            {
                agent.SetDestination(transform.position);
            }

    }

    void CheckDistance()
    {
          float distance = Vector3.Distance(Player.position, transform.position);     
          if(distance <= LookRadius && distance > agent.stoppingDistance && !Status.BotisDied)                    /*Player is Running Away*/
          {
             
          

                anim.SetBool("Run",true);  
                anim.SetBool("Attack",false);
                anim.SetBool("Attack2",false);      
                agent.SetDestination(Player.position);
                FaceTarget();

          }
          if(distance <= agent.stoppingDistance && !Status.BotisDied)                                            /*Player is in Range of BOTS */
          {
                    Attack();
                    FaceTarget();
          }
    }

    void FaceTarget()
    {
        Vector3 direction = Player.position - transform.position;
        if(direction.x !=0 && direction.z !=0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
       
    }

    void Attack()
    {
        anim.SetBool("Attack2",false);       
        anim.SetBool("Run",false);          
        anim.SetBool("Attack",true); 
        anim.SetBool("Attack2",true); 
    }


}
