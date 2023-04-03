using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    public AudioClip[] punchSounds;

    AudioSource audioSource;
    
    public void PlayPunchSound()
    {
        audioSource.PlayOneShot(punchSounds[Random.Range(0, punchSounds.Length)]);
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
