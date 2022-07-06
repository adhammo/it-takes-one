using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dragon : MonoBehaviour
{
    public Rigidbody rb;
    public Transform[] Points; 
    public int index = 0;
    bool Flag = true;

    void Update()
    {

            Vector3 Direction = Points[index].position - transform.position;
            transform.Translate(Direction * Time.deltaTime );
           // FaceTarget();
            if(Flag)
            {
                StartCoroutine(ChangeDragonPath(5f));
                Flag = false;
            }


    }

    IEnumerator ChangeDragonPath (float time)
    {
          yield return new WaitForSeconds(time); 
          index++;
          if(index>3)
          {
            index=0;
          }     
           Flag = true;
                
    }

    void FaceTarget()
    {
        Vector3 direction = Points[index].position- transform.position;
        if(direction.x !=0 && direction.z !=0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
       
    }
}
