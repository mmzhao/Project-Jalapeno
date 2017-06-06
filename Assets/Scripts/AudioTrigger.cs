using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class AudioTrigger : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip audioClip;
    public bool triggered;


    void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.tag == "Player") playAudio();
        }
    }

    void playAudio()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        triggered = true;
    }
}
