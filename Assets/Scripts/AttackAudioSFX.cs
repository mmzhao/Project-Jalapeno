using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAudioSFX : MonoBehaviour {

    public AudioClip[] defaultSFX;
    public AudioClip[] onHitSFX;
    public AudioSource defaultAudioSource;
    public AudioSource onHitAudioSource;

    public bool autoPlayAudio;
    public int autoPlayIndex = -1; // -1 means play a random clip
    

    // Use this for initialization
    void Awake ()
    {
        defaultAudioSource = addAudioSource();
        onHitAudioSource = addAudioSource();
    }

    void Start () {
        if (autoPlayAudio) playDefaultClip(autoPlayIndex);
	}

    public AudioSource addAudioSource ()
    {
        return gameObject.AddComponent<AudioSource>();
    }

    public void playRandomDefaultClip ()
    {
        if (defaultSFX.Length == 0)
        {
            return;
        }
        defaultAudioSource.clip = defaultSFX[Random.Range(0, defaultSFX.Length)];
        defaultAudioSource.Play();
    }

    public void playRandomOnHitClip ()
    {
        if (onHitSFX.Length == 0)
        {
            return;
        }
        onHitAudioSource.clip = onHitSFX[Random.Range(0, onHitSFX.Length)];
        onHitAudioSource.Play();
    }

    public void playDefaultClip (int i)
    {
        if ( i < defaultSFX.Length && i >= 0)
        {
            defaultAudioSource.clip = defaultSFX[i];
            defaultAudioSource.Play();
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
            onHitAudioSource.clip = onHitSFX[i];
            onHitAudioSource.Play();
        }
        else
        {
            playRandomOnHitClip();
        }
    }
}
