using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    public AudioClip[] LightpunchSounds;
    public AudioClip[] HeavyPunchSounds;
    public AudioClip[] jumpSounds;
    public AudioClip[] UppercutSounds;
    public AudioClip[] DamageSounds;

    AudioSource audioSource;
    
    public void PlaylightPunchSound()
    {
        if(LightpunchSounds.Length < 1) return;
        audioSource.PlayOneShot(LightpunchSounds[Random.Range(0, LightpunchSounds.Length)]);
    }

    public void PlayJumpSound()
    {
        if(jumpSounds.Length < 1) return;
        audioSource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
    }

    public void PlayHeavyPunchSound()
    {
        if(HeavyPunchSounds.Length < 1) return;
        audioSource.PlayOneShot(HeavyPunchSounds[Random.Range(0, HeavyPunchSounds.Length)]);
    }

    public void PlayDamageSounds(){
        if(DamageSounds.Length < 1) return;
        audioSource.PlayOneShot(DamageSounds [Random.Range(0, DamageSounds.Length)]);
    }

    public void PlayUppercutSounds(){
        if (UppercutSounds.Length < 1) return;
        audioSource.PlayOneShot(UppercutSounds[Random.Range(0, UppercutSounds.Length)]);
    }


    // Start is called before the first frame update
    void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
