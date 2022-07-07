using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public Animator ButtonAnimator, WallAnimator;

    public bool CanPressButton = false;
    private bool CanUseButton = true;
    
    public AudioSource ButtonSoundSource;
    public AudioClip clip;

    public int objectid = 0;

    public GameObject Mark;

    void Start()
    {
        Mark.SetActive(false);
    }
    
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
        if(collider.gameObject.tag == "Player" && objectid == 0)
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
        if(collider.gameObject.tag == "Player" && objectid == 0)
        {
            CanPressButton = false;
        }
    }

    public void ButtonSound()
    {
        ButtonSoundSource.PlayOneShot(clip);
        Mark.SetActive(true);
    }
}
