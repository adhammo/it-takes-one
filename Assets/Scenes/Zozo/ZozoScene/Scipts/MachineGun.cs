using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public Tower tower;
   
    void Start()
    {
    }

    
    void Update()
    {
        
    }

    public void KK()
    {
        tower.MachineGunSpy();
    }

    public void TT()
    {
        tower.CanonSpy();
    }
}
