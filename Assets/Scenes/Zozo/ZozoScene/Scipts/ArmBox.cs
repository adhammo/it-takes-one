using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmBox : MonoBehaviour
{
    public Animator ArmAnimator;

    public bool CanPressArm;



    void Update()
    {
        if(CanPressArm)
        {
            ArmAnimator.SetBool("ArmClick",true);
            CanPressArm=false;
        }
    }

      public void OnTriggerEnter ( Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            CanPressArm = true;
        }
    }
        public void OnTriggerExit (Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            CanPressArm = false;
            ArmAnimator.SetBool("ArmClick",false);            
        }
    }
}
