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
    private PlayerController pc;
    private EnemyController ec;
    public bool forEnemy;

    void Awake ()
    {
        if (forEnemy)
        {
            ec = transform.root.GetComponent<EnemyController>();
        }
        else
        {
            pc = transform.root.GetComponent<PlayerController>();
        }
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
        if (!forEnemy)
        {
            pc.getHit(gameObject, other);
        }
        else
        {
            ec.getHit(gameObject, other);
        }
    }
}
