using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explosive : MonoBehaviour
{
    public ParticleSystem effect;
    public GameObject Explo;

    void Start()
    {
        effect.Stop();
    }

    public void OnTriggerEnter ( Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {

            effect.Play();
            //Destroy(Explo,1f);

        }

    }
    

}
