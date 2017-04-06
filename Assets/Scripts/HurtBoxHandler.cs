using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Created by Eric Zhou
 * Handles triggers for hurtboxes and passes them to PlayerController
 * Handles Hurtbox initialization for further idiotproofing
 */
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class HurtBoxHandler : MonoBehaviour {
    public PlayerController pc;

    void Awake ()
    {
        pc = transform.root.GetComponent<PlayerController>();
        Collider col = gameObject.GetComponent<Collider>();
        col.isTrigger = true;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        if (gameObject.tag == "Shield")
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print("lol");
        pc.getHit(gameObject, other);
    }
}
