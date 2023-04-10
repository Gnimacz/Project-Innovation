using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    public AudioClip[] LightpunchSounds;
    public AudioClip[] HeavyPunchSounds;
    public AudioClip[] jumpSounds;
    public AudioClip[] DamageSounds;

    AudioSource audioSource;
    
    public void PlaylightPunchSound()
    {
        audioSource.PlayOneShot(LightpunchSounds[Random.Range(0, LightpunchSounds.Length)]);
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSounds[Random.Range(0, jumpSounds.Length)]);
    }

    public void PlayHeavyPunchSound()
    {
        audioSource.PlayOneShot(HeavyPunchSounds[Random.Range(0, HeavyPunchSounds.Length)]);
    }

    public void PlayDamageSounds(){
        audioSource.PlayOneShot(DamageSounds [Random.Range(0, DamageSounds.Length)]);
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
