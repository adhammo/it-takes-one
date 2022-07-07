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

            /* Transition to be Implemented Here */ 
              PlayerPrefs.SetInt("Level", 2);

              UnityEngine.SceneManagement.SceneManager.LoadScene("TarekScene"); 

             gameObject.SetActive(false);

            /*  UI to be Implemented Here  */




 

        }

    }


}
