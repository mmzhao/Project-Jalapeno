using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Created by Eric Zhou
 * Handles triggers for hurtboxes and passes them to PlayerController
 * Handles Hurtbox initialization for further idiotproofing
 */
public class HurtBoxHandler : MonoBehaviour {
    public PlayerController pc;

    void Awake ()
    {
        pc = transform.root.GetComponent<PlayerController>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        pc.getHit(gameObject, other);
    }
}
