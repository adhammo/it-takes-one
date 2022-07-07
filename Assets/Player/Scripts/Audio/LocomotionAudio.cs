using UnityEngine;

public class LocomotionAudio : MonoBehaviour
{
    [Header("Audio")]
    [Tooltip("Player audio source")]
    public AudioSource AudioSource;
    [Tooltip("Player steps audio")]
    public AudioClip[] StepsAudio;

    public void PlayStep()
    {
        AudioSource.PlayOneShot(StepsAudio[Random.Range(0, StepsAudio.Length)]);
    }
}
