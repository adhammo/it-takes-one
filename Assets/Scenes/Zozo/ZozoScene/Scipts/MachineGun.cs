using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public Tower tower;

    public AudioSource CanonAudioSource;
    public AudioSource MGAudioSource;

    public AudioClip[] Clips;

   
    public void KK()
    {
        tower.MachineGunSpy();
    }

    public void TT()
    {
        tower.CanonSpy();
    }

    public void CanonSound()
    {
        CanonAudioSource.PlayOneShot(Clips[0]);
    }

    public void MGSound()
    {
        MGAudioSource.PlayOneShot(Clips[0]);
    }
}
