using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVariables : MonoBehaviour {

    public int damage;
    public float knockback;
    public float hitstunTime;
    public float stunTime;
    public AttackAudioSFX audioSFX;
    public int rageGain;

    bool hit;

	// Use this for initialization
    void Awake ()
    {
        audioSFX = GetComponent<AttackAudioSFX>();
        hit = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int Damage()
    {
        return damage;
    }

    public bool Hit()
    {
        return hit;
    }

    public void ToggleHit()
    {
        hit = true;
    }

}
