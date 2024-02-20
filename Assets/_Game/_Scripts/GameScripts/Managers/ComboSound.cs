using UnityEngine;

public class ComboSound : MonoBehaviour
{
    public AudioClip soundClip; // the sound clip to play
    public float pitchIncrement = 0.1f; // the amount to increase the pitch each time
    public float maxPitch = 2.0f; // the maximum pitch value
    public float minPitch = 1.0f; // the minimum pitch value
    public float defaultPitch = 1f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayComboSound()
    {
        if (audioSource && soundClip)
        {
            audioSource.clip = soundClip;
            audioSource.pitch += pitchIncrement;
            if (audioSource.pitch > maxPitch) audioSource.pitch = maxPitch;
            audioSource.Play();
        }
    }
    public void GoBackOnSound()
    {
        if (audioSource && soundClip)
        {
            audioSource.clip = soundClip;
            audioSource.pitch -= pitchIncrement;
            if (audioSource.pitch < defaultPitch) audioSource.pitch = defaultPitch;
            
            if (audioSource.isPlaying) audioSource.Stop();
            
            audioSource.Play();
        }
    }
    public void ResetPitch()
    {
        audioSource.pitch = defaultPitch;
    }
}
