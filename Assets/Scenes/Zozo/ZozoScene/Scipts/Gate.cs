using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Gate : MonoBehaviour
{

    public void OnTriggerEnter ( Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {

             gameObject.SetActive(false);

            /*  UI to be Implemented Here  */

            /* Transition to be Implemented Here */  

        }

    }


}
