using UnityEngine;

public class Audios : MonoBehaviour
{
    [Header("Audio")]
    [Tooltip("Player audio source")]
    public AudioSource AudioSource;
    [Tooltip("Id")]
    public string AudiosId;
    [Tooltip("Audios")]
    public AudioClip[] AudioClips;

    private int index = 0;
    public void Play(string id)
    {
        if (AudiosId != id) return;
        
        AudioSource.PlayOneShot(AudioClips[index++]);
        if (index == AudioClips.Length) index = 0;
    }
}
