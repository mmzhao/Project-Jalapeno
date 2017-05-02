using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AttackAudioSFX : MonoBehaviour {

    public AudioClip[] defaultSFX;
    public AudioClip[] onHitSFX;
    public AudioSource audioSource;

    public bool autoPlayAudio;
    public int autoPlayIndex = -1; // -1 means play a random clip
    

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        if (autoPlayAudio) playDefaultClip(autoPlayIndex);
	}

    public void playRandomDefaultClip ()
    {
        if (defaultSFX.Length == 0)
        {
            return;
        }
        audioSource.clip = defaultSFX[Random.Range(0, defaultSFX.Length)];
        audioSource.Play();
    }

    public void playRandomOnHitClip ()
    {
        if (onHitSFX.Length == 0)
        {
            return;
        }
        audioSource.clip = onHitSFX[Random.Range(0, onHitSFX.Length)];
        audioSource.Play();
    }

    public void playDefaultClip (int i)
    {
        if ( i < defaultSFX.Length && i >= 0)
        {
            audioSource.clip = defaultSFX[i];
            audioSource.Play();
        }
        else
        {
            playRandomDefaultClip();
        }
    }

    public void playOnHitClip (int i)
    {
        if (i < onHitSFX.Length && i >= 0)
        {
            audioSource.clip = onHitSFX[i];
            audioSource.Play();
        }
        else
        {
            playRandomOnHitClip();
        }
    }
}
