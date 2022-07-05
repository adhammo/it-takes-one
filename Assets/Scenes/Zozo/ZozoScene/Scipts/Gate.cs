using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Gate : MonoBehaviour
{
    
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    public void OnTriggerEnter ( Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {

                 /*   Press Certian Key Cap to Trave to Tarek's World    */    

        }

    }
}
