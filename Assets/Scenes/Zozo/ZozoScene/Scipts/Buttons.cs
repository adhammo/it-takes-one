using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public Animator ButtonAnimator, WallAnimator;

    public bool CanPressButton = false;
    private bool CanUseButton = true;
    


    
    void Update()
    {   
        if(CanPressButton)
        {  
                ButtonAnimator.SetBool("ButtonClick",true);
                WallAnimator.SetBool("Stand",true);
        }
    }

    public void OnTriggerEnter ( Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if ( CanUseButton)
            {
                CanPressButton = true;
                CanUseButton = false;
            }

        }

    }
    public void OnTriggerExit (Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            CanPressButton = false;
        }
    }
}
